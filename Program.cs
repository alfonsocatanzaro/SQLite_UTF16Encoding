using System;

namespace ConsoleApp1
{
  class Program
  {
    static void Main(string[] args)
    {

      //Without UTF16Encodong
      new Tester("test1.db", false).DoTest();


      //With UTF16Encodong
      new Tester("test2.db", true).DoTest();

    }
  }
}
