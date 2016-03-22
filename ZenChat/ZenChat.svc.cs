// Copyright (c) 2016 Pascal Honegger
// All rights reserved.
using System;
using System.ServiceModel;
using ZenChat.ServiceClasses;

namespace ZenChat
{
	/// <summary>
	///    Die Implementation des <see cref="IZenChat"/>
	/// </summary>
	public class ZenChat : IZenChat
	{
		public User GetUser(string phoenNumber)
		{
			throw new NotImplementedException();
		}

		public PrivateUser Login(string name, string phone)
		{
			throw new NotImplementedException();
		}

		public ChatRoomBase GetAllChatRooms(string userId)
		{
			throw new NotImplementedException();
		}

		public ChatRoomBase GetChatRoom(string chatRoomId)
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

		public ChatRoomBase ReadChatMessage(string userId, string messageId)
		{
			throw new NotImplementedException();
		}

		public ChatRoomBase RecieveChatMessage(string userId, string messageId)
		{
			throw new NotImplementedException();
		}
	}
}
