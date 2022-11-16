using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace druhe_nejvetsi_cislo
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] v = Console.ReadLine().Split(' ');
            int t = Array.IndexOf(v,"-1");
            int[] c = new int[t];

            for(int i = 0; i < t;i++)
            {
                c[i] = int.Parse(v[i]);
            }
            Array.Sort(c);
            Console.WriteLine(c[t-2]);
            Console.ReadKey();
        }
    }
}
