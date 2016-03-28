// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ZenChat.Properties;
using ZenChat.ServiceClasses;

namespace ZenChat
{
	/// <summary>
	///     Die Implementation des <see cref="IZenChat" />
	/// </summary>
	public class ZenChat : IZenChat
	{
		public User GetUser(string phoneNumber)
		{
			using (var connection = new SqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();

				var command = new SqlCommand("SELECT * FROM [user]", connection);

				var reader = command.ExecuteReader();

				while (reader.Read())
				{
					return new User(reader.GetInt32(0));
				}
			}

			throw new NotImplementedException();
		}

		public Tuple<string, User> Login(string name, string phone)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<User> GetFriends(User user)
		{
			throw new NotImplementedException();
		}

		public IConversation GetAllChatRooms(string userId)
		{
			throw new NotImplementedException();
		}

		public IConversation GetChatRoom(string chatRoomId)
		{
			throw new NotImplementedException();
		}

		public IConversation CreateChatRoom(string userId)
		{
			throw new NotImplementedException();
		}

		public IConversation JoinChatroom(string userId, string chatRoomId)
		{
			throw new NotImplementedException();
		}

		public IConversation WriteChatMessage(string userId, string chatRoomId, string message)
		{
			throw new NotImplementedException();
		}

		public IConversation ReadChatMessage(string userId, string messageId)
		{
			throw new NotImplementedException();
		}

		public IConversation RecieveChatMessage(string userId, string messageId)
		{
			throw new NotImplementedException();
		}
	}
}