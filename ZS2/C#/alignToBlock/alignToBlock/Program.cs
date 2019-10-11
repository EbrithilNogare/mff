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
                        WriteLineFromBuffer(false);
                    }
                }
            }
            catch
            {
                Console.WriteLine("File Error");
            }
        }

        private static void AddWordToBuffer(string word, char separator)
        {
            // check if we can pass it into buffer
            if (lettersOnRow + words.Count + word.Length > lineWidth)
            {
                WriteLineFromBuffer(true);
            }

            words.Add(word);
            lettersOnRow += word.Length;

            // check if word is super long
            if (word.Length > lineWidth) {
                WriteLineFromBuffer(false);
            }

            // check if there is end of paragraph
            if (separator == '\n' && words.Count != 0)
            {
                WriteLineFromBuffer(false);
            }
        }

        static bool IsSeparator(char symbol)
        {
            return symbol == ' ' || symbol == '\t' || symbol == '\n';
        }

        static void WriteLineFromBuffer(bool blockAlign)
        {
            for (int i = 0; i < words.Count-1; i++)
            {
                sw.Write(words[i]);

                int spacesCount = lineWidth - lettersOnRow; // todo repair it
                if(spacesCount% words.Count - 1 > i)
                    sw.Write(new String(' ', spacesCount / words.Count + 1));
                else
                    sw.Write(new String(' ', spacesCount / words.Count));
            }
            sw.WriteLine(words[words.Count - 1]);

            // reset all
            lettersOnRow = 0;
            words = new List<string>();
        }
    }
}
