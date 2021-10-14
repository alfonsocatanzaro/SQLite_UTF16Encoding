using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
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

    private string line = new string('-', 60);

    public void DoTest()
    {
      Console.WriteLine(line);
      Console.WriteLine($"UseUTF16Encoding enabled:{_useUTF16Encoding}");
      _deleteDatabaseFileIfExists();
      _createDatabase(getConnectionString(_useUTF16Encoding));
      _execQuery(getConnectionString(_useUTF16Encoding));
      Console.WriteLine(line);
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


    private string[] fields = {
      "Bathtub",
      "Poison",
      "Weak",
      "Brag",
      "Condition",
      "Graduate",
      "Ankle",
      "Category",
      "Displace",
      "Means",
      "Half",
      "Stumble",
      "Shark",
      "Forest",
      "Commission",
      "Front",
      "Spend",
      "Eliminate",
      "Assume",
      "Extraterrestrial",
      "Hallway",
      "Curve",
      "Belt",
      "Policeman",
      "Cut",
      "Bucket",
      "Fee",
      "Genetic",
      "Bill",
      "Think",
      "Exhibition",
      "Glacier",
      "Taste",
      "Frank",
      "Maid",
      "Reconcile",
      "Delete",
      "Treaty",
      "Struggle",
      "Profit"

    };


    private void _createDatabase(string connectionString)
    {
      using SQLiteConnection connection = new SQLiteConnection(connectionString);
      connection.Open();
      using SQLiteCommand cmd = connection.CreateCommand();

      string fieldDef = string.Join(" ", fields.Select(a => $",\"{a}\" TEXT NULL"));
      cmd.CommandText = $"CREATE TABLE \"Invoice\" (\"Id\" uniqueidentifier NOT NULL {fieldDef})";
      cmd.ExecuteNonQuery();
    }

    private void _execQuery(string connectionString)
    {
      using SQLiteConnection connection = new SQLiteConnection(connectionString);
      connection.Open();
      using SQLiteCommand cmd = connection.CreateCommand();
      cmd.CommandText = "SELECT * FROM Invoice";
      SQLiteDataReader reader = cmd.ExecuteReader();

      Console.WriteLine($"{"ID",-3} {"Expected",-20} {"Actual",-20} {"IsEqual"}");
      Console.WriteLine(line);

      bool failed = false;
      failed |= check(reader, 0, "Id");
      for (int i = 0; i < fields.Length; i++)
        failed |= check(reader, i + 1, fields[i]);

      if (failed)
        Console.WriteLine("TEST FAILED!!!!!");
      else
        Console.WriteLine("SUCCESS");
    }

    private bool check(SQLiteDataReader reader, int fieldId, string expected)
    {
      string actual = reader.GetName(fieldId); // GetName() method on linux sometime returns not well terminated strings
      bool isEqual = string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase);
      Console.WriteLine($"{fieldId,-3:0#} {expected,-20} {actual,-20} {isEqual}");
      return !isEqual;
    }
  }
}
