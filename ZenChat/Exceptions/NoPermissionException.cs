// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;

namespace ZenChatService.Exceptions
{
	/// <summary>
	/// Chatraum konnte nicht gefunden werden.
	/// </summary>
	[Serializable]
	public class NoPermissionException : Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public NoPermissionException() : base("Zu wenig Rechte für die gewünschte Aktion!")
		{
		}
	}
}