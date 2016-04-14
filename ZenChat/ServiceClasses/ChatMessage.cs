// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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
		/// <param name="idMessageUser">ID_Message_User</param>
		public ChatMessage(int idMessageUser)
		{
			throw new NotImplementedException();
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
		public IEnumerable<User> ArrivedAt
		{
			get { return new List<User>(); }
		}

		/// <summary>
		///     Die Liste der User, welche diese Nachricht gelesen haben.
		/// </summary>
		[DataMember]
		public IEnumerable<User> ReadBy
		{
			get { return new List<User>(); }
		}

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