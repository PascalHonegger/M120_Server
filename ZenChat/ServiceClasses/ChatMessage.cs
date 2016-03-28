// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ZenChat.ServiceClasses
{
	[DataContract]
	public class ChatMessage
	{
		[DataMember]
		public int Id { get; }

		[DataMember]
		public User Author { get; set; }

		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public DateTime Created { get; set; }

		[DataMember]
		public IEnumerable<User> ArrivedAt { get; set; }

		[DataMember]
		public IEnumerable<User> ReadBy { get; set; }

		private bool Equals(ChatMessage other)
		{
			return Equals(Id, other.Id);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((ChatMessage) obj);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}