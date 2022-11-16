using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radixSort
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> vstup = new List<int>();
            string vstupTemp = Console.ReadLine();
            while(vstupTemp != "")
            {
                int a;
                if (int.TryParse(vstupTemp, out a))
                {
                    vstup.Add(a);
                    vstupTemp = Console.ReadLine();
                }
                else
                    break;
            }

            int[] pole = vstup.ToArray();
            radixSort(pole);
            Console.ReadKey();


        }
        static void radixSort(int[] pole)
        {
            List<int>[] fronty = new List<int>[10];
            for (int i = 0; i < fronty.Length; i++)
            {
                fronty[i] = new List<int>();
            }
            for (int mocnina = 1;mocnina <= 1000000; mocnina *= 10)
            {
                for (int i = 0; i < pole.Length; i++)
                {
                    int findex = (pole[i] % (10 * mocnina)) / mocnina;
                    fronty[findex].Add(pole[i]);
                }

                int index = 0;
                for (int i = 0; i < fronty.Length; i++)
                {
                    for (int j = 0; j < fronty[i].Count; j++)
                    {
                        pole[index] = fronty[i][j];
                        index++;
                    }
                }
                for (int i = 0; i < fronty.Length; i++)
                {
                    fronty[i].Clear();
                }

                for (int i = 0; i < pole.Length; i++)
                {
                     if (i!=pole.Length)
                    {
                        Console.Write(pole[i] + " ");
                    }
                    else
                    {
                        Console.Write(pole[i]);
                    }
                }
                Console.WriteLine();
            }            
        }
    }
}
