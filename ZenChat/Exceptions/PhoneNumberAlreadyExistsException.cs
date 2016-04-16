// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System.Runtime.Serialization;

namespace ZenChatService.Exceptions
{
	/// <summary>
	///     Chatraum konnte nicht gefunden werden.
	/// </summary>
	[DataContract]
	public class PhoneNumberAlreadyExistsException
	{
		/// <summary>
		/// </summary>
		public PhoneNumberAlreadyExistsException(string message = "Es existiert bereits ein Account mit dieser Telefonnummer!")
		{
			Message = message;
		}

		public string Message { get; }
	}
}