using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c = System.Console;

namespace grafy
{
    class Program
    {
        static void Main(string[] args)
        {
            cisla[] a = new cisla[3];
            a[0] = new cisla(0);
            a[1] = new cisla(1);
            a[2] = new cisla(2);








            Console.WriteLine((a[0]+a[1]).ToString());

            c.ReadKey();
        }
     }
    class cisla
    {
        private int x;
        public cisla(int x) {
            this.x = x;
        }
        public static int operator +(cisla a, cisla b) {
            return a.x + b.x;
        }
        public override string ToString()
        {
            return x.ToString();
        }
    }
}
