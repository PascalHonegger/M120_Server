// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ZenChat.Exceptions;
using ZenChat.Properties;
using ZenChat.ServiceClasses;

namespace ZenChat
{
	/// <summary>
	///     Die Implementation des <see cref="IZenChat" />
	/// </summary>
	public class ZenChat : IZenChat
	{
		/// <summary>
		///     Ladet den User anhand seiner Telefonnummer
		/// </summary>
		/// <param name="phoneNumber">Die Nummer des zu ladenden Users</param>
		/// <returns>Der geladene User, falls dieser existiert</returns>
		public User GetUser(string phoneNumber)
		{
			using (var connection = new SqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();

				var command = new SqlCommand("SELECT id_user, name, phone FROM [user] where phone = @phone", connection);

				command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar));

				command.Parameters["@phone"].Value = phoneNumber;

				var reader = command.ExecuteReader();

				if (reader.Read())
				{
					var id = reader.GetInt32(0);
					var name = reader.GetString(1);
					var phone = reader.GetString(2);
					return new User(id) {Name = name, PhoneNumber = phone};
				}
			}

			throw new UserNotFoundException();
		}

		/// <summary>
		///     Meldet einen User an. Gibt den nun angemeldeten User zurück.
		///     Funktioniert auch als Registrieren: Falls die E-Mail und der Username noch nicht verwendet werden, wird der User
		///     erstellt
		/// </summary>
		/// <param name="name">Name des Users</param>
		/// <param name="phone">Telefonnummer des Users</param>
		/// <returns>User und seine <see cref="User.Id" /></returns>
		public Tuple<int, User> Login(string phone, string name)
		{
			try
			{
				var user = GetUser(phone);

				//User exists
				return new Tuple<int, User>(user.Id, user);
			}
			catch (UserNotFoundException)
			{
				//Create new user

				using (var connection = new SqlConnection(Settings.Default.ConnectionString))
				{
					connection.Open();

					var command = new SqlCommand("INSERT INTO [user] VALUES(null, @name, @phone)", connection);

					command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar));
					command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));

					command.Parameters["@phone"].Value = phone;
					command.Parameters["@name"].Value = name;

					command.ExecuteNonQuery();

					//Get User-ID
					command = new SqlCommand("SELECT LAST_INSERT_ID() as Id", connection);

					var reader = command.ExecuteReader();

					var userId = -1;
					if (reader.Read())
					{
						userId = reader.GetInt32(0);
					}

					if (userId == -1)
					{
						throw new LoginFailedException();
					}

					var user = new User(userId) {Name = name, PhoneNumber = phone};

					return new Tuple<int, User>(userId, user);
				}
			}
		}

		public User LoginWithId(int id)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<User> GetFriends(int userId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<ChatRoom> GetAllChatRooms(int userId)
		{
			throw new NotImplementedException();
		}

		public ChatRoom GetChatRoom(int chatRoomId)
		{
			throw new NotImplementedException();
		}

		public ChatRoom CreateChatRoom(int userId)
		{
			throw new NotImplementedException();
		}

		public void InviteToChatRoom(int userId, string phoneNumber, int chatRoomId)
		{
			throw new NotImplementedException();
		}

		public ChatRoom WriteGroupChatMessage(int userId, int chatRoomId, string message)
		{
			throw new NotImplementedException();
		}

		public PrivateConversation GetPrivateConversation(int userId, string otherPhone)
		{
			throw new NotImplementedException();
		}

		public PrivateConversation WritePrivateChatMessage(int userId, string otherPhone, string message)
		{
			throw new NotImplementedException();
		}

		public void ReadChatMessage(int userId, int messageId)
		{
			throw new NotImplementedException();
		}

		public void RecieveChatMessage(int userId, int messageId)
		{
			throw new NotImplementedException();
		}
	}
}