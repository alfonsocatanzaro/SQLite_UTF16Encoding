using System;

namespace ConsoleApp1
{
  class Program
  {
    static void Main(string[] args)
    {
      var noUTF16Test = new Tester("test1.db", false);
      noUTF16Test.DoTest();

      var UTF16Test = new Tester("test2.db", true);
      UTF16Test.DoTest();

    }
  }
}
