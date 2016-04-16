// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;

namespace ZenChatService.Exceptions
{
	/// <summary>
	///     Chatraum konnte nicht gefunden werden.
	/// </summary>
	[Serializable]
	public class AlreadyMemberException : Exception
	{
		/// <summary>
		/// </summary>
		public AlreadyMemberException() : base("Der gewünschte User ist bereits ein Mitglied dieses Chats!")
		{
		}
	}
}