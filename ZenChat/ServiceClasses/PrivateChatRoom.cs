// Copyright (c) 2016 Pascal Honegger
// All rights reserved.
using System.Runtime.Serialization;
using ZenChat.ServiceClasses;

namespace ZenChat
{
	public abstract class PrivateChatRoom : ChatRoomBase
	{
		public PrivateChatRoom(string id) : base(id)
		{

		}
	}
}