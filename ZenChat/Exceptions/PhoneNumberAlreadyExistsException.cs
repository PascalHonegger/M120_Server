// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;

namespace ZenChatService.Exceptions
{
	/// <summary>
	///     Chatraum konnte nicht gefunden werden.
	/// </summary>
	[Serializable]
	public class PhoneNumberAlreadyExistsException : Exception
	{
		/// <summary>
		/// </summary>
		public PhoneNumberAlreadyExistsException() : base("Es existiert bereits ein Account mit dieser Telefonnummer!")
		{
		}
	}
}