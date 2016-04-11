// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using ZenChatService;
using ZenChatService.Exceptions;
using ZenChatService.Properties;
using ZenChatService.ServiceClasses;

namespace ZenChatServiceTest
{
	public class ZenChatTest : UnitTestBase<ZenChat>
	{
		protected override void DoOnSetUp()
		{
			//Nothing
		}

		protected override void DoOnTearDown()
		{
			foreach (var user in _temporaryUsers)
			{
				DeleteUser(user.Id);
			}

			_temporaryUsers.Clear();
		}

		#region TemporaryData

		private readonly List<User> _temporaryUsers = new List<User>();


		private User TemporaryUser
		{
			get
			{
				const string username = "Temporary User";
				var phone = RandomString;

				var user = UnitUnderTest.Login(phone, username).Item2;

				_temporaryUsers.Add(user);

				return user;
			}
		}

		private static void DeleteChat(int id)
		{
			using (var connection = new SqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();

				//Delete Chatroom_Users
				var command = new SqlCommand("DELETE FROM [chatroom_user] WHERE fk_chatroom = @id", connection);

				command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));

				command.Parameters["@id"].Value = id;

				command.ExecuteNonQuery();

				//Delete Chatrooms
				command.CommandText = "DELETE FROM [chatroom] WHERE id_chatroom = @id";

