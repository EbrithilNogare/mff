using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maximum_v_posloupnosti__výskyty_
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine()); 
            string s = Console.ReadLine();
            string[] sa = s.Split(' ');
            int[] si = new int[n];
            int vysledek1;
            string vysledek2 = "";

            for(int i = 0; i < n; i++)
            {
                si[i] = int.Parse(sa[i]);
            }

            vysledek1 = si.Max();

            int index = 0;
            do
            {
                index = Array.IndexOf(si, vysledek1, index+1);
                if (index != -1)
                    vysledek2 += index + " ";
            }
            while(index >= 0);

            Console.WriteLine(vysledek1);
            Console.WriteLine(vysledek2);
            Console.ReadKey();


        }
    }
}
