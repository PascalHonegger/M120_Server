// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using NUnit.Framework;

namespace ZenChatServiceTest
{
	[TestFixture]
	public abstract class UnitTestBase<T> where T : new()
	{
		[SetUp]
		public void SetUp()
		{
			UnitUnderTest = new T();
		}

		[TearDown]
		public void TearDown()
		{
		}

		protected T UnitUnderTest;
	}
}