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
            int a = int.Parse(Console.ReadLine());
            string[] b = Console.ReadLine().Split(' ');

            //int a = 100;
            //string[] b = "0 1 2 5 3 3 0".Split(' ');

            int[] p = new int[b.Length];
            int[] ps = new int[b.Length];

            for (int i = 0; i < b.Length; i++)
            {
                p[i] = int.Parse(b[i]);
                ps[i] = int.Parse(b[i]);
            }
            Array.Sort<int>(ps,delegate (int aa, int bb) { return bb - aa; });
            if (ps.SequenceEqual(p))
                Console.WriteLine("NEEXISTUJE");
            else
            {
                ps = p;
                int pivot = 0;
                for (int n = p.Length-1; n > 0; n--)
                {
                    if(p[n] > p[n - 1])
                    {
                        pivot = n - 1;
                        break;
                    }
                }


                for (int n = p.Length - 1; n > 0; n--)  
                {
                    if (p[n] > p[pivot])
                    {
                        int temp = p[n];
                        p[n] = p[pivot];
                        p[pivot] = temp;
                        break;
                    }
                }


                for (int n = p.Length - 1; n > pivot; n--)
                {
                    int temp = p[n];
                    p[n] = p[pivot+1];
                    p[pivot+1] = temp;
                    pivot++;
                }
                


                foreach (int pp in p)
                    Console.Write(pp + " ");



                Console.WriteLine();

            }
            Console.ReadKey();
        }
    }
}
