using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace cetnostSlov
{
    class Program
    {
        static void Main(string[] args)
        {
            var wordCount = new Dictionary<string, int>();
            string word = "";
            char[] separators = new char[] { ' ', '\t', '\n' };
            if (args.Length != 1)
            {
                Console.WriteLine("Argument Error");
                return;
            }
            try
            {
                using (var sr = new StreamReader(args[0]))
                {
                    while (sr.Peek() >= 0)
                    {
                        char c = (char)sr.Read();
                        if (c == ' ' || c == '\t' || c == '\n' || c == '\r')
                        {
                            if (word != "")
                            {
                                if (wordCount.ContainsKey(word))
                                {
                                    wordCount[word] = wordCount[word] + 1;
                                }
                                else
                                {
                                    wordCount.Add(word, 1);
                                }
                            }
                            word = "";
                        }
                        else
                        {                            
                            word += c;
                        }

                    }
                    if (word != "")
                    {
                        if (wordCount.ContainsKey(word))
                        {
                            wordCount[word] = wordCount[word] + 1;
                        }
                        else
                        {
                            wordCount.Add(word, 1);
                        }
                    }
                }
                
            }
            catch
            {
                Console.WriteLine("File Error");
                return;
            }

           

            var list = wordCount.Keys.ToList();
            list.Sort();

            /*
            using (var sw = new StreamWriter("counts.txt"))
            {
                foreach (var key in list)
                {
                    //Console.WriteLine("{0}: {1}", key, wordCount[key]);
                    sw.WriteLine("{0}: {1}", key, wordCount[key]);
                }
            }*/
            foreach (var key in list)
            {
                Console.WriteLine($"{key}: {wordCount[key]}");
            }

        }
    }
}
