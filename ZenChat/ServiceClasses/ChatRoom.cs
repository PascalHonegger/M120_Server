// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.ServiceModel;
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
		private readonly int _userId;

		/// <summary>
		///     Setzt die Werte des Chatraums.
		/// </summary>
		/// <param name="id">ID des Chatraums</param>
		/// <param name="userId">
		///     ID des Users, welcher den Chatraum sehen m�chte. Diese wird ben�tigt, das man nur die
		///     Nachrichten sehen kann, welche an einen gesendet wurden-
		/// </param>
		public ChatRoom(int id, int userId)
		{
			Id = id;

			_userId = userId;

			ToFullChatroom();
		}

		/// <summary>
		///     Ersteller / Admin des Chatraumes
		/// </summary>
		[DataMember]
		public User Admin { get; set; }

		/// <summary>
		///     ID des Chatraumes
		/// </summary>
		[DataMember]
		public int Id { get; set; }

		/// <summary>
		///     Das Thema des Chats. Kann beim Erstellen eines Chats gesetzt werden.
		/// </summary>
		[DataMember]
		public string Topic { get; set; }

		/// <summary>
		///     An welchem Datum dieser Chatraum erstellt wurde.
		/// </summary>
		[DataMember]
		public DateTime Created { get; set; }

		/// <summary>
		///     Alle Nachrichten, welchen in diesem Chatraum an den spezifischen <see cref="_userId" /> gesendet wurde.
		/// </summary>
		[DataMember]
		public IEnumerable<ChatMessage> Messages
		{
			get
			{
				using (var connection = new SqlConnection(Settings.Default.ConnectionString))
				{
					connection.Open();

					//Load Messages sent to you
					var command =
						new SqlCommand(
							"SELECT DISTINCT fk_message FROM [message_user] INNER JOIN [message] ON fk_message = id_message WHERE fk_chatroom = @chatroomId AND (fk_user = @userId OR author = @userId)",
							connection);

					command.Parameters.Add(new SqlParameter("@chatroomId", SqlDbType.Int));
					command.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int));

					command.Parameters["@chatroomId"].Value = Id;
					command.Parameters["@userId"].Value = _userId;

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
		///     Alle Mitglieder dieses Chatraumes.
		/// </summary>
		[DataMember]
		public IEnumerable<User> Members { get; private set; }

		/// <summary>
		///     True, falls er immernoch Mitglied ist. False, falls er ein Mitglied war.
		/// </summary>
		[DataMember]
		public bool CanWriteMessages { get; private set; }

		private void ToFullChatroom()
		{
			using (var connection = new SqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();

				//Load General Settings

				var command = new SqlCommand("SELECT admin, created, topic FROM [chatroom] WHERE id_chatroom = @id", connection);

				command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));

				command.Parameters["@id"].Value = Id;

				var reader = command.ExecuteReader();

				if (reader.Read())
				{
					var adminId = reader.GetInt32(0);
					Admin = new User(adminId);

					Created = reader.GetDateTime(1);
					Topic = reader.GetString(2);
				}
				else
				{
					var e = new ChatNotFoundException();
					throw new FaultException<ChatNotFoundException>(e, e.Message);
				}

				reader.Close();

				//Load Members

				command = new SqlCommand("SELECT fk_user, isMember FROM [chatroom_user] WHERE fk_chatroom = @chatroomId", connection);

				command.Parameters.Add(new SqlParameter("@chatroomId", SqlDbType.Int));

				command.Parameters["@chatroomId"].Value = Id;

				reader = command.ExecuteReader();

				var list = new List<User>();

				var everWasMember = false;

				while (reader.Read())
				{
					var idUser = reader.GetInt32(0);
					var isMember = reader.GetBoolean(1);
					//Player is Member
					if (_userId == idUser)
					{
						CanWriteMessages = isMember;
						everWasMember = true;
					}

					if (isMember)
					{
						list.Add(new User(idUser));
					}
				}

				if (!everWasMember)
				{
					var e = new MemberNotFoundException();
					throw new FaultException<MemberNotFoundException>(e, e.Message);
				}

				Members = list;
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
		///     Ein Hashcode f�r das aktuelle Objekt.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}