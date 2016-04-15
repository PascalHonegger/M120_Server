// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ZenChatService.Properties;

namespace ZenChatService.ServiceClasses
{
	/// <summary>
	///     Eine Chatnachricht. Ist generisch für Private und Gruppennachrichten.
	/// </summary>
	[DataContract]
	public class ChatMessage
	{
		/// <summary>
		///     Lädt eine Chatmessage.
		/// </summary>
		/// <param name="idMessage">ID_Message_User</param>
		public ChatMessage(int idMessage)
		{
			Id = idMessage;

			using (var connection = new SqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();

				//General Settings
				var command = new SqlCommand("SELECT author, message, created FROM [message] WHERE id_message = @messageId", connection);

				command.Parameters.Add(new SqlParameter("@messageId", SqlDbType.Int));

				command.Parameters["@messageId"].Value = idMessage;

				var reader = command.ExecuteReader();

				if (reader.Read())
				{
					Author = new User(reader.GetInt32(0));
					Message = reader.GetString(1);
					Created = reader.GetDateTime(2);
				}

				reader.Close();

				//SentTo, ReadBy, ArrivedAt
				command.CommandText = "SELECT fk_user, wasRead, wasReceived FROM message_user WHERE fk_message = @messageId";

				reader = command.ExecuteReader();

				var sentTo = new List<User>();
				var readBy = new List<User>();
				var receivedBy = new List<User>();

				while (reader.Read())
				{
					var target = new User(reader.GetInt32(0));
					sentTo.Add(target);
					if (reader.GetBoolean(1))
					{
						readBy.Add(target);
					}
					if (reader.GetBoolean(2))
					{
						receivedBy.Add(target);
					}
				}
				SentTo = sentTo;
				ReadBy = readBy;
				ArrivedAt = receivedBy;
			}
		}

		/// <summary>
		///     ID dieser Nachricht.
		/// </summary>
		[DataMember]
		public int Id { get; }

		/// <summary>
		///     Der Author dieser Nachricht.
		/// </summary>
		[DataMember]
		public User Author { get; }

		/// <summary>
		///     Die eigentliche Nachricht.
		/// </summary>
		[DataMember]
		public string Message { get; }

		/// <summary>
		///     Das Datum, an welchem diese Nachricht gesendet / erstellt wurde.
		/// </summary>
		[DataMember]
		public DateTime Created { get; }

		/// <summary>
		///     Die Liste der User, bei welcher diese Nachricht angekommen ist.
		/// </summary>
		[DataMember]
		public IEnumerable<User> ArrivedAt { get; private set; }

		/// <summary>
		///     Die Liste der User, welche diese Nachricht gelesen haben.
		/// </summary>
		[DataMember]
		public IEnumerable<User> ReadBy { get; private set; }

		/// <summary>
		///     Die Liste der User, an welche diese Nachricht gesendet wurde.
		/// </summary>
		[DataMember]
		public IEnumerable<User> SentTo { get; private set; }

		private bool Equals(ChatMessage other) => Equals(Id, other.Id);

		/// <summary>Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.</summary>
		/// <returns>true, wenn das angegebene Objekt und das aktuelle Objekt gleich sind, andernfalls false.</returns>
		/// <param name="obj">Das Objekt, das mit dem aktuellen Objekt verglichen werden soll. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((ChatMessage) obj);
		}

		/// <summary>Fungiert als die Standardhashfunktion. </summary>
		/// <returns>Ein Hashcode für das aktuelle Objekt.</returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode() => Id.GetHashCode();
	}
}