using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

//Author: David Nápravník

namespace first
{
    class Program
    {
        static void Main(string[] args)
        {
            int START = 2000000000;
            int count = 0;
            float f = START;
            for (; f < START + 50; f++)
            {
                count++;
            }
            Console.WriteLine(count);
        }
    }
}
