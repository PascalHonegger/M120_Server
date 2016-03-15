// Copyright (c) 2016 Pascal Honegger
// All rights reserved.
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ZenChat.ServiceClasses
{
	[DataContract]
	public class User
	{
		[DataMember]
		public string Id { get; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string PhoneNumber { get; set; }

		[DataMember]
		public IEnumerable<User> Friends { get; set; } 

		public User(string id)
		{
			Id = id;
		}

		private bool Equals(User other)
		{
			return string.Equals(Id, other.Id);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((User) obj);
		}

		public override int GetHashCode()
		{
			return Id?.GetHashCode() ?? 0;
		}
	}
}