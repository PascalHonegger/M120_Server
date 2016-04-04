// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ZenChatService.Exceptions;
using ZenChatService.Properties;
using ZenChatService.ServiceClasses;

namespace ZenChatService
{
	/// <summary>
	///     Die Implementation des <see cref="IZenChat" />
	/// </summary>
	public class ZenChat : IZenChat
	{
		#region User

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

				var command = new SqlCommand("SELECT id_user, name FROM [user] where phone = @phone", connection);

				command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar));

				command.Parameters["@phone"].Value = phoneNumber;

				var reader = command.ExecuteReader();

				if (reader.Read())
				{
					var id = reader.GetInt32(0);
					var name = reader.GetString(1);
					return new User(id, name, phoneNumber);
				}


				throw new UserNotFoundException();
			}
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
				//Try load existing user
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

					var command = new SqlCommand("INSERT INTO [user] (name, phone) OUTPUT INSERTED.id_user VALUES(@name, @phone)",
						connection);

					command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar));
					command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));

					command.Parameters["@phone"].Value = phone;
					command.Parameters["@name"].Value = name;

					var userId = (int) command.ExecuteScalar();

					var user = new User(userId, name, phone);

					return new Tuple<int, User>(userId, user);
				}
			}
		}

		/// <summary>
		///     Lädt einen User anhand der ID. Benutzt intern und für das "Angemeldet bleiben" Feature.
		/// </summary>
		/// <param name="id">ID des users</param>
		/// <returns>User, falls dieser existiert</returns>
		/// <exception cref="UserNotFoundException">Es wurde kein User mit der angegebenen ID gefunden.</exception>
		public User GetUserFromId(int id)
		{
			return new User(id);
		}

		#endregion

		#region Freunde

		/// <summary>
		///     Lädt die freunde eines Users.
		/// </summary>
		/// <param name="userId">User</param>
		/// <returns>Alle Freunde des Users</returns>
		public IEnumerable<User> GetFriends(int userId)
		{
			var user = GetUserFromId(userId);

			return user.Friends;
		}

		/// <summary>
		///     Fügt einen Freund hinzu.
		/// </summary>
		/// <param name="userId">User</param>
		/// <param name="otherPhone">Hinzuzufügender Freund</param>
		public void AddFriend(int userId, string otherPhone)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///     Entfernt einen Freund hinzu.
		/// </summary>
		/// <param name="userId">User</param>
		/// <param name="otherPhone">Zu entfernender Freund</param>
		public void RemoveFriend(int userId, string otherPhone)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Chatraum

		/// <summary>
		///     Lädt alle Chats, welche der mitgegebene Spieler sehen kann.
		/// </summary>
		/// <param name="userId">Der Spieler, von welchem alle Lobbies geladen werden.</param>
		/// <returns></returns>
		public IEnumerable<ChatRoom> GetAllChatRooms(int userId)
		{
			using (var connection = new SqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();

				var command = new SqlCommand("SELECT fk_chatroom FROM [chatroom_user] where fk_person = @userID", connection);

				command.Parameters.Add(new SqlParameter("@userID", SqlDbType.Int));

				command.Parameters["@userID"].Value = userId;

				var reader = command.ExecuteReader();

				while (reader.Read())
				{
					var chatroomId = reader.GetInt32(0);

					yield return GetChatRoom(chatroomId, userId);
				}
			}

			throw new UserNotFoundException();
		}

		/// <summary>
		///     Lädt den mitgegebenen Chat.
		/// </summary>
		/// <param name="chatRoomId">Der zu ladende Chat</param>
		/// <param name="playerId">Der Spieler, an wen die Nachrichten gesendet wurden</param>
		/// <returns>Chat, falls dieser existiert</returns>
		public ChatRoom GetChatRoom(int chatRoomId, int playerId)
		{
			return new ChatRoom(chatRoomId, playerId);
		}

		/// <summary>
		///     Erstellt einen neuen Chat
		/// </summary>
		/// <param name="userId">Autor</param>
		/// <param name="topic">Thema</param>
		/// <returns>Ersteller Chat</returns>
		public ChatRoom CreateChatRoom(int userId, string topic)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///     Lädt einen Freund zu einem chat ein
		/// </summary>
		/// <param name="userId">Der jetzige Use</param>
		/// <param name="phoneNumber">Einzuladender</param>
		/// <param name="chatRoomId">Beizutretender Chat</param>
		public void InviteToChatRoom(int userId, string phoneNumber, int chatRoomId)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///     Schriebt eine Chat-Message in den mitgegebenen Chat
		/// </summary>
		/// <param name="userId">Autor</param>
		/// <param name="chatRoomId">Chat</param>
		/// <param name="message">Nachricht</param>
		/// <returns></returns>
		public ChatRoom WriteGroupChatMessage(int userId, int chatRoomId, string message)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Privater Chat

		/// <summary>
		///     Lädt die aktuelle Konversation
		/// </summary>
		/// <param name="userId">Autor</param>
		/// <param name="otherPhone">Mit wem</param>
		/// <returns></returns>
		public PrivateConversation GetPrivateConversation(int userId, string otherPhone)
		{
			var user1 = GetUserFromId(userId);
			var user2 = GetUser(otherPhone);

			return new PrivateConversation(user1, user2);
		}

		/// <summary>
		///     Schriebt eine Chat-Message in den mitgegebenen Chat
		/// </summary>
		/// <param name="userId">Autor</param>
		/// <param name="otherPhone">Mit wem</param>
		/// <param name="message">Nachricht</param>
		/// <returns></returns>
		public PrivateConversation WritePrivateChatMessage(int userId, string otherPhone, string message)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Nachricht

		/// <summary>
		///     Markiere eine Nachricht als gelesen
		/// </summary>
		/// <param name="userId">User</param>
		/// <param name="messageId">Nachricht</param>
		/// <returns></returns>
		public void ReadChatMessage(int userId, int messageId)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///     Markiere eine Nachricht als empfangen
		/// </summary>
		/// <param name="userId">User</param>
		/// <param name="messageId">Nachricht</param>
		/// <returns></returns>
		public void RecieveChatMessage(int userId, int messageId)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}