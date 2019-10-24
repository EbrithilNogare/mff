using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace alignToBlock
{
    class Program {
        static void Main(string[] args)
        {
            Aligmentor a = new Aligmentor();
            a.AlignFiles(args);
        }
    }

    public class Aligmentor
    {
        int lineWidth;
        List<string> words = new List<string>();
        int lettersOnRow = 0;
        StreamWriter sw;
        int newLines = 0;
        bool newLineWaiting = false;
        char spaceSymbol = ' ';
        string newLineSymbol = "\n";

        public void AlignFiles(string[] args)
        {
            if (!ArgumentsCheck(args))
            {
                Console.WriteLine("Argument Error");
                return;
            }

            if (File.Exists(args[args.Length-2]))
            {
                File.Delete(args[args.Length - 2]);
            }

            for (int i = args[0]== "--highlight-spaces"?1:0; i < args.Length-2; i++)
            {
                AlignFile(args[i], args[args.Length-2]);
            }

            try
            {
                using (sw = new StreamWriter(args[args.Length - 2], true))
                {
                    sw.NewLine = newLineSymbol;
                    // clean buffer
                    WriteLineFromBuffer(false);
                }
            }
            catch
            {
                Console.WriteLine("File Error");
            }
        }

        bool ArgumentsCheck(string[] args)
        {
            if (args.Length < 3)
            {
                return false;
            }
            if (!(int.TryParse(args[args.Length-1], out int bin)) || !(int.Parse(args[args.Length-1]) > 0))
            {
                return false;
            }
            if (args[0] == "--highlight-spaces") {
                if (args.Length < 4) return false;
                spaceSymbol = '.';
                newLineSymbol = "<-" + newLineSymbol;
            }
            lineWidth = int.Parse(args[args.Length-1]);
            return true;
        }

        void AlignFile(string fileIn, string fileOut)
        {
            try
            {
                using (var sr = new StreamReader(fileIn))
                {
                    using (sw = new StreamWriter(fileOut,true))
                    {
                        sw.NewLine = newLineSymbol; // because of WIN line ending
                        StringBuilder actualWord = new StringBuilder(); // build word letter by letter
                        char actualSymbol;
                        while (sr.Peek() > -1)
                        {
                            actualSymbol = (char)sr.Read();

                            if (actualSymbol == '\r') continue; // is promised no \r will apear, but one never know

                            // look for two new lines, meaning paragraph
                            if (paragraphCheck(actualSymbol))
                            {
                                WriteLineFromBuffer(false);
                                newLineWaiting = true;
                            }

                            // letter or separator char?
                            if (IsSeparator(actualSymbol))
                            {
                                if (actualWord.Length != 0)
                                {
                                    AddWordToBuffer(actualWord.ToString(), actualSymbol);
                                    actualWord.Clear();
                                }
                            }
                            else
                            {
                                actualWord.Append(actualSymbol);
                            }
                        }
                        // save last word to buffer
                        if (actualWord.Length != 0)
                            AddWordToBuffer(actualWord.ToString(), ' ');
                    }
                }
            }
            catch
            {
                Console.WriteLine("File Error");
            }
        }

        private bool paragraphCheck(char actualSymbol)
        {
            if (actualSymbol == '\n')
                newLines++;

            if (!IsSeparator(actualSymbol))
                newLines = 0;

            // because of edge case 2x\n and than separator give two paragraphs
            if (newLines == 2)
            {
                newLines++;
                return true;
            }
            return false;
        }

        private void AddWordToBuffer(string word, char separator)
        {
            // check if we can pass it into buffer
            if (lettersOnRow + words.Count + word.Length > lineWidth)
            {
                WriteLineFromBuffer(true);
            }

            words.Add(word);
            lettersOnRow += word.Length;

            // check if word is super long
            if (word.Length > lineWidth)
            {
                WriteLineFromBuffer(false);
            }
        }

        bool IsSeparator(char symbol)
        {
            return symbol == ' ' || symbol == '\t' || symbol == '\n';
        }

        void WriteLineFromBuffer(bool blockAlign)
        {   // nothing to write, dont waste time
            if (words.Count == 0)
                return;

            // because of condition of new paragraph on end of file
            if (newLineWaiting)
            {
                sw.WriteLine();
                newLineWaiting = false;
            }

            // write all words from buffer to stream
            for (int i = 0; i < words.Count - 1; i++)
            {
                sw.Write(words[i]);

                // count spaces and where to put them
                int spacesCount = Math.Max(lineWidth - lettersOnRow, 0);
                int countInOne = spacesCount / (words.Count - 1);
                int countOfLonger = spacesCount % (words.Count - 1);

                if (i < countOfLonger)
                    sw.Write(new String(spaceSymbol, blockAlign ? countInOne + 1 : 1));
                else
                    sw.Write(new String(spaceSymbol, blockAlign ? countInOne : 1));
            }
            sw.WriteLine(words[words.Count - 1]);

            // reset all
            lettersOnRow = 0;
            words = new List<string>();
        }
    }
}
