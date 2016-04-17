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
	///     Die Klasse eines Users. Beinhaltet alle notwendigen Informationen über einen User.
	/// </summary>
	[DataContract]
	public class ChatMessage
	{
		/// <summary>
		///     Lädt eine Chatmessage.
		/// </summary>
		/// <param name="id">Id der Nachricht</param>
		public ChatMessage(int id)
		{
			Id = id;
			ToFullChatMessage();
		}

		/// <summary>
		///     ID dieser Nachricht.
		/// </summary>
		[DataMember]
		public int Id { get; set; }

		/// <summary>
		///     Lädt alle Daten des Users nach.
		/// </summary>
		/// <exception cref="UserNotFoundException">Es wurde kein User mit der angegebenen ID gefunden.</exception>
		private void ToFullChatMessage()
		{
			using (var connection = new SqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();

				//General Settings
				var command = new SqlCommand("SELECT author, message, created FROM [message] WHERE id_message = @messageId",
					connection);

				command.Parameters.Add(new SqlParameter("@messageId", SqlDbType.Int));

				command.Parameters["@messageId"].Value = Id;

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

				var sentTo = new List<User> { Author };
				var readBy = new List<User> { Author };
				var receivedBy = new List<User> { Author };

				while (reader.Read())
				{
					if (!reader.IsDBNull(0))
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
				}
				SentTo = sentTo;
				ReadBy = readBy;
				ArrivedAt = receivedBy;
			}
		}

		/// <summary>
		///     Der Author dieser Nachricht.
		/// </summary>
		[DataMember]
		public User Author { get; set; }

		/// <summary>
		///     Die eigentliche Nachricht.
		/// </summary>
		[DataMember]
		public string Message { get; set; }

		/// <summary>
		///     Das Datum, an welchem diese Nachricht gesendet / erstellt wurde.
		/// </summary>
		[DataMember]
		public DateTime Created { get; set; }

		/// <summary>
		///     Die Liste der User, bei welcher diese Nachricht angekommen ist.
		/// </summary>
		[DataMember]
		public IEnumerable<User> ArrivedAt { get;  set; }

		/// <summary>
		///     Die Liste der User, welche diese Nachricht gelesen haben.
		/// </summary>
		[DataMember]
		public IEnumerable<User> ReadBy { get; set; }

		/// <summary>
		///     Die Liste der User, an welche diese Nachricht gesendet wurde.
		/// </summary>
		[DataMember]
		public IEnumerable<User> SentTo { get; set; }

		private bool Equals(User other)
		{
			return Equals(Id, other.Id);
		}

		/// <summary>Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.</summary>
		/// <returns>true, wenn das angegebene Objekt und das aktuelle Objekt gleich sind, andernfalls false.</returns>
		/// <param name="obj">Das Objekt, das mit dem aktuellen Objekt verglichen werden soll. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((User)obj);
		}

		/// <summary>Fungiert als die Standardhashfunktion. </summary>
		/// <returns>Ein Hashcode für das aktuelle Objekt.</returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode() => Id.GetHashCode();
	}
}