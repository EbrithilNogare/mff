using System;
using System.IO;

namespace worldCounting
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 1;
            char[] splitters = new char[] { ' ', '\t', '\n'};
            bool first = false;
            if (args.Length != 1) {
                Console.WriteLine("Argument Error");
                return;
            }
            try
            {
                using (var sr = new StreamReader(args[0])){
                    while (sr.Peek() >= 0)
                    {
                        char c = (char)sr.Read();
                        if (c == ' ' || c == '\t' || c == '\n')
                        {
                            if (first)
                                count++;
                            first = false;
                        }
                        else
                        {
                            first = true;
                        }
                    }
                    if (!first)
                        count--;
                }



                /*
                using (var sr = new StreamReader(args[0]))
                {
                    string text = "";
                    while ((text = sr.ReadLine()) != null)
                    {
                        string[] line = text.Split(splitters);
                        for (int i = 0; i < line.Length; i++)
                        {
                            if (line[i] != "")
                                count++;
                        }
                    }
                }
                */
                Console.WriteLine(count);
            }
            catch {
                Console.WriteLine("File Error");
            }
        }
    }
}
