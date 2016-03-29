// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;

namespace ZenChat.Exceptions
{
	public class LoginFailedException : Exception
	{
		public LoginFailedException() : base("Login fehlgeschlagen!")
		{
		}
	}
}