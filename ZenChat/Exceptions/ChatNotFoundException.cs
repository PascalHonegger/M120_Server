// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System.Runtime.Serialization;

namespace ZenChatService.Exceptions
{
	/// <summary>
	///     Chatraum konnte nicht gefunden werden.
	/// </summary>
	[DataContract]
	public class ChatNotFoundException
	{
		/// <summary>
		/// </summary>
		public ChatNotFoundException(string message = "Der gewünschte Chatraum konnte nicht gefunden werden")
		{
			Message = message;
		}

		public string Message { get; }
	}
}