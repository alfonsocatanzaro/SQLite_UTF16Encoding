using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace ConsoleApp1
{
    public class Tester
    {
        private readonly string _databaseFile;
        private readonly bool _createInUTF16;
        private readonly bool _useInUTF16;
        public Tester(string databaseFile, bool createInUTF16, bool useInUTF16)
        {
            _databaseFile = databaseFile;
            _createInUTF16 = createInUTF16;
            _useInUTF16 = useInUTF16;
        }

        public void DoTest()
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"createInUTF16: {_createInUTF16} - useInUTF16:{_useInUTF16}");
            _deleteDatabaseFileIfExists();
            _createDatabase(getConnectionString(_createInUTF16));
            _execQuery(getConnectionString(_useInUTF16));
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("");
        }

        private void _deleteDatabaseFileIfExists()
        {
            if (File.Exists(_databaseFile))
                File.Delete(_databaseFile);
        }

        private string getConnectionString(bool useUTF16)
        {
            SQLiteConnectionStringBuilder connectionStringBuilder = new SQLiteConnectionStringBuilder();
            connectionStringBuilder.DataSource = _databaseFile;
            if (useUTF16)
                connectionStringBuilder.UseUTF16Encoding = true;
            return connectionStringBuilder.ConnectionString;
        }

        private void _createDatabase(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                        CREATE TABLE ""Invoice"" (
                            ""Id"" uniqueidentifier NOT NULL CONSTRAINT ""PK_Invoice"" PRIMARY KEY,
                            ""CostumerName"" TEXT NULL
                        )";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void _execQuery(string connectionString)
        {
            using SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            using SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Invoice";
            SQLiteDataReader reader = cmd.ExecuteReader();
            Console.WriteLine($"  reader.GetName(0) expected:'Id' actual:'{reader.GetName(0)}'");
            Console.WriteLine($"  reader.GetName(1) expected:'CostumerName' actual:'{reader.GetName(1)}'");
            if (string.Equals("Id", reader.GetName(0), StringComparison.OrdinalIgnoreCase) &&
                string.Equals("CostumerName", reader.GetName(1), StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("SUCCESS");
            }
            else
            {
                Console.WriteLine("TEST FAILED!!!!!");
            }
        }
    }
}
