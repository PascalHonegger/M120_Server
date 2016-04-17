// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System.Runtime.Serialization;

namespace ZenChatService.Exceptions
{
	/// <summary>
	///     Der Benutzer konnte nicht gefunden werden.
	/// </summary>
	[DataContract]
	public class UserNotFoundException
	{
		/// <summary>
		/// </summary>
		public UserNotFoundException(string message = "Der gewünschte Benutzer konnte nicht gefunden werden")
		{
			Message = message;
		}

		/// <summary>
		///     Die Nachricht des Fehlers
		/// </summary>
		public string Message { get; }
	}
}