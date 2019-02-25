using System;
using System.IO;

namespace codex_1._2
{
    class Program
    {
        static void Main(string[] args)
        {
            int a, b;
            string s = Console.ReadLine();
            string[] ss = s.Split(' ');
            a = int.Parse(ss[0]);
            b = int.Parse(ss[1]);



            int c;
            if (b != 0)
            {
                c = (a / b);
                Console.WriteLine(c);
            }
            else
                Console.WriteLine("NELZE");
            Console.ReadKey();
        }
    }
}