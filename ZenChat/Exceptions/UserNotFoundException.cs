// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;

namespace ZenChat.Exceptions
{
	public class UserNotFoundException : Exception
	{
		public UserNotFoundException() : base("Der gewünschte Benutzer konnte nicht gefunden werden")
		{
		}
	}
}