// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ZenChatService.Exceptions;
using ZenChatService.Properties;

namespace ZenChatService.ServiceClasses
{
	/// <summary>
	///     Ein Chatraum, welcher eine beliebige Anzahl Users beinhalten kann.
	/// </summary>
	[DataContract]
	public class ChatRoom
	{
		private readonly int _playerId;

		/// <summary>
		///     Setzt die Werte des Chatraums.
		/// </summary>
		/// <param name="id">ID des Chatraums</param>
		/// <param name="playerId">
		///     ID des Spielers, welcher den Chatraum sehen möchte. Diese wird benötigt, das man nur die
		///     Nachrichten sehen kann, welche an einen gesendet wurden-
		/// </param>
		public ChatRoom(int id, int playerId)
		{
			Id = id;

			_playerId = playerId;

			ToFullChatroom();
		}

		/// <summary>
		///     Ersteller / Admin des Chatraumes
		/// </summary>
		[DataMember]
		public User Admin { get; private set; }

		/// <summary>
		///     ID des Chatraumes
		/// </summary>
		[DataMember]
		public int Id { get; }

		/// <summary>
		///     Das Thema des Chats. Kann beim Erstellen eines Chats gesetzt werden.
		/// </summary>
		[DataMember]
		public string Topic { get; }

		/// <summary>
		///     An welchem Datum dieser Chatraum erstellt wurde.
		/// </summary>
		[DataMember]
		public DateTime Created { get; private set; }

		/// <summary>
		///     Alle Nachrichten, welchen in diesem Chatraum an den spezifischen <see cref="_playerId" /> gesendet wurde.
		/// </summary>
		[DataMember]
		public IEnumerable<ChatMessage> Messages
		{
			get
			{
				using (var connection = new SqlConnection(Settings.Default.ConnectionString))
				{
					connection.Open();

					var command =
						new SqlCommand(
							"SELECT id_message_user FROM [message_user] where fk_chatroom = @chatroomId and fk_user = @userId", connection);

					command.Parameters.Add(new SqlParameter("@chatroomId", SqlDbType.Int));
					command.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int));

					command.Parameters["@chatroomId"].Value = Id;
					command.Parameters["@userID"].Value = _playerId;

					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						var idMessageUser = reader.GetInt32(0);
						yield return new ChatMessage(idMessageUser);
					}
				}
			}
		}

		/// <summary>
		///     Alle Mitglieder dieses Chatraumes. Beinhaltet nicht den Ersteller (TODO Abklären)!
		/// </summary>
		[DataMember]
		public IEnumerable<User> Members
		{
			get
			{
				using (var connection = new SqlConnection(Settings.Default.ConnectionString))
				{
					connection.Open();

					var command = new SqlCommand("SELECT fk_user FROM [chatroom_user] where fk_chatroom = @chatroomId", connection);

					command.Parameters.Add(new SqlParameter("@chatroomId", SqlDbType.Int));

					command.Parameters["@chatroomId"].Value = Id;

					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						var idUser = reader.GetInt32(0);
						yield return new User(idUser);
					}
				}
			}
		}

		private void ToFullChatroom()
		{
			using (var connection = new SqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();

				var command = new SqlCommand("SELECT admin, created FROM [chatroom] where id_chatroom = @id", connection);

				command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));

				command.Parameters["@id"].Value = Id;

				var reader = command.ExecuteReader();

				if (reader.Read())
				{
					var adminId = reader.GetInt32(0);
					Admin = new User(adminId);

					Created = reader.GetDateTime(1);
				}
				else
				{
					throw new ChatNotFoundExcetion();
				}
			}
		}

		private bool Equals(ChatRoom other)
		{
			return Id == other.Id;
		}


		/// <summary>Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.</summary>
		/// <returns>true, wenn das angegebene Objekt und das aktuelle Objekt gleich sind, andernfalls false.</returns>
		/// <param name="obj">Das Objekt, das mit dem aktuellen Objekt verglichen werden soll. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((ChatRoom) obj);
		}

		/// <summary>
		///     Fungiert als die Standardhashfunktion.
		/// </summary>
		/// <returns>
		///     Ein Hashcode für das aktuelle Objekt.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}