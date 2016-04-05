// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;

namespace ZenChatService.Exceptions
{
	/// <summary>
	/// Chatraum konnte nicht gefunden werden.
	/// </summary>
	[Serializable]
	public class MemberNotFoundException : Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public MemberNotFoundException() : base("Der gewünschte Spieler war nie Mitglied dieses Chats!")
		{
		}
	}
}