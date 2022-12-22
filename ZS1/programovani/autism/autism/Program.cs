using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autism
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] vstupS = Console.ReadLine().Split(' ');
            int[] vstup = new int[vstupS.Length];
            for (int i = 0; i < vstupS.Length; i++)
            {
                vstup[i] = int.Parse(vstupS[i]);
            }
            Array.Sort(vstup);


            odeber(vstup, 0, 0);







            for (int i = 0; i < vstup.Length; i++)
            {
                Console.Write(vstup[i]+" ");
            }
            Console.ReadKey();
        }



        static void odeber(int[] vstup,int kolik,int kde)
        {
            if (vstup[kde] >= kolik)
            {
                vstup[kde] -= kolik;
                for(int i = 1; i < vstup.Length; i++)
                {
                    for(int j = 1; j < vstup[i]; j++)
                    {
                        odeber(vstup, j, i);
                    }
                }

            }
            else
            {
                for (int i = 0; i < vstup.Length; i++)
                {
                    Console.Write(vstup[i] + " ");
                }
                Console.WriteLine();
            }
        }

    }
}
