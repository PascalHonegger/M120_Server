// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;

namespace ZenChatService.Exceptions
{
	/// <summary>
	/// Chatraum konnte nicht gefunden werden.
	/// </summary>
	public class ChatNotFoundExcetion : Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public ChatNotFoundExcetion() : base("Der gewünschte Chatraum konnte nicht gefunden werden")
		{
		}
	}
}