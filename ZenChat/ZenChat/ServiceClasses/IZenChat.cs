// Copyright (c) 2016 Pascal Honegger
// All rights reserved.
using System.ServiceModel;

namespace ZenChat.ServiceClasses
{
	/// <summary>
	///     Interface für den allgemeinen ZenChat
	/// </summary>
	[ServiceContract(
		Name = "ZenChat",
		ConfigurationName = "ZenChat",
		Namespace = "http://zenchatservice.azurewebsites.net/ZenChat.svc",
		SessionMode = SessionMode.NotAllowed)]
	public interface IZenChat
	{
		#region user

		[OperationContract]
		User GetUser(string userId);

		[OperationContract]
		User Login(string mail, string name);

		#endregion

		#region Chatroom

		[OperationContract]
		ChatRoomBase GetChatRoom(string userId, string chatRoomId);

		[OperationContract]
		ChatRoomBase CreateChatRoom(string userId);

		[OperationContract]
		ChatRoomBase JoinChatroom(string userId, string chatRoomId);

		#endregion

		#region ChatMessages

		[OperationContract]
		ChatRoomBase WriteChatMessage(string userId, string chatRoomId, string message);

		[OperationContract]
		ChatRoomBase ReadChatMessage(string userId, string chatRoomId, string messageId);

		[OperationContract]
		ChatRoomBase RecieveChatMessage(string userId, string chatRoomId, string messageId);

		#endregion
	}
}