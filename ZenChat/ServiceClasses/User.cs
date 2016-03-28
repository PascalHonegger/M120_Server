// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ZenChat.ServiceClasses
{
	[DataContract]
	public class User
	{
		/// <summary>
		///     Setzt die <see cref="Id" /> des Users
		/// </summary>
		/// <param name="id"></param>
		public User(int id)
		{
			Id = id;
		}

		private int Id { get; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string PhoneNumber { get; set; }

		/// <summary>
		///     Alle Freunde des Users
		/// </summary>
		public IEnumerable<User> Friends
		{
			get { throw new NotImplementedException(); }
		}

		private bool Equals(User other)
		{
			return Equals(Id, other.Id);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((User) obj);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}