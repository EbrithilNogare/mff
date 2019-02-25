using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            int a = 0;
            for (int i = 0; i < 100000000; i++)
            {
                a++;
            }
            DateTime dt = DateTime.Now;
            for (int i = 0; i < 999; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Console.Write(i);
                }
                Console.WriteLine();
            }
            TimeSpan ts = DateTime.Now - dt;
            dt = DateTime.Now;
            string s = "";
            for (int i = 0; i < 999; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    s += i;
                }
                s += "\n";
            }
            Console.Write(s);
            TimeSpan tts = DateTime.Now - dt;
            Console.WriteLine();
            Console.WriteLine(ts.TotalMilliseconds.ToString());
            Console.WriteLine(tts.TotalMilliseconds.ToString());
            Console.ReadKey();
        }
    }
}
