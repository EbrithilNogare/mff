using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Je_číslo_symetrické
{
    class Program
    {
        static void Main(string[] args)
        {
            string vstup;
            string vstup2;
            do
            {
                vstup = Console.ReadLine();
                char[] inputarray = vstup.ToCharArray();
                Array.Reverse(inputarray);
                vstup2 = new string(inputarray);
                if (vstup == vstup2 && vstup!="0")
                {
                    Console.Write(vstup + " ");
                }
            }
            while (vstup != "0");
            Console.ReadKey();



        }
    }
}
