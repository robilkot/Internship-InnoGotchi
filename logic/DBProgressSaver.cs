using Microsoft.Data.SqlClient;

namespace InnoGotchi.logic
{
    public class DBProgressSaver : IProgressSaver
    {
        public string ConnectionString =
            $"Server=localhost;" +
            $"Database=InnoGotchi;" +
            $"Trusted_Connection=True;" +
            $"TrustServerCertificate=True";

        private static DBProgressSaver? s_instance = null;
        private DBProgressSaver()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string commandText =
                    "IF OBJECT_ID(N'dbo.pets', N'U') IS NULL " +
                    "CREATE TABLE dbo.pets (" +
                    "Id uniqueidentifier PRIMARY KEY NOT NULL, " +
                    "Name varchar(50) NOT NULL, " +
                    "Mouth int NOT NULL, " +
                    "Nose int NOT NULL, " +
                    "Eyes int NOT NULL, " +
                    "Body int NOT NULL, " +
                    "Created datetime NOT NULL, " +
                    "Updated datetime NOT NULL, " +
                    "LastDrinkTime datetime NOT NULL, " +
                    "LastEatTime datetime NOT NULL, " +
                    "Hunger int NOT NULL, " +
                    "Thirst int NOT NULL, " +
                    "HappinessDaysCount int NOT NULL, " +
                    "Dead bit NOT NULL" +
                    ");";

                using var command = new SqlCommand(commandText, connection);
                command.ExecuteNonQuery();
            }
        }

        public static DBProgressSaver GetInstance()
        {
            if (s_instance == null)
                s_instance = new DBProgressSaver();
            return s_instance;
        }

        public async Task Write(List<Pet> pets)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Should be update but suitable in this case
                using (var command = new SqlCommand("TRUNCATE TABLE pets", connection))
                {
                    command.ExecuteNonQuery();
                }

                foreach (var pet in pets)
                {
                    string commandText =
                        //$"IF EXISTS (SELECT 1 FROM pets WHERE Id = '{pet.Id}') " +
                        //"UPDATE pets ";
                        "INSERT INTO pets " +
                        "(Id, Name, Mouth, Nose, Eyes, Body, " +
                        "Created, Updated, LastEatTime, LastDrinkTime, Thirst, Hunger, HappinessDaysCount, Dead) " +
                        "VALUES " +
                        "(@guid, @name, @mouth, @nose, @eyes, @body, " +
                        "@created, @updated, @lastEatTime, @lastDrinkTime, @thirst, @hunger, @happinessDaysCount, @dead)";

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = commandText;

                        command.Parameters.AddWithValue("@guid", pet.Id);
                        command.Parameters.AddWithValue("@name", pet.Name);
                        command.Parameters.AddWithValue("@mouth", pet.Mouth);
                        command.Parameters.AddWithValue("@nose", pet.Nose);
                        command.Parameters.AddWithValue("@eyes", pet.Eyes);
                        command.Parameters.AddWithValue("@body", pet.Body);

                        command.Parameters.AddWithValue("@created", pet.Created);
                        command.Parameters.AddWithValue("@updated", pet.Updated);
                        command.Parameters.AddWithValue("@lastEatTime", pet.LastEatTime);
                        command.Parameters.AddWithValue("@lastDrinkTime", pet.LastDrinkTime);

                        command.Parameters.AddWithValue("@thirst", pet.Thirst);
                        command.Parameters.AddWithValue("@hunger", pet.Hunger);
                        command.Parameters.AddWithValue("@happinessDaysCount", pet.HappinessDaysCount);
                        command.Parameters.AddWithValue("@dead", pet.Dead);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<Pet> Read()
        {
            List<Pet> pets = new();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Get tables names
                //foreach (DataRow row in connection.GetSchema("Tables").Rows)
                //{
                //    Console.WriteLine(row[2]);
                //}

                string commandText = "SELECT * from Pets";

                using var command = new SqlCommand(commandText, connection);
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Pet readPet = new()
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"],
                        Body = (Body)reader["Body"],
                        Mouth = (Mouth)reader["Mouth"],
                        Nose = (Nose)reader["Nose"],
                        Eyes = (Eyes)reader["Eyes"],

                        Created = (DateTime)reader["Created"],
                        Updated = (DateTime)reader["Updated"],
                        LastEatTime = (DateTime)reader["LastEatTime"],
                        LastDrinkTime = (DateTime)reader["LastDrinkTime"],

                        Thirst = (Thirst)reader["Thirst"],
                        Hunger = (Hunger)reader["Hunger"],

                        HappinessDaysCount = (int)reader["HappinessDaysCount"],
                        Dead = (bool)reader["Dead"]
                    };

                    pets.Add(readPet);
                }
            }

            return pets;
        }
    }
}