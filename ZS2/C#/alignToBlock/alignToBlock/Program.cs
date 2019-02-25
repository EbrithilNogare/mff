using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace alignToBlock
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!argumentsCheck(args))
            {
                Console.WriteLine("Argument Error");
                return;
            }

            char[] separators = new char[] { ' ', '\t', '\n', '\r' };

            blockAlign(args[0], args[1], int.Parse(args[2]), separators);



        }

        static bool argumentsCheck(string[] args)
        {
            if (args.Length != 3)
            {
                return false;
            }
            if (!(int.TryParse(args[2], out int bin)) || !(int.Parse(args[2]) > 0))
            {
                return false;
            }
            return true;
        }

        static void blockAlign(string fileIn, string fileOut, int length, char[] separators)
        {
            Console.WriteLine(new string('#', length));
            try
            {
                using (var sr = new StreamReader(fileIn))
                {

                    using (var sw = new StreamWriter(fileOut, true))
                    {
                        




                    }
                }
            }
            catch
            {
                Console.WriteLine("File Error");
            }
        }

        private static void writeLine(int length, List<string> words, int lineLength, bool lastLine)
        {
            int spaces = words.Count - 1;
            lineLength--;
            if (spaces != 0)
            {
                int count = length - lineLength;
                int countInOne = count / spaces + 1;
                int countOfLonger = count % spaces;


                for (int i = 0; i < words.Count - 1; i++)
                {
                    int number = 0;
                    /*if (words == words.Count)
                        number = 1;
                    else */
                    if (i < countOfLonger)
                        number = countInOne + 1;
                    else
                        number = countInOne;
                    Console.Write(words.ElementAt(i) + new String(' ', number));
                }
            }
            Console.Write(words.ElementAt(words.Count - 1));
            Console.Write('\n');
            if (!lastLine)
            {
                Console.Write('\n');
            }
        }
    }
}
