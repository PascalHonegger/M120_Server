// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Net.Security;
using System.ServiceModel;

namespace ZenChatService.ServiceClasses
{
	/// <summary>
	///     Interface für den allgemeinen ZenChat
	/// </summary>
	[ServiceContract(
		Namespace = "http://zenchatservice.azurewebsites.net/ZenChat.svc",
		SessionMode = SessionMode.NotAllowed,
		ProtectionLevel = ProtectionLevel.None,
		Name = "ZenChatService")]
	public interface IZenChat
	{
		#region User

		/// <summary>
		/// Ändert den Username eines Users.
		/// </summary>
		/// <param name="userId">User</param>
		/// <param name="newUsername">Neuer Username</param>
		/// <returns></returns>
		[OperationContract]
		User ChangeUsername(int userId, string newUsername);

		/// <summary>
		///     Ladet den User anhand seiner Telefonnummer
		/// </summary>
		/// <param name="phoneNumber">Die Nummer des zu ladenden Users</param>
		/// <returns>Der geladene User, falls dieser existiert</returns>
		[OperationContract]
		User GetUser(string phoneNumber);

		/// <summary>
		///     Lädt einen User anhand der ID. Benutzt intern und für das "Angemeldet bleiben" Feature.
		/// </summary>
		/// <param name="id">ID des users</param>
		/// <returns>User, falls dieser existiert</returns>
		[OperationContract]
		User GetUserFromId(int id);

		/// <summary>
		///     Meldet einen User an. Gibt den nun angemeldeten User zurück.
		///     Funktioniert auch als Registrieren: Falls die E-Mail und der Username noch nicht verwendet werden, wird der User
		///     erstellt
		/// </summary>
		/// <param name="name">Name des Users</param>
		/// <param name="phone">Telefonnummer des Users</param>
		/// <returns>User und seine <see cref="User.Id" /></returns>
		[OperationContract]
		Tuple<int, User> Login(string phone, string name);

		#endregion

		#region Freunde

		/// <summary>
		///     Lädt die freunde eines Users.
		/// </summary>
		/// <param name="userId">User</param>
		/// <returns>Alle Freunde des Users</returns>
		[OperationContract]
		IEnumerable<User> GetFriends(int userId);

		/// <summary>
		///     Fügt einen Freund hinzu.
		/// </summary>
		/// <param name="userId">User</param>
		/// <param name="otherPhone">Hinzuzufügender Freund</param>
		[OperationContract]
		void AddFriend(int userId, string otherPhone);

		/// <summary>
		///     Entfernt einen Freund hinzu.
		/// </summary>
		/// <param name="userId">User</param>
		/// <param name="otherPhone">Zu entfernender Freund</param>
		[OperationContract]
		void RemoveFriend(int userId, string otherPhone);

		#endregion

		#region Chatraum

		/// <summary>
		///     Lädt alle Chats, welche der mitgegebene Spieler sehen kann.
		/// </summary>
		/// <param name="userId">Der Spieler, von welchem alle Lobbies geladen werden.</param>
		/// <returns></returns>
		[OperationContract]
		IEnumerable<ChatRoom> GetAllChatRooms(int userId);

		/// <summary>
		///     Lädt den mitgegebenen Chat.
		/// </summary>
		/// <param name="chatRoomId">Der zu ladende Chat</param>
		/// <param name="playerId">Der Spieler, an wen die Nachrichten gesendet wurden</param>
		/// <returns>Chat, falls dieser existiert</returns>
		[OperationContract]
		ChatRoom GetChatRoom(int chatRoomId, int playerId);

		/// <summary>
		///     Erstellt einen neuen Chat
		/// </summary>
		/// <param name="userId">Autor</param>
		/// <param name="topic">Thema</param>
		/// <returns>Ersteller Chat</returns>
		[OperationContract]
		ChatRoom CreateChatRoom(int userId, string topic);

		/// <summary>
		///     Lädt einen Freund zu einem chat ein
		/// </summary>
		/// <param name="userId">Der jetzige User</param>
		/// <param name="phoneNumber">Einzuladender</param>
		/// <param name="chatRoomId">Beizutretender Chat</param>
		[OperationContract]
		void InviteToChatRoom(int userId, string phoneNumber, int chatRoomId);

		/// <summary>
		///     Lädt einen Freund zu einem chat ein
		/// </summary>
		/// <param name="userId">Der jetzige User</param>
		/// <param name="phoneNumber">Einzuladender</param>
		/// <param name="chatRoomId">Beizutretender Chat</param>
		[OperationContract]
		void RemoveFromChatRoom(int userId, string phoneNumber, int chatRoomId);

		/// <summary>
		///     Schriebt eine Chat-Message in den mitgegebenen Chat
		/// </summary>
		/// <param name="userId">Autor</param>
		/// <param name="chatRoomId">Chat</param>
		/// <param name="message">Nachricht</param>
		/// <returns></returns>
		[OperationContract]
		ChatRoom WriteGroupChatMessage(int userId, int chatRoomId, string message);

		#endregion

		#region Privater Chat

		/// <summary>
		///     Lädt die aktuelle Konversation
		/// </summary>
		/// <param name="userId">Autor</param>
		/// <param name="otherPhone">Mit wem</param>
		/// <returns></returns>
		[OperationContract]
		PrivateConversation GetPrivateConversation(int userId, string otherPhone);

		/// <summary>
		///     Schriebt eine Chat-Message in den mitgegebenen Chat
		/// </summary>
		/// <param name="userId">Autor</param>
		/// <param name="otherPhone">Mit wem</param>
		/// <param name="message">Nachricht</param>
		/// <returns></returns>
		[OperationContract]
		PrivateConversation WritePrivateChatMessage(int userId, string otherPhone, string message);

		#endregion

		#region Nachricht

		/// <summary>
		///     Markiere eine Nachricht als gelesen
		/// </summary>
		/// <param name="userId">User</param>
		/// <param name="messageId">Nachricht</param>
		/// <returns></returns>
		[OperationContract]
		void ReadChatMessage(int userId, int messageId);

		/// <summary>
		///     Markiere eine Nachricht als empfangen
		/// </summary>
		/// <param name="userId">User</param>
		/// <param name="messageId">Nachricht</param>
		/// <returns></returns>
		[OperationContract]
		void RecieveChatMessage(int userId, int messageId);

		#endregion
	}
}