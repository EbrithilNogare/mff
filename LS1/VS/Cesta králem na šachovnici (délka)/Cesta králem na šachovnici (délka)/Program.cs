using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesta_králem_na_šachovnici__délka_
{
    class Program
    {
        static void Main(string[] args)
        {
            int kunX, kunY, vstupX, vstupY, vystupX, vystupY, pocetPrekazek;
            string tmp;
            int[,] sachovnice = new int[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    sachovnice[i, j] = 99;
                }
            }
            pocetPrekazek =  int.Parse(Console.ReadLine());
            for (int i = 0; i < pocetPrekazek; i++)
            {
                tmp = Console.ReadLine();
                sachovnice[int.Parse(tmp.Split(' ')[0])-1, int.Parse(tmp.Split(' ')[1])-1] = -1;
            }
            tmp = Console.ReadLine();
            kunX = int.Parse(tmp.Split(' ')[0])-1;
            kunY = int.Parse(tmp.Split(' ')[1]) - 1;

            vstupX = kunX;
            vstupY = kunY;

            tmp = Console.ReadLine();
            vystupX = int.Parse(tmp.Split(' ')[0]) - 1;
            vystupY = int.Parse(tmp.Split(' ')[1]) - 1;

            sachovnice[kunX, kunY] = 0;

            cesta(kunX, kunY, sachovnice);

            if(sachovnice[vystupX, vystupY] == 99)
                Console.WriteLine(-1);
            else
                Console.WriteLine(sachovnice[vystupX, vystupY]);
            Console.ReadKey();

        }

        static void cesta(int kunX, int kunY, int[,] sachovnice)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (((i != 0) || (j != 0)) && (kunX + i < 8) && (kunX + i >= 0) && (kunY + j < 8) && (kunY + j >= 0) && (sachovnice[kunX + i, kunY + j] > sachovnice[kunX, kunY]+1))
                    {
                        sachovnice[kunX + i, kunY + j] = sachovnice[kunX, kunY] + 1;
                        cesta(kunX + i,kunY+j,sachovnice);
                    }
                }
            }
        
    }
    }
}
