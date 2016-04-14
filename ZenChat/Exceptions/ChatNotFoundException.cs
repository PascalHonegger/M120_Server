// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;

namespace ZenChatService.Exceptions
{
	/// <summary>
	///     Chatraum konnte nicht gefunden werden.
	/// </summary>
	[Serializable]
	public class ChatNotFoundException : Exception
	{
		/// <summary>
		/// </summary>
		public ChatNotFoundException() : base("Der gewünschte Chatraum konnte nicht gefunden werden")
		{
		}
	}
}