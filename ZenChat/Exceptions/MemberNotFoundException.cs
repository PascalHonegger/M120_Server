// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;

namespace ZenChatService.Exceptions
{
	/// <summary>
	///     Benutzer konnte nicht in den Mitgliedern gefunden werden.
	/// </summary>
	[Serializable]
	public class MemberNotFoundException : Exception
	{
		/// <summary>
		/// </summary>
		public MemberNotFoundException() : base("Der gewünschte Benutzer ist kein Mitglied dieses Chats!")
		{
		}
	}
}