// Copyright (c) 2016 Pascal Honegger
// All rights reserved.

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using ZenChatService.Exceptions;
using ZenChatService.Properties;

namespace ZenChatService.ServiceClasses
{
	/// <summary>
	///     Die Klasse eines Users. Beinhaltet alle notwendigen Informationen über einen User.
	/// </summary>
	[DataContract]
	public class User
	{
		/// <summary>
		///     Setzt die <see cref="Id" /> des Users und ruft <see cref="ToFullUser" /> auf.
		/// </summary>
		/// <param name="id">
		///     <see cref="Id" />
		/// </param>
		public User(int id)
		{
			Id = id;
			ToFullUser();
		}

		/// <summary>
		///     ID des Spielers. Kommt aus der Datenbank.
		/// </summary>
		public int Id { get; }

		/// <summary>
		///     Anzeigename des Spielers. Ist nicht einmalig und wird als Nickname benutzt.
		/// </summary>
		[DataMember]
		public string Name { get; private set; }

		/// <summary>
		///     Telefonnummer des Users. Ist einmalig und wird beim Login verwendet.
		/// </summary>
		[DataMember]
		public string PhoneNumber { get; private set; }

		/// <summary>
		///     Alle Freunde des Users
		/// </summary>
		public IEnumerable<User> Friends
		{
			get
			{
				var friendIds = new List<int>();

				using (var connection = new SqlConnection(Settings.Default.ConnectionString))
				{
					connection.Open();

					var command = new SqlCommand("SELECT fk_user1 FROM [friendship] WHERE fk_user2 = @id", connection);

					command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));

					command.Parameters["@id"].Value = Id;

					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						friendIds.Add(reader.GetInt32(0));
					}

					reader.Close();

					command.CommandText = "SELECT fk_user2 FROM [friendship] WHERE fk_user1 = @id";

					reader = command.ExecuteReader();

					while (reader.Read())
					{
						friendIds.Add(reader.GetInt32(0));
					}
				}

				return friendIds.Select(id => new User(id));
			}
		}

		/// <summary>
		///     Lädt alle Daten des Users nach.
		/// </summary>
		/// <exception cref="UserNotFoundException">Es wurde kein User mit der angegebenen ID gefunden.</exception>
		private void ToFullUser()
		{
			using (var connection = new SqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();

				var command = new SqlCommand("SELECT name, phone FROM [user] WHERE id_user = @id", connection);

				command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));

				command.Parameters["@id"].Value = Id;

				var reader = command.ExecuteReader();

				if (reader.Read())
				{
					Name = reader.GetString(0);
					PhoneNumber = reader.GetString(1);
				}
				else
				{
					var e = new UserNotFoundException();
					throw new FaultException<UserNotFoundException>(e, e.Message);
				}
			}
		}

		private bool Equals(User other)
		{
			return Equals(Id, other.Id);
		}

		/// <summary>Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.</summary>
		/// <returns>true, wenn das angegebene Objekt und das aktuelle Objekt gleich sind, andernfalls false.</returns>
		/// <param name="obj">Das Objekt, das mit dem aktuellen Objekt verglichen werden soll. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((User) obj);
		}

		/// <summary>Fungiert als die Standardhashfunktion. </summary>
		/// <returns>Ein Hashcode für das aktuelle Objekt.</returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode() => Id.GetHashCode();
	}
}