// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ZenChat.ServiceClasses
{
	/// <summary>
	///     Das Interface f�r die Generelle Konversation. Eine Konversation verf�gt �ber Nachrichten und Mitglieder!
	/// </summary>
	public interface IConversation
	{
		[DataMember]
		IEnumerable<ChatMessage> Messages { get; set; }

		[DataMember]
		IEnumerable<User> Members { get; set; }
	}
}