				command.ExecuteNonQuery();
			}
		}

		private static void DeleteUser(int id)
		{
			using (var connection = new SqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();

				var command = new SqlCommand("DELETE FROM [user] WHERE id_user = @id", connection);

				command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));

				command.Parameters["@id"].Value = id;

				command.ExecuteNonQuery();
			}
		}

		private static string RandomString
		{
			get
			{
				const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
				var random = new Random();
				return new string(Enumerable.Repeat(chars, 50)
				  .Select(s => s[random.Next(s.Length)]).ToArray());
			}
		}

		#endregion

		#region User

		[Test]
		public void TestLoginReturnsSameUserEveryTime()
		{
			//Arrange
			const string username = "TestUsername";
			var phone = RandomString;

			//Act
			var result = UnitUnderTest.Login(phone, username);
			var result2 = UnitUnderTest.Login(phone, username);

			//Assert
			Assert.That(result.Item2.Name, Is.EqualTo(username));
			Assert.That(result.Item2.PhoneNumber, Is.EqualTo(phone));
			Assert.That(result.Item2, Is.EqualTo(result2.Item2));
			Assert.That(result.Item1, Is.EqualTo(result2.Item1));

			//Cleanup
			DeleteUser(result.Item1);
		}

		[Test]
		public void TestGetUserReturnsExistingUser()
		{
			//Arrange
			const string username = "TestUsername";
			var phone = RandomString;

			//Act
			var result = UnitUnderTest.Login(phone, username);
			var result2 = UnitUnderTest.GetUser(phone);

			//Assert
			Assert.That(result.Item2.Name, Is.EqualTo(username));
			Assert.That(result.Item2.PhoneNumber, Is.EqualTo(phone));
			Assert.That(result.Item2, Is.EqualTo(result2));
			Assert.That(result.Item1, Is.EqualTo(result2.Id));

			//Cleanup
			DeleteUser(result.Item1);
		}

		[Test]
		public void TestGetUserThrowsExceptionIfWrongUser()
		{
			//Arrange
			const string phone = "This Phone Number will never exist!";

			//Act & Assert
			Assert.Throws<UserNotFoundException>(() => UnitUnderTest.GetUser(phone));
		}

		[Test]
		public void TestGetUserFromIdReturnsExistingUser()
		{
			//Arrange
			const string username = "TestUsername";
			var phone = RandomString;

			//Act
			var result = UnitUnderTest.Login(phone, username);
			var result2 = UnitUnderTest.GetUserFromId(result.Item1);

			//Assert
			Assert.That(result.Item2.Name, Is.EqualTo(username));
			Assert.That(result.Item2.PhoneNumber, Is.EqualTo(phone));
			Assert.That(result.Item2, Is.EqualTo(result2));
			Assert.That(result.Item1, Is.EqualTo(result2.Id));

			//Cleanup
			DeleteUser(result.Item1);
		}

		[Test]
		public void TestGetUserFromIdThrowsExceptionIfWrongUser()
		{
			//Arrange
			const int id = 1;

			//Act & Assert
			Assert.Throws<UserNotFoundException>(() => UnitUnderTest.GetUserFromId(id));
		}
		#endregion

		#region Chatraum

		[Test]
		public void TestGetAllChatRoomsReturnsAllChatrooms()
		{
			//Arrange
			var user = TemporaryUser;
			var createdChat1 = UnitUnderTest.CreateChatRoom(user.Id, "ExampleTopic");
			var createdChat2 = UnitUnderTest.CreateChatRoom(user.Id, "ExampleTopic2");


			//Act
			var allChatRooms = UnitUnderTest.GetAllChatRooms(user.Id).ToList();

			//Assert
			Assert.That(allChatRooms.Contains(createdChat1));
			Assert.That(allChatRooms.Contains(createdChat2));

			//Cleanup
			DeleteChat(createdChat1.Id);
			DeleteChat(createdChat2.Id);
		}

		[Test]
		public void TestGetAllChatRoomsReturnsNoChatroomsIfNoExist()
		{
			//Arrange
			var user = TemporaryUser;

			//Act
			var allChatRooms = UnitUnderTest.GetAllChatRooms(user.Id).ToList();

			//Assert
			Assert.That(allChatRooms, Is.Empty);
		}

		[Test]
		public void TestGetChatRoomReturnsCreatedChatroom()
		{
			//Arrange
			var user = TemporaryUser;
			var createdChat = UnitUnderTest.CreateChatRoom(user.Id, "ExampleTopic");

			//Act
			var chatFromServer = UnitUnderTest.GetChatRoom(createdChat.Id, user.Id);

			//Assert
			Assert.That(chatFromServer, Is.EqualTo(createdChat));

			//Cleanup
			DeleteChat(createdChat.Id);
		}

		[Test]
		public void TestGetChatRoomThrowsException()
		{
			//Act & Assert
			Assert.Throws<ChatNotFoundException>(() => UnitUnderTest.GetChatRoom(0, 0));
		}

		[Test]
		public void TestCreatedChatRoomContainsMember()
		{
			//Arrange
			var user = TemporaryUser;
			const string topic = "ExampleTopic";

			//Act
			var createdChat = UnitUnderTest.CreateChatRoom(user.Id, topic);

			//Assert
			Assert.That(createdChat.Admin, Is.EqualTo(user));
			Assert.That(createdChat.CanWriteMessages);
			Assert.That(createdChat.Members, Contains.Item(user));
			Assert.That(createdChat.Topic, Is.EqualTo(topic));
			Assert.That(createdChat.Messages, Is.Empty);

			//Cleanup
			DeleteChat(createdChat.Id);
		}

		/*
		[Test]
		public void TestChatroomOnlyShowsMessagesThatWereSentToYou()
		{
			//Arrange

			//Act

			//Assert

			//Cleanup
		}
		*/

		[Test, Ignore("TODO PHO")]
		public void TestInviteToChatRoomAddsMember()
		{
			//Arrange
			const string topic = "ExampleTopic";
			var user = TemporaryUser;
			var user2 = TemporaryUser;
			var createdChat = UnitUnderTest.CreateChatRoom(user.Id, topic);

			//Act
			UnitUnderTest.InviteToChatRoom(user.Id, user2.PhoneNumber, createdChat.Id);

			//Assert
			Assert.That(createdChat.Admin, Is.EqualTo(user));
			Assert.That(createdChat.CanWriteMessages);
			Assert.That(createdChat.Members, Contains.Item(user));
			Assert.That(createdChat.Members, Contains.Item(user2));
			Assert.That(createdChat.Topic, Is.EqualTo(topic));
			Assert.That(createdChat.Messages, Is.Empty);

			//Cleanup
			DeleteChat(createdChat.Id);
		}

		/*
		void InviteToChatRoom(int userId, string phoneNumber, int chatRoomId);

		void RemoveFromChatRoom(int userId, string phoneNumber, int chatRoomId);

		ChatRoom WriteGroupChatMessage(int userId, int chatRoomId, string message);
		*/
		#endregion
	}
}