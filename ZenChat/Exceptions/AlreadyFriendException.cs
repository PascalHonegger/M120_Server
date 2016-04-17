// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System.Runtime.Serialization;

namespace ZenChatService.Exceptions
{
	/// <summary>
	///     Chatraum konnte nicht gefunden werden.
	/// </summary>
	[DataContract]
	public class AlreadyFriendException
	{
		/// <summary>
		/// </summary>
		public AlreadyFriendException(string message = "Du bist bereits mit diesem Benutzer befreundet!")
		{
			Message = message;
		}

		public string Message { get; }
	}
}