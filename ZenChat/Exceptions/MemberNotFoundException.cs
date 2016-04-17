// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System.Runtime.Serialization;

namespace ZenChatService.Exceptions
{
	/// <summary>
	///     Benutzer konnte nicht in den Mitgliedern gefunden werden.
	/// </summary>
	[DataContract]
	public class MemberNotFoundException
	{
		/// <summary>
		/// </summary>
		public MemberNotFoundException(string message = "Der gewünschte Benutzer ist kein Mitglied dieses Chats!")
		{
			Message = message;
		}

		public string Message { get; }
	}
}