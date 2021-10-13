using System;
using System.IO;
using System.Data.SQLite;
using Xunit;

namespace SQLite_UTF16Econdig_Reproduce
{
    public class UnitTest1
    {
        private string connString;
        private string connStringUTF16;
        private void createDatabase(string databaseFile)
        {
            if (File.Exists(databaseFile)) File.Delete(databaseFile);
            SQLiteConnectionStringBuilder b = new SQLiteConnectionStringBuilder()
            {
                DataSource = databaseFile,
                UseUTF16Encoding = true
            };
            connStringUTF16 = b.ConnectionString;

            b.UseUTF16Encoding = false;
            connString = b.ConnectionString;

            using (SQLiteConnection connection = new SQLiteConnection(connString))
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

                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                        CREATE TABLE ""InvoiceDetail"" (
                            ""Id"" uniqueidentifier NOT NULL CONSTRAINT ""PK_InvoiceDetail"" PRIMARY KEY,
                            ""IdInvoice"" uniqueidentifier NOT NULL,
                            ""DetailData"" TEXT NULL
                        )";
                    cmd.ExecuteNonQuery();
                }
            }

        }


        [Fact]
        public void TestUTF16Disabled()
        {
            createDatabase("data.db");
            execTest(connString);
        }

        [Fact]
        public void TestUTF16Enabled()
        {
            createDatabase("data16.db");
            execTest(connStringUTF16);
        }


        private void execTest(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT * 
                        FROM Invoice 
                        INNER JOIN InvoiceDetail 
                            ON Invoice.Id = InvoiceDetail.IdInvoice";

                    SQLiteDataReader reader = cmd.ExecuteReader();
                    Assert.Equal("Id", reader.GetName(0));
                    Assert.Equal("CostumerName", reader.GetName(1));

                }
            }
        }
      
    }
}
