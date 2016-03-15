// Copyright (c) 2016 Pascal Honegger
// All rights reserved.
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ZenChat.ServiceClasses
{
	[DataContract]
	public abstract class ChatRoomBase
	{
		[DataMember]
		public string Id { get; }

		[DataMember]
		public IEnumerable<ChatMessage> Messages { get; set; }

		[DataMember]
		public IEnumerable<User> Members { get; set; }

		[DataMember]
		public DateTime Created { get; set; }

		protected ChatRoomBase(string id)
		{
			Id = id;
		}

		private bool Equals(ChatRoomBase other)
		{
			return string.Equals(Id, other.Id);
		}

		/// <summary>
		/// Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.
		/// </summary>
		/// <returns>
		/// true, wenn das angegebene Objekt und das aktuelle Objekt gleich sind, andernfalls false.
		/// </returns>
		/// <param name="obj">Das Objekt, das mit dem aktuellen Objekt verglichen werden soll. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((ChatRoomBase)obj);
		}

		/// <summary>
		/// Fungiert als die Standardhashfunktion. 
		/// </summary>
		/// <returns>
		/// Ein Hashcode für das aktuelle Objekt.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return Id?.GetHashCode() ?? 0;
		}
	}
}