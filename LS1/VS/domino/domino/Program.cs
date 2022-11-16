using System;
using System.Collections.Generic;

namespace domino
{
    class Program
    {
        static void Main(string[] args)
        {
            string vstup = "";
            string tempVstup;
            while((tempVstup = Console.ReadLine()) != null)
            {
                vstup += tempVstup + " ";
            }
            while (vstup.Contains("  ")) vstup = vstup.Replace("  ", " ");
            string[] vstupArray = vstup.Split(' ');
            int n = int.Parse(vstupArray[0]);
            List<int> kostkyX = new List<int>();
            List<int> kostkyY = new List<int>();
            int max = 0;

            for (int i = 1; i <= n; i++)
            {
                kostkyX.Add(int.Parse(vstupArray[2 * i - 1]));
                kostkyY.Add(int.Parse(vstupArray[2 * i]));
            }


            Console.WriteLine(pridej(kostkyX, kostkyY, 0, max));
            Console.ReadKey();
        }

        static int pridej(List<int> kostkyX, List<int> kostkyY, int predchudce, int max)
        {
            int tempMax = max;
            int tempTempMax = max;
            for (int i = 0; i < kostkyX.Count; i++)
            {
                if (kostkyX[i] == predchudce || predchudce == 0)
                {
                    List<int> tempX = new List<int>(kostkyX);
                    List<int> tempY = new List<int>(kostkyY);
                    tempX.RemoveAt(i);
                    tempY.RemoveAt(i);
                    tempTempMax = pridej(tempX, tempY, kostkyY[i], max + 1);
                    if (tempTempMax > tempMax) tempMax = tempTempMax;
                }
            }
            for (int i = 0; i < kostkyY.Count; i++)
            {
                if (kostkyY[i] == predchudce || predchudce == 0)
                {
                    List<int> tempX = new List<int>(kostkyX);
                    List<int> tempY = new List<int>(kostkyY);
                    tempX.RemoveAt(i);
                    tempY.RemoveAt(i);
                    tempTempMax = pridej(tempX, tempY, kostkyX[i], max + 1);
                    if (tempTempMax > tempMax) tempMax = tempTempMax;
                }
            }
            return tempMax;
        }
    }
}
