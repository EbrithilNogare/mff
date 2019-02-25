using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uzavorkovani_dynamicly
{
    class Program
    {
        static long[,] ca;
        static void Main()
        {
            int i = Convert.ToInt32(Console.ReadLine()), a, b;
            ca = new long[i + 1, i + 1];
            for (a = i; a >= 0; a--)
                for (b = i; b >= a; b--)
                {
                    if ((i == b) && (a == b))
                        ca[b, a] = 1;
                    if (b > a) ca[b, a] = ca[b, a + 1];
                    if (b < i) ca[b, a] += ca[b + 1, a];
                }
            for (int k = 0; k < i+1; k++)
            {
                for (int j = 0; j < i+1; j++)
                {
                    Console.Write(ca[k,j].ToString("D3") + " ");
                }
                Console.WriteLine();
            }
            Console.ReadKey();

        }
    }
}
