// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;

namespace ZenChatService.Exceptions
{
	/// <summary>
	///     Chatraum konnte nicht gefunden werden.
	/// </summary>
	[Serializable]
	public class AlreadyFriendException : Exception
	{
		/// <summary>
		/// </summary>
		public AlreadyFriendException() : base("Du bist bereits mit diesem Benutzer befreundet!")
		{
		}
	}
}