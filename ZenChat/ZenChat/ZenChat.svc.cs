// Copyright (c) 2016 Pascal Honegger
// All rights reserved.
using System;
using ZenChat.ServiceClasses;

namespace ZenChat
{
	public class ZenChat : IZenChat
	{
		public User GetUser(string userId)
		{
			throw new NotImplementedException();
		}

		public User Login(string mail, string name)
		{
			throw new NotImplementedException();
		}

		public ChatRoomBase GetChatRoom(string userId, string chatRoomId)
		{
			throw new NotImplementedException();
		}

		public ChatRoomBase CreateChatRoom(string userId)
		{
			throw new NotImplementedException();
		}

		public ChatRoomBase JoinChatroom(string userId, string chatRoomId)
		{
			throw new NotImplementedException();
		}

		public ChatRoomBase WriteChatMessage(string userId, string chatRoomId, string message)
		{
			throw new NotImplementedException();
		}

		public ChatRoomBase ReadChatMessage(string userId, string chatRoomId, string messageId)
		{
			throw new NotImplementedException();
		}

		public ChatRoomBase RecieveChatMessage(string userId, string chatRoomId, string messageId)
		{
			throw new NotImplementedException();
		}
	}
}
