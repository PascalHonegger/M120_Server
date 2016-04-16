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

				//Delete Friends
				var command = new SqlCommand("DELETE FROM [friendship] WHERE fk_user1 = @id OR fk_user2 = @id", connection);

				command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));

				command.Parameters["@id"].Value = id;

				command.ExecuteNonQuery();

				//Get own Messages
				command.CommandText = "SELECT id_message FROM [message] WHERE author = @id";

				var reader = command.ExecuteReader();

				var messages = new List<int>();

				while (reader.Read())
				{
					messages.Add(reader.GetInt32(0));
				}

				reader.Close();

				command.Parameters.Add(new SqlParameter("@message", SqlDbType.Int));

				//Delete own Messages
				foreach (var message in messages)
				{
					command.Parameters["@message"].Value = message;

					//Delete sent messages
					command.CommandText = "DELETE FROM [message_user] WHERE fk_message = @message";

					command.ExecuteNonQuery();

					//Delete message
					command.CommandText = "DELETE FROM [message] WHERE id_message = @message";

					command.ExecuteNonQuery();
				}

				command.Parameters.Clear();

				command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));

				command.Parameters["@id"].Value = id;

				//Delete received messages
				command.CommandText = "DELETE FROM message_user WHERE fk_user = @id";

				command.ExecuteNonQuery();

				//Delete User
				command.CommandText = "DELETE FROM [user] WHERE id_user = @id";

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
		public void TestChangeUsernameChangesUsername()
		{
			//Arrange
			var user = TemporaryUser;
			var newUsername = RandomString;
			
			//Act
			var updatedUser = UnitUnderTest.ChangeUsername(user.Id, newUsername);

			//Assert
			Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
			Assert.That(updatedUser.PhoneNumber, Is.EqualTo(user.PhoneNumber));
			Assert.That(updatedUser.Name, Is.EqualTo(newUsername));
		}

		[Test]
		public void TestChangePhonenumberChangesPhonenumber()
		{
			//Arrange
			var user = TemporaryUser;
			var newPhoneNumber = RandomString;

			//Act
			var updatedUser = UnitUnderTest.ChangePhoneNumber(user.Id, newPhoneNumber);

			//Assert
			Assert.That(updatedUser.Id, Is.EqualTo(user.Id));
			Assert.That(updatedUser.PhoneNumber, Is.EqualTo(newPhoneNumber));
			Assert.That(updatedUser.Name, Is.EqualTo(user.Name));
		}

		[Test]
		public void TestCannotChangePhonenumberToExistingPhonenumber()
		{
			//Arrange
			var user = TemporaryUser;
			var existingPhone = TemporaryUser.PhoneNumber;

			//Act & Assert
			Assert.Throws<PhoneNumberAlreadyExistsException>(() => UnitUnderTest.ChangePhoneNumber(user.Id, existingPhone));
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

		//TODO
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
		public void TestNewUserHasNoFriends()
		{
			//Arrange
			var randomUser = TemporaryUser;

			//Act
			var friends = UnitUnderTest.GetFriends(randomUser.Id).ToList();

			//Assert
			Assert.That(friends, Is.Empty);
			Assert.That(friends, Is.EqualTo(randomUser.Friends));

			//Cleanup
			DeleteUser(randomUser.Id);
		}

		[Test]
		public void TestAddFriendsAddsFriend()
		{
			//Arrange
			var randomUser = TemporaryUser;
			var otherUser = TemporaryUser;

			//Act
			UnitUnderTest.AddFriend(randomUser.Id, otherUser.PhoneNumber);

			//Assert
			Assert.That(randomUser.Friends, Is.Not.Empty);
			Assert.That(randomUser.Friends, Contains.Item(otherUser));
		}

		[Test]
		public void TestCannotAddFriendTwice()
		{
			//Arrange
			var randomUser = TemporaryUser;
			var otherUser = TemporaryUser;

			UnitUnderTest.AddFriend(randomUser.Id, otherUser.PhoneNumber);

			//Act & Assert
			Assert.Throws<AlreadyFriendException>(() => UnitUnderTest.AddFriend(randomUser.Id, otherUser.PhoneNumber));
		}

		[Test]
		public void TestRemoveFriendsRemovesFriend()
		{
			//Arrange
			var randomUser = TemporaryUser;
			var friend1 = TemporaryUser;
			var friend2 = TemporaryUser;
			var friend3 = TemporaryUser;

			UnitUnderTest.AddFriend(randomUser.Id, friend1.PhoneNumber);
			UnitUnderTest.AddFriend(randomUser.Id, friend2.PhoneNumber);
			UnitUnderTest.AddFriend(randomUser.Id, friend3.PhoneNumber);

			//Act
			UnitUnderTest.RemoveFriend(randomUser.Id, friend2.PhoneNumber);

			//Assert
			Assert.That(randomUser.Friends, Is.Not.Empty);
			Assert.That(randomUser.Friends, Contains.Item(friend1));
			Assert.That(randomUser.Friends, !Contains.Item(friend2));
			Assert.That(randomUser.Friends, Contains.Item(friend3));
		}

		[Test]
		public void TestGetPrivateConversationWithNewFriendIsEmpty()
		{
			//Arrange
			var randomUser = TemporaryUser;
			var friend = TemporaryUser;

			UnitUnderTest.AddFriend(randomUser.Id, friend.PhoneNumber);

			//Act
			var conversation = UnitUnderTest.GetPrivateConversation(randomUser.Id, friend.PhoneNumber);

			//Assert
			Assert.That(conversation.Messages, Is.Empty);
			Assert.That(conversation.Members, Contains.Item(randomUser).And.Contains(friend));
		}

		[Test]
		public void TestWritePrivateMessageAddsMessage()
		{
			//Arrange
			var randomUser = TemporaryUser;
			var friend = TemporaryUser;
			const string message = "Hallo Welt";

			UnitUnderTest.AddFriend(randomUser.Id, friend.PhoneNumber);

			//Act
			var conversation = UnitUnderTest.WritePrivateChatMessage(randomUser.Id, friend.PhoneNumber, message);

			//Assert
			Assert.That(conversation.Messages.Count(), Is.EqualTo(1));
			var msg = conversation.Messages.First();
			Assert.That(msg.Message, Is.EqualTo(message));
			Assert.That(msg.SentTo, Contains.Item(friend));
			Assert.That(msg.Author, Is.EqualTo(randomUser));
			Assert.That(msg.ReadBy, Is.Empty);
			Assert.That(msg.ArrivedAt, Is.Empty);
		}

		[Test]
		public void TestGetPrivateMessageContainsSentMessages()
		{
			//Arrange
			var randomUser = TemporaryUser;
			var friend = TemporaryUser;
			const string message = "Hallo Welt";

			UnitUnderTest.AddFriend(randomUser.Id, friend.PhoneNumber);

			UnitUnderTest.WritePrivateChatMessage(friend.Id, randomUser.PhoneNumber, message);

			//Act
			var conversation = UnitUnderTest.GetPrivateConversation(randomUser.Id, friend.PhoneNumber);

			//Assert
			Assert.That(conversation.Messages.Count(), Is.EqualTo(1));
			var msg = conversation.Messages.First();
			Assert.That(msg.Message, Is.EqualTo(message));
			Assert.That(msg.SentTo, Contains.Item(randomUser));
			Assert.That(msg.Author, Is.EqualTo(friend));
			Assert.That(msg.ReadBy, Is.Empty);
			Assert.That(msg.ArrivedAt, Is.Empty);
		}

		[Test]
		public void TestCannotWritePrivateMessageToNoFriends()
		{
			//Arrange
			var randomUser = TemporaryUser;
			var randomUser2 = TemporaryUser;
			const string message = "Hallo Welt";

			//Act & Assert
			Assert.Throws<NoPermissionException>(
				() => UnitUnderTest.WritePrivateChatMessage(randomUser.Id, randomUser2.PhoneNumber, message));
		}

		[Test]
		public void TestReceivePrivateMessage()
		{
			//Arrange
			var user = TemporaryUser;
			var friend = TemporaryUser;
			const string message = "Hallo Welt";

			UnitUnderTest.AddFriend(user.Id, friend.PhoneNumber);
			var conversation = UnitUnderTest.WritePrivateChatMessage(user.Id, friend.PhoneNumber, message);

			//Act
			UnitUnderTest.ReceiveChatMessage(friend.Id, conversation.Messages.First().Id);
			conversation = UnitUnderTest.GetPrivateConversation(user.Id, friend.PhoneNumber);

			//Assert
			Assert.That(conversation.Messages.Count(), Is.EqualTo(1));
			var msg = conversation.Messages.First();
			Assert.That(msg.Message, Is.EqualTo(message));
			Assert.That(msg.Author, Is.EqualTo(user));
			Assert.That(msg.SentTo, Contains.Item(friend));
			Assert.That(msg.ArrivedAt, Contains.Item(friend));
			Assert.That(msg.ReadBy, Is.Empty);
		}

		[Test]
		public void TestReadPrivateMessage()
		{
			//Arrange
			var user = TemporaryUser;
			var friend = TemporaryUser;
			const string message = "Hallo Welt";

			UnitUnderTest.AddFriend(user.Id, friend.PhoneNumber);
			var conversation = UnitUnderTest.WritePrivateChatMessage(user.Id, friend.PhoneNumber, message);

			//Act
			UnitUnderTest.ReadChatMessage(friend.Id, conversation.Messages.First().Id);
			conversation = UnitUnderTest.GetPrivateConversation(user.Id, friend.PhoneNumber);

			//Assert
			Assert.That(conversation.Messages.Count(), Is.EqualTo(1));
			var msg = conversation.Messages.First();
			Assert.That(msg.Message, Is.EqualTo(message));
			Assert.That(msg.Author, Is.EqualTo(user));
			Assert.That(msg.SentTo, Contains.Item(friend));
			Assert.That(msg.ArrivedAt, Contains.Item(friend));
			Assert.That(msg.ReadBy, Contains.Item(friend));
		}
	}
}