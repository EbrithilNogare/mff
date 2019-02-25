using System;

namespace Hanoiske_veze
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            int zacatek = 1, konec = 2, temp = 3;

            Tah(n, zacatek, konec, temp);

            Console.ReadKey();
        }
        static void Tah(int n, int zacatek, int konec, int temp)
        {
            if (n > 0)
            {
                Tah(n - 1, zacatek, temp, konec);
                Console.WriteLine("Kotouc " + n + " z " + zacatek + " na " + konec);
                Tah(n - 1, temp, konec, zacatek);
            }
        }
    }
}
