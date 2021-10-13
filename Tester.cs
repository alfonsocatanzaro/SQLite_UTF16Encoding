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
    private readonly bool _useUTF16Encoding;
    public Tester(string databaseFile, bool useUTF16Encoding)
    {
      _databaseFile = databaseFile;
      _useUTF16Encoding = useUTF16Encoding;
    }

    public void DoTest()
    {
      Console.WriteLine("--------------------------------------------------------------");
      Console.WriteLine($"UseUTF16Encoding enabled:{_useUTF16Encoding}");
      _deleteDatabaseFileIfExists();
      _createDatabase(getConnectionString(_useUTF16Encoding));
      _execQuery(getConnectionString(_useUTF16Encoding));
      Console.WriteLine("--------------------------------------------------------------");
      Console.WriteLine("");
    }

    private void _deleteDatabaseFileIfExists()
    {
      if (File.Exists(_databaseFile))
        File.Delete(_databaseFile);
    }

    private string getConnectionString(bool useUTF16Encoding)
    {
      SQLiteConnectionStringBuilder connectionStringBuilder = new SQLiteConnectionStringBuilder();
      connectionStringBuilder.DataSource = _databaseFile;
      if (useUTF16Encoding)
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
                            ""CostumerTestAaaa"" TEXT NULL,
                            ""CostumerTestBbbb"" TEXT NULL,
                            ""CostumerTestCccc"" TEXT NULL,
                            ""CostumerTestDddd"" TEXT NULL,
                            ""CostumerTestEeee"" TEXT NULL,
                            ""CostumerTestFfff"" TEXT NULL,
                            ""CostumerTestGggg"" TEXT NULL,
                            ""CostumerTestHhhh"" TEXT NULL,
                            ""CostumerTestIiii"" TEXT NULL,
                            ""CostumerTestLlll"" TEXT NULL
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
      bool failed = false;

      failed |= check(reader, 0, "Id");
      failed |= check(reader, 1, "CostumerTestAaaa");
      failed |= check(reader, 2, "CostumerTestBbbb");
      failed |= check(reader, 3, "CostumerTestCccc");
      failed |= check(reader, 4, "CostumerTestDddd");
      failed |= check(reader, 5, "CostumerTestEeee");
      failed |= check(reader, 6, "CostumerTestFfff");
      failed |= check(reader, 7, "CostumerTestGggg");
      failed |= check(reader, 8, "CostumerTestHhhh");
      failed |= check(reader, 9, "CostumerTestIiii");
      failed |= check(reader, 10, "CostumerTestLlll");

      if (failed)
        Console.WriteLine("TEST FAILED!!!!!");
      else
        Console.WriteLine("SUCCESS");
    }

    private bool check(SQLiteDataReader reader, int fieldId, string expected)
    {
      string actual = reader.GetName(fieldId);
      bool isEqual = string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase);
      Console.WriteLine($"  reader.GetName({fieldId}) expected:'{expected}' actual:'{actual}' isEqual={isEqual}");
      return !isEqual;
    }
  }
}
