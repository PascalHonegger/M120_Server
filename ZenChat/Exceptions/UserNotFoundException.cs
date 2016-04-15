// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;

namespace ZenChatService.Exceptions
{
	/// <summary>
	///     Der Benutzer konnte nicht gefunden werden.
	/// </summary>
	[Serializable]
	public class UserNotFoundException : Exception
	{
		/// <summary>
		/// </summary>
		public UserNotFoundException() : base("Der gewünschte Benutzer konnte nicht gefunden werden")
		{
		}
	}
}