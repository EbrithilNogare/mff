using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace postfix
{
    class Program
    {
        static void Main(string[] args)
        {
            //string vstup = "5a3+ 4 * + 5"; //21
            string vstup = "";
            int vstupTemp = Console.Read();
            while (vstupTemp != -1)
            {
                vstup += Convert.ToChar(vstupTemp);
                vstupTemp = Console.Read();
            }
            Console.WriteLine(solvit(vstup));
            Console.ReadKey();
        }
        static string solvit(string vstup)
        {
            List<int> cisla = new List<int>();

            while (vstup.Length > 0)
            {
                if (vstup[0] == '+' || vstup[0] == '-' || vstup[0] == '*' || vstup[0] == '/')
                {
                    if (cisla.Count < 2) return "Chyba!";
                    switch (vstup[0])
                    {
                        case '+':
                            cisla[cisla.Count - 2] = cisla[cisla.Count - 2] + cisla[cisla.Count - 1];
                            break;
                        case '-':
                            cisla[cisla.Count - 2] = cisla[cisla.Count - 2] - cisla[cisla.Count - 1];
                            break;
                        case '*':
                            cisla[cisla.Count - 2] = cisla[cisla.Count - 2] * cisla[cisla.Count - 1];
                            break;
                        case '/':
                            cisla[cisla.Count - 2] = cisla[cisla.Count - 2] / cisla[cisla.Count - 1];
                            break;
                    }
                    cisla.RemoveAt(cisla.Count - 1);

                    vstup = vstup.Remove(0, 1);
                }
                else if ("1234567890".Contains(vstup[0]))
                {
                    string temp = "";
                    while (vstup.Length > 0 && "1234567890".Contains(vstup[0]))
                    {
                        temp += int.Parse(vstup[0].ToString());
                        vstup = vstup.Remove(0, 1);
                    }
                    cisla.Add(int.Parse(temp));
                }
                else
                {
                    vstup = vstup.Remove(0, 1);
                }
            }

            return cisla[0].ToString();

        }
    }
}
