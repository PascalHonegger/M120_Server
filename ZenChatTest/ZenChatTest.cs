// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

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
	[TestFixture]
	public class ZenChatTest : UnitTestBase<ZenChat>
	{
		#region User
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

		[Test]
		public void TestLoginReturnsSameUserEveryTime()
		{
			//Arrange
			const string username = "TestUsername";
			const string phone = "Test 1";

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
			const string phone = "Test 2";

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
			const string phone = "Test 3";

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
			UnitUnderTest = new ZenChat();

			UnitUnderTest.GetUserFromId(id);

			//Act & Assert
			Assert.Throws<UserNotFoundException>(() => UnitUnderTest.GetUserFromId(id));
		}
		#endregion

		#region Chatraum

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

		[Test]
		public void TestGetAllChatRoomsReturnsAllChatrooms()
		{
			//Arrange
			const string username = "TestUsername";
			const string phone = "Test 4";

			//Act
			var user = UnitUnderTest.Login(phone, username).Item2;
			var createdChat1 = UnitUnderTest.CreateChatRoom(user.Id, "ExampleTopic");
			var createdChat2 = UnitUnderTest.CreateChatRoom(user.Id, "ExampleTopic2");

			//Assert
			var allChatRooms = UnitUnderTest.GetAllChatRooms(user.Id).ToList();

			Assert.That(allChatRooms.Contains(createdChat1));
			Assert.That(allChatRooms.Contains(createdChat2));

			//Cleanup
			DeleteChat(createdChat1.Id);
			DeleteChat(createdChat2.Id);
			DeleteUser(user.Id);
		}

		[Test]
		public void TestGetAllChatRoomsReturnsNoChatroomsIfNoExist()
		{
			//Arrange
			const string username = "TestUsername";
			const string phone = "Test 5";

			//Act
			var user = UnitUnderTest.Login(phone, username).Item2;

			//Assert
			var allChatRooms = UnitUnderTest.GetAllChatRooms(user.Id).ToList();

			Assert.That(allChatRooms, Is.Empty);

			//Cleanup
			DeleteUser(user.Id);
		}

		[Test]
		public void TestGetChatRoomReturnsCreatedChatroom()
		{
			//Arrange
			const string username = "TestUsername";
			const string phone = "Test 5";

			//Act
			var user = UnitUnderTest.Login(phone, username).Item2;
			var createdChat = UnitUnderTest.CreateChatRoom(user.Id, "ExampleTopic");
			
			//Assert
			var chatFromServer = UnitUnderTest.GetChatRoom(createdChat.Id, user.Id);

			Assert.That(chatFromServer, Is.EqualTo(createdChat));

			//Cleanup
			DeleteChat(createdChat.Id);
			DeleteUser(user.Id);
		}

		[Test]
		public void TestGetChatRoomThrowsException()
		{
			//Act & Assert
			Assert.Throws<ChatNotFoundException>(() => UnitUnderTest.GetChatRoom(0, 0));
		}

		[Test, Ignore("Work in Progress Pascal")]
		public void TestChatroomOnlyShowsMessagesThatWereSentToYou()
		{
			//Arrange

			//Act

			//Assert

			//Cleanup
		}

		/*
		ChatRoom GetChatRoom(int chatRoomId, int playerId);

		ChatRoom CreateChatRoom(int userId, string topic);

		void InviteToChatRoom(int userId, string phoneNumber, int chatRoomId);

		ChatRoom WriteGroupChatMessage(int userId, int chatRoomId, string message);
		*/
		#endregion
	}
}