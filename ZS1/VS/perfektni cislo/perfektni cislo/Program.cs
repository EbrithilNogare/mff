using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace perfektni_cislo
{
    class Program
    {
        static void Main(string[] args)
        {
            long c = long.Parse(Console.ReadLine());
            string vysledek = "";

            switch (c)
            {
                case 6:
                case 28:
                case 496:
                case 8128:
                case 33550336:
                case 8589869056:
                case 137438691328:
                case 2305843008139952128:
                    vysledek += "P";
                    break;
            }
            if (Math.Sqrt(c) % 1.0 == 0)
                vysledek += "C";

            if (Math.Pow(c, 1.0 / 3) % 1 < 0.00001 || Math.Pow(c, 1.0 / 3) % 1 > 0.999999)
                vysledek += "K";

            Console.WriteLine(vysledek);
            Console.ReadKey();
        }
    }
}
