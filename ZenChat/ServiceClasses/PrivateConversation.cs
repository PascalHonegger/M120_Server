// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ZenChat.ServiceClasses
{
	[DataContract]
	public abstract class PrivateConversation : IConversation
	{
		public IEnumerable<ChatMessage> Messages { get; set; }
		public IEnumerable<User> Members { get; set; }
	}
}