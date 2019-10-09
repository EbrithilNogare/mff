#define testing
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace alignToBlock
{
    class Program
    {
        static int lineWidth; 
        static List<string> words = new List<string>();
        static int lettersOnRow = 0;
        static StreamWriter sw;

        static void Main(string[] args)
        {
            if (!ArgumentsCheck(args))
            {
                Console.WriteLine("Argument Error");
                return;
            }

            lineWidth = int.Parse(args[2]);
            AlignFile(args[0], args[1]);

#if testing
            Console.WriteLine("done");
            Console.ReadKey();
#endif
        }

        static bool ArgumentsCheck(string[] args)
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

        static void AlignFile(string fileIn, string fileOut)
        {
            try
            {
                using (var sr = new StreamReader(fileIn))
                {
                    using (sw = new StreamWriter(fileOut, false))
                    {
                        StringBuilder actualWord = new StringBuilder();
                        char actualSymbol;
                        while (sr.Peek() > -1)
                        {
                            actualSymbol = (char)sr.Read();
                            if (IsSeparator(actualSymbol)){
                                AddWordToBuffer(actualWord.ToString(), actualSymbol);
                                actualWord.Clear();
                            }
                            else{
                                actualWord.Append(actualSymbol);
                            }
                        }
                        WriteLeftWords();
                    }
                }
            }
            catch
            {
                Console.WriteLine("File Error");
            }
        }

        private static void WriteLeftWords()
        {
            WriteLineFromBuffer(words.Count, false);
        }

        private static void AddWordToBuffer(string word, char separator)
        {
            words.Add(word);
            lettersOnRow += word.Length;

            if (lettersOnRow + words.Count - 1 > lineWidth)
            {
                WriteLineFromBuffer(Math.Max(words.Count - 1, 1));
            }
            if (separator == '\n')
            {
                WriteLeftWords();
            }
        }

        static bool IsSeparator(char symbol)
        {
            return symbol == ' ' || symbol == '\t' || symbol == '\n';
        }

        static void WriteLineFromBuffer(int count, bool blockAlign=true)
        {
            for (int i = 0; i < count-1; i++)
            {
                sw.Write(words[i]);

                int spacesCount = lineWidth - lettersOnRow;
                if(spacesCount%count-1 > i)
                    sw.Write(new String(' ', spacesCount / count + 1));
                else
                    sw.Write(new String(' ', spacesCount / count));
            }
            sw.WriteLine(words[count-1]);

            // reset all
            lettersOnRow = 0;
            string leftWord = words[words.Count - 1];
            words = new List<string>() { leftWord};
        }
    }
}
