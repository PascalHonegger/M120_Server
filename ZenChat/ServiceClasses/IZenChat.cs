// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;
using System.Collections.Generic;
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
	[ServiceKnownType(typeof(IConversation))]
	[ServiceKnownType(typeof(IEnumerable<IConversation>))]
	public interface IZenChat
	{
		#region user

		/// <summary>
		///     Ladet den User anhand seiner ID
		/// </summary>
		/// <param name="phoneNumber">Die Nummer des zu ladenden Users</param>
		/// <returns>Der geladene User, falls dieser existiert</returns>
		[OperationContract]
		User GetUser(string phoneNumber);

		/// <summary>
		///     Meldet einen User an. Gibt den nun angemeldeten User zurück.
		///     Funktioniert auch als Registrieren: Falls die E-Mail und der Username noch nicht verwendet werden, wird der User
		///     erstellt
		/// </summary>
		/// <param name="name">Name des Users</param>
		/// <param name="phone">Telefonnummer des Users</param>
		/// <returns>User und seine <see cref="User.Id" /></returns>
		[OperationContract]
		Tuple<string, User> Login(string name, string phone);

		/// <summary>
		///     Lädt die freunde eines Users.
		/// </summary>
		/// <param name="user">User</param>
		/// <returns>Alle Freunde des Users</returns>
		[OperationContract]
		IEnumerable<User> GetFriends(User user);

		#endregion

		#region Chatroom

		/// <summary>
		///     Lädt alle Chats, welche der mitgegebene Spieler sehen kann.
		/// </summary>
		/// <param name="userId">Der Spieler, von welchem alle Lobbies geladen werden.</param>
		/// <returns></returns>
		[OperationContract]
		IConversation GetAllChatRooms(string userId);

		/// <summary>
		///     Lädt den mitgegebenen Chat.
		/// </summary>
		/// <param name="chatRoomId">Der zu ladende Chat</param>
		/// <returns></returns>
		[OperationContract]
		IConversation GetChatRoom(string chatRoomId);

		/// <summary>
		///     Erstellt einen neuen Chat
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		[OperationContract]
		IConversation CreateChatRoom(string userId);

		/// <summary>
		///     Tritt einen Chat bei
		/// </summary>
		/// <param name="userId">User</param>
		/// <param name="chatRoomId">Beizutretender Chat</param>
		/// <returns></returns>
		[OperationContract]
		IConversation JoinChatroom(string userId, string chatRoomId);

		#endregion

		#region ChatMessages

		/// <summary>
		///     Schriebt eine Chat-Message in den mitgegebenen Chat
		/// </summary>
		/// <param name="userId">Autor</param>
		/// <param name="chatRoomId">Chat</param>
		/// <param name="message">Nachricht</param>
		/// <returns></returns>
		[OperationContract]
		IConversation WriteChatMessage(string userId, string chatRoomId, string message);

		/// <summary>
		///     Markiere eine Nachricht als gelesen
		/// </summary>
		/// <param name="userId">User</param>
		/// <param name="messageId">Nachricht</param>
		/// <returns></returns>
		[OperationContract]
		IConversation ReadChatMessage(string userId, string messageId);

		/// <summary>
		///     Markiere eine Nachricht als empfangen
		/// </summary>
		/// <param name="userId">User</param>
		/// <param name="messageId">Nachricht</param>
		/// <returns></returns>
		[OperationContract]
		IConversation RecieveChatMessage(string userId, string messageId);

		#endregion
	}
}