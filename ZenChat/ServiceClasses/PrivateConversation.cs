// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ZenChat.ServiceClasses
{
	/// <summary>
	///     Eine Konversation zwischen zwei Freunden
	/// </summary>
	[DataContract]
	public abstract class PrivateConversation
	{
		public PrivateConversation(User member1, User member2)
		{
			Members = new List<User> {member1, member2};
		}

		[DataMember]
		public IEnumerable<ChatMessage> Messages
		{
			get { throw new NotImplementedException(); }
		}

		[DataMember]
		public IEnumerable<User> Members { get; }
	}
}