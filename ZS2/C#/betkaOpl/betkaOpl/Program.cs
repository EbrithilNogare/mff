using System;
using System.IO;
using System.Globalization;

namespace betkaOpl
{
    class Program
    {
        static void Main(string[] args)
        {
            string znamenko = "";
            while(znamenko!="+" && znamenko != "-")
            {
                Console.WriteLine("odečíst, nebo pricist?\nzvolte znamenko");
                Console.WriteLine("+ -");
                znamenko = Console.ReadLine();
            }
            if(args.Length != 1)
            {
                Console.WriteLine("neplatné argumenty");
                return;
            }
            try
            {
                using (var sr = new StreamReader(args[0]))
                {
                    using (var sw = new StreamWriter("file.out"))
                    {
                        CultureInfo myCIintl = new CultureInfo("en-EN", false);
                        sw.WriteLine(sr.ReadLine());
                        string[] radek = new string[2];
                        double vysledek = 0;
                        while (!sr.EndOfStream)
                        {
                            char[] splitters = new char[] { ' ', '\t' };
                            radek = sr.ReadLine().Split(splitters);
                            vysledek = double.Parse(radek[0], myCIintl);
                            double cislo1 = 0;
                            cislo1 = double.Parse(radek[1], myCIintl);
                            if (znamenko == "+")
                            {                                
                                vysledek += cislo1;
                            }
                            else
                            {
                                vysledek -= cislo1;

                            }
                            vysledek = Math.Round(vysledek * 10000) / 10000;
                            sw.WriteLine(vysledek.ToString("0.0000"));
                        }
                        Console.WriteLine("uspesne hotovo");
                    }
                }
            }
            catch(IOException exc)
            {
                Console.WriteLine("file Error");
                Console.WriteLine(exc.Message);
            }
            catch (FormatException exc)
            {
                Console.WriteLine("chyba převodu");
                Console.WriteLine(exc.Message);
            }
            Console.WriteLine("ukoncite jakoukoliv klavesou");
            Console.ReadKey();
        }
    }
}
