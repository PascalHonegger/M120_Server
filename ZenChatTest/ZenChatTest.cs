using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;
using ZenChatService;
using ZenChatService.Exceptions;
using ZenChatService.Properties;
using ZenChatService.ServiceClasses;

namespace ZenChatServiceTest
{
	[TestFixture]
	public class ZenChatTest : UnitTestBase<IZenChat>
	{
		#region User
		private static void DeleteUser(int id)
		{
			using (var connection = new SqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();

				var command = new SqlCommand("DELETE FROM [user] WHERE id_user = @id", connection);

				command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));

				command.Parameters["@id"].Value = id;

				command.ExecuteNonQuery();
			}
		}

		[Test]
		public void TestLoginReturnsSameUserEveryTime()
		{
			//Arrange
			const string username = "TestUsername";
			const string phone = "012 345 67 89";
			UnitUnderTest = new ZenChat();

			//Act
			var result = UnitUnderTest.Login(phone, username);
			var result2 = UnitUnderTest.Login(phone, username);

			//Assert
			Assert.That(result.Item2.Name, Is.EqualTo(username));
			Assert.That(result.Item2.PhoneNumber, Is.EqualTo(phone));
			Assert.That(result.Item2, Is.EqualTo(result2.Item2));
			Assert.That(result.Item1, Is.EqualTo(result2.Item1));

			//Cleanup
			DeleteUser(result.Item1);
		}

		[Test]
		public void TestGetUserReturnsExistingUser()
		{
			//Arrange
			const string username = "TestUsername";
			const string phone = "012 345 67 89";
			UnitUnderTest = new ZenChat();

			//Act
			var result = UnitUnderTest.Login(phone, username);
			var result2 = UnitUnderTest.GetUser(phone);

			//Assert
			Assert.That(result.Item2.Name, Is.EqualTo(username));
			Assert.That(result.Item2.PhoneNumber, Is.EqualTo(phone));
			Assert.That(result.Item2, Is.EqualTo(result2));
			Assert.That(result.Item1, Is.EqualTo(result2.Id));

			//Cleanup
			DeleteUser(result.Item1);
		}

		[Test]
		public void TestGetUserThrowsExceptionIfWrongUser()
		{
			//Arrange
			const string phone = "This Phone Number will never exist!";
			UnitUnderTest = new ZenChat();

			//Act & Assert
			Assert.Throws<UserNotFoundException>(() => UnitUnderTest.GetUser(phone));
		}

		[Test]
		public void TestGetUserFromIdReturnsExistingUser()
		{
			//Arrange
			const string username = "TestUsername";
			const string phone = "012 345 67 89";
			UnitUnderTest = new ZenChat();

			//Act
			var result = UnitUnderTest.Login(phone, username);
			var result2 = UnitUnderTest.GetUserFromId(result.Item1);

			//Assert
			Assert.That(result.Item2.Name, Is.EqualTo(username));
			Assert.That(result.Item2.PhoneNumber, Is.EqualTo(phone));
			Assert.That(result.Item2, Is.EqualTo(result2));
			Assert.That(result.Item1, Is.EqualTo(result2.Id));

			//Cleanup
			DeleteUser(result.Item1);
		}

		[Test]
		public void TestGetUserFromIdThrowsExceptionIfWrongUser()
		{
			//Arrange
			const int id = 1;
			UnitUnderTest = new ZenChat();

			//Act & Assert
			Assert.Throws<UserNotFoundException>(() => UnitUnderTest.GetUserFromId(id));
		}
		#endregion
	}
}