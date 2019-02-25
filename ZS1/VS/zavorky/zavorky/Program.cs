using System;

namespace zavorky
{
    class Program
    {
        static void Main()
        {
            int pocet = int.Parse(Console.ReadLine());
            char[] text = new char[pocet * 2];
            vytiskniZavorku(text, pocet);



            Console.WriteLine("hotovo");
            Console.ReadKey(); 
        }
            static void vytiskniZavorky(char[] text, int misto, int pocet, int oteviraci, int konecna)
            {
                if (konecna == pocet)
                {
                    using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"WriteLines.txt"))
                {
                    file.Write("Console.WriteLine(\"");
                    for (int i = 0; i < text.Length; i++)
                            file.Write(text[i]);
                    file.Write("\");");
                    file.WriteLine();
                }

                    return;
                }
                else
                {
                    if (oteviraci < pocet)
                    {
                        text[misto] = '(';
                        vytiskniZavorky(text, misto + 1, pocet, oteviraci + 1, konecna);
                    }
                    if (oteviraci > konecna)
                    {
                        text[misto] = ')';
                        vytiskniZavorky(text, misto + 1, pocet, oteviraci, konecna + 1);
                    }
                }
            }
        
            static void vytiskniZavorku(char[] text, int pocet)
            {
                if (pocet > 0)
                    vytiskniZavorky(text, 0, pocet, 0, 0);
                return;
            }
        
    }
}
