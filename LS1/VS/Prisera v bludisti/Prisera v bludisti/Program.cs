using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prisera_v_bludisti
{
    class Program
    {
        static void Main(string[] args)
        {
            int w, h,x,y;
            w = h = x = y = 0;
            w = int.Parse(Console.ReadLine());
            h = int.Parse(Console.ReadLine());
            char d = '>';
            bool p = false;
            char[,] map = new char[h,w];
            for (int i = 0; i < h; i++)
            {
                string temp = Console.ReadLine();
                for (int j = 0; j < w; j++)
                {
                    map[i, j] = temp[j];
                    if(temp[j] == '>' || temp[j] == '<' || temp[j] == '^' || temp[j] == 'v')
                    {

                        map[i, j] = '.';
                        y = i;
                        x = j;
                        d = temp[j];
                    }                    
                }
            }

            for (int i = 0; i < 20; i++)
            {
                char dd = d;
                switch (dd)
                {
                    case '>':
                        if (map[y+1, x] == 'X' || p)
                        {
                            if (x + 1 >= 0 && x + 1 < map.GetLength(1) && map[y,x+1] == 'X')
                            {
                                d = '^';
                            }
                            else
                            {
                                x += 1;
                            }
                        }
                        else
                        {
                            d = 'v';
                        }
                        break;
                    case '<':
                        if (map[y-1,x] == 'X' || p)
                        {
                            if (x - 1 >= 0 && x - 1 < map.GetLength(1) && map[y,x-1] == 'X')
                            {
                                d = 'v';
                            }
                            else
                            {
                                x -= 1;
                            }
                        }
                        else
                        {
                            d = '^';
                        }
                        break;
                    case 'v':
                        if (map[y , x - 1] == 'X' || p)
                        {
                            if (y + 1 >= 0 && y + 1 < map.GetLength(0) && map[y + 1,x] == 'X')
                            {
                                d = '>';
                            }
                            else
                            {
                                y += 1;
                            }
                        }
                        else
                        {
                            d = '<';
                        }
                        break;
                    case '^':
                        if (map[y,x + 1] == 'X' || p)
                        {
                            if (y - 1 >= 0 && y - 1 < map.GetLength(0) && map[y - 1,x] == 'X')
                            {
                                d = '<';
                            }
                            else
                            {
                                y -= 1;
                            }
                        }
                        else
                        {
                            d = '>';
                        }
                        break;
                }
                p = false;
                if(d != dd)
                {
                    p = true;
                }




                vykresly(map,x,y,d);
            }

            
        }

        static void vykresly(char[,] map,int x, int y, char d)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (i == y && j == x)
                    {
                        Console.Write(d);
                    }
                    else
                    {
                        Console.Write(map[i, j]);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
