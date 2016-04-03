// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System;

namespace ZenChat.Exceptions
{
	public class ChatNotFoundExcetion : Exception
	{
		public ChatNotFoundExcetion() : base("Der gewünschte Chatraum konnte nicht gefunden werden")
		{
		}
	}
}