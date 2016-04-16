// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using ZenChatService.Properties;

namespace ZenChatService.ServiceClasses
{
	/// <summary>
	///     Eine Konversation zwischen zwei Freunden
	/// </summary>
	[DataContract]
	public class PrivateConversation
	{
		/// <summary>
		///     Initialisiert die Private Konversation.
		/// </summary>
		/// <param name="member1">Erster Teilnehmer</param>
		/// <param name="member2">Zweiter Teilnehmer</param>
		public PrivateConversation(User member1, User member2)
		{
			Members = new List<User> {member1, member2};
		}

		/// <summary>
		///     Alle Nachrichten, welche zwischen den beiden <see cref="Members" /> gesendet wurden.
		/// </summary>
		[DataMember]
		public IEnumerable<ChatMessage> Messages
		{
			get
			{
				using (var connection = new SqlConnection(Settings.Default.ConnectionString))
				{
					connection.Open();

					//Load received Messages
					var command = new SqlCommand("SELECT fk_message FROM [message_user] WHERE fk_user = @user1", connection);

					command.Parameters.Add(new SqlParameter("@user1", SqlDbType.Int));
					command.Parameters.Add(new SqlParameter("@user2", SqlDbType.Int));

					command.Parameters["@user1"].Value = Members.First().Id;
					command.Parameters["@user2"].Value = Members.Last().Id;

					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						var idMessage = reader.GetInt32(0);
						yield return new ChatMessage(idMessage);
					}
					reader.Close();

					//Load Sent Messages
					command.CommandText = "SELECT fk_message FROM [message_user] WHERE fk_user = @user2";

					reader = command.ExecuteReader();

					while (reader.Read())
					{
						var idMessage = reader.GetInt32(0);
						yield return new ChatMessage(idMessage);
					}
				}
			}
		}

		/// <summary>
		///     Die Mitgleider einer privaten Konversation. Sollte immer aus zwei user bestehen -> zwei Freunden.
		/// </summary>
		[DataMember]
		public IEnumerable<User> Members { get; }
	}
}