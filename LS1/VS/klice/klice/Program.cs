using System;
using System.Collections.Generic;

namespace klice
{
    public static class Program
    {
        static int count = 0;

        static void Main(string[] args)
        {
            int k=0, l=0;
            while (k == 0 || l == 0)
            {
                int a;
                string temp = Console.ReadLine();
                if (int.TryParse(temp, out a))
                if (k == 0)
                    k = a;
                else
                    l = a;
            }

            permutations(new List<int>(),  k, l);


            Console.WriteLine(count);
            Console.ReadKey();

        }
        

        static void permutations(List<int> text, int numberOfDigits, int numberOfChars)
        {
            if (numberOfDigits > 0)
            {
                int j = text.Count == 0?0:text[text.Count - 1] - 1;
                while ((text.Count == 0 && j < numberOfChars) || (text.Count != 0 && j <= text[text.Count - 1] + 1))
                {
                    if (j >= 0 && j < numberOfChars)
                    {
                        List<int> text2 = new List<int>(text); ;
                        text2.Add(j);
                        permutations(text2, numberOfDigits - 1, numberOfChars);
                    }
                    j++;
                }
            }
            else
                count++;
        }
    }


}