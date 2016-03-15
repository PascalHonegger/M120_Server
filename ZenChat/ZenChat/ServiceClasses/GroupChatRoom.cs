// Copyright (c) 2016 Pascal Honegger
// All rights reserved.
using System.Runtime.Serialization;
using ZenChat.ServiceClasses;

namespace ZenChat
{
	public abstract class GroupChatRoom : ChatRoomBase
	{
		public GroupChatRoom(string id) : base(id)
		{

		}

		public User Admin { get; set; }
	}
}