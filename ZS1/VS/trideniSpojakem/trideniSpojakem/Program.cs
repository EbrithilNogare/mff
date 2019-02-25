using System;
using System.Collections.Generic;

namespace trideniSpojakem
{
    class Program
    {
        static void Main()
        {
            int c = 0;
            long[] cisla = new long[100000];
            for(int i = 0; i < cisla.Length; i++)
            {
                cisla[i] = long.MinValue;
            }
            string a; a = Console.ReadLine();
            while ( a != "")
            {
                foreach (string b in a.Split(' '))
                {
                    if (b!="") {
                        cisla[c] = (long.Parse(b));
                        c++;
                    }
                }
                a = Console.ReadLine();
            }
            Array.Sort(cisla);
            foreach(long cislo in cisla)
            {
                if(cislo != long.MinValue)
                Console.Write(cislo + " ");
            }
            Console.ReadKey();
        }
    }
}
