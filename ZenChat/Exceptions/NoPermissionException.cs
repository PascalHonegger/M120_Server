// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System.Runtime.Serialization;

namespace ZenChatService.Exceptions
{
	/// <summary>
	///     Chatraum konnte nicht gefunden werden.
	/// </summary>
	[DataContract]
	public class NoPermissionException
	{
		/// <summary>
		/// </summary>
		public NoPermissionException(string message = "Zu wenig Rechte für die gewünschte Aktion!")
		{
			Message = message;
		}

		public string Message { get; }
	}
}