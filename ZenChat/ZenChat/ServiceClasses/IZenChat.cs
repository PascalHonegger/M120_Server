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

		/// <summary>
		///    Ladet den User anhand seiner ID
		/// </summary>
		/// <param name="phoenNumber">Die Nummer des zu ladenden Users</param>
		/// <returns>Der geladene User, falls dieser existiert</returns>
		[OperationContract]
		User GetUser(string phoenNumber);

		/// <summary>
		///     Meldet einen User an. Gibt den nun angemeldeten User zurück. 
		///     Funktioniert auch als Registrieren: Falls die E-Mail und der Username noch nicht verwendet werden, wird der User erstellt
		/// </summary>
		/// <param name="name">Name des Users</param>
		/// <param name="phone">Name des Users</param>
		/// <returns></returns>
		[OperationContract]
		PrivateUser Login(string name, string phone);

		#endregion

		#region Chatroom

		/// <summary>
		///     Lädt alle Chats, welche der mitgegebene Spieler sehen kann.
		/// </summary>
		/// <param name="userId">Der Spieler, von welchem alle Lobbies geladen werden.</param>
		/// <returns></returns>
		[OperationContract]
		ChatRoomBase GetAllChatRooms(string userId);

		/// <summary>
		///     Lädt den mitgegebenen Chat.
		/// </summary>
		/// <param name="chatRoomId">Der zu ladende Chat</param>
		/// <returns></returns>
		[OperationContract]
		ChatRoomBase GetChatRoom(string chatRoomId);

		/// <summary>
		///     Erstellt einen neuen Chat
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		[OperationContract]
		ChatRoomBase CreateChatRoom(string userId);

		/// <summary>
		///     Tritt einen Chat bei
		/// </summary>
		/// <param name="userId">User</param>
		/// <param name="chatRoomId">Beizutretender Chat</param>
		/// <returns></returns>
		[OperationContract]
		ChatRoomBase JoinChatroom(string userId, string chatRoomId);

		#endregion

		#region ChatMessages

		/// <summary>
		/// Schriebt eine Chat-Message in den mitgegebenen Chat
		/// </summary>
		/// <param name="userId">Autor</param>
		/// <param name="chatRoomId">Chat</param>
		/// <param name="message">Nachricht</param>
		/// <returns></returns>
		[OperationContract]
		ChatRoomBase WriteChatMessage(string userId, string chatRoomId, string message);

		/// <summary>
		/// Markiere eine Nachricht als gelesen
		/// </summary>
		/// <param name="userId">User</param>
		/// <param name="messageId">Nachricht</param>
		/// <returns></returns>
		[OperationContract]
		ChatRoomBase ReadChatMessage(string userId, string messageId);

		/// <summary>
		/// Markiere eine Nachricht als empfangen
		/// </summary>
		/// <param name="userId">User</param>
		/// <param name="messageId">Nachricht</param>
		/// <returns></returns>
		[OperationContract]
		ChatRoomBase RecieveChatMessage(string userId, string messageId);

		#endregion
	}
}