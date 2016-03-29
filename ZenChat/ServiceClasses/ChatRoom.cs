// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ZenChat.ServiceClasses
{
	[DataContract]
	public class ChatRoom
	{
		public ChatRoom(int id)
		{
			Id = id;
		}

		[DataMember]
		public User Admin { get; set; }

		[DataMember]
		public int Id { get; }

		[DataMember]
		public IEnumerable<ChatMessage> Messages
		{
			get { throw new NotImplementedException(); }
		}

		[DataMember]
		public IEnumerable<User> Members
		{
			get { throw new NotImplementedException(); }
		}

		private bool Equals(ChatRoom other)
		{
			return Id == other.Id;
		}

		/// <summary>
		///     Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.
		/// </summary>
		/// <returns>
		///     true, wenn das angegebene Objekt und das aktuelle Objekt gleich sind, andernfalls false.
		/// </returns>
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