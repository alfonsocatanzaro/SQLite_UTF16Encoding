using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            new Tester("test1.db", false, false).DoTest();
            new Tester("test2.db", true, true).DoTest();
            new Tester("test3.db", false, true).DoTest();
            new Tester("test4.db", true, false).DoTest();
        }
    }
}
