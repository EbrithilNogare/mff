using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rekurze
{
    class Program
    {
        static void Main(string[] args)
        {
            metoda(3, "");
            Console.ReadKey();
        }

        static void metoda(int n, string a)
        {
            if (n > 0)
            {
                metoda(n - 1, a + "0");
                metoda(n - 1, a + "1");
            }
            
                Console.WriteLine(a);
            

        }


    }
}
