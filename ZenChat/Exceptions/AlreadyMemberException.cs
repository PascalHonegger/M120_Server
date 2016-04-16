// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System.Runtime.Serialization;

namespace ZenChatService.Exceptions
{
	/// <summary>
	///     Chatraum konnte nicht gefunden werden.
	/// </summary>
	[DataContract]
	public class AlreadyMemberException
	{
		/// <summary>
		/// </summary>
		public AlreadyMemberException(string message = "Der gewünschte User ist bereits ein Mitglied dieses Chats!")
		{
			Message = message;
		}

		public string Message { get; }
	}
}