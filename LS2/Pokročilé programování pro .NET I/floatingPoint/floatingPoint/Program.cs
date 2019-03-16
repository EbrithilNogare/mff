using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("UnitTests")]

namespace floatingPoint
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Q24_8:");
            {
                var f1 = new Fixed<Q24_8>(3);
                Console.WriteLine($"3: {f1}");

                var f2 = new Fixed<Q24_8>(2);
                var f3 = f1.Add(f2);
                Console.WriteLine($"3 + 2: {f3}");

                f3 = f1.Multiply(f2);
                Console.WriteLine($"3 * 2: {f3}");

                f1 = new Fixed<Q24_8>(19);
                f2 = new Fixed<Q24_8>(13);
                f3 = f1.Multiply(f2);
                Console.WriteLine($"19 * 13: {f3}");

                f1 = new Fixed<Q24_8>(3);
                f2 = new Fixed<Q24_8>(2);
                f3 = f1.Divide(f2);
                Console.WriteLine($"3 / 2: {f3}");

                f1 = new Fixed<Q24_8>(248);
                f2 = new Fixed<Q24_8>(10);
                f3 = f1.Divide(f2);
                Console.WriteLine($"248 / 10: {f3}");

                f1 = new Fixed<Q24_8>(625);
                f2 = new Fixed<Q24_8>(1000);
                f3 = f1.Divide(f2);
                Console.WriteLine($"625 / 1000: {f3}");
            }

            //

            Console.WriteLine();
            Console.WriteLine("Q16_16:");
            {
                var f1 = new Fixed<Q16_16>(3);
                Console.WriteLine($"3: {f1}");

                var f2 = new Fixed<Q16_16>(2);
                var f3 = f1.Add(f2);
                Console.WriteLine($"3 + 2: {f3}");

                f3 = f1.Multiply(f2);
                Console.WriteLine($"3 * 2: {f3}");

                f1 = new Fixed<Q16_16>(19);
                f2 = new Fixed<Q16_16>(13);
                f3 = f1.Multiply(f2);
                Console.WriteLine($"19 * 13: {f3}");

                f1 = new Fixed<Q16_16>(248);
                f2 = new Fixed<Q16_16>(10);
                f3 = f1.Divide(f2);
                Console.WriteLine($"248 / 10: {f3}");

                f1 = new Fixed<Q16_16>(625);
                f2 = new Fixed<Q16_16>(1000);
                f3 = f1.Divide(f2);
                Console.WriteLine($"625 / 1000: {f3}");
            }

            //
            
            Console.WriteLine();
            Console.WriteLine("Q8_24:");
            {
                var f1 = new Fixed<Q8_24>(3);
                var f2 = new Fixed<Q8_24>(2);
                var f3 = f1.Add(f2);
                Console.WriteLine($"3 + 2: {f3}");

                f3 = f1.Multiply(f2);
                Console.WriteLine($"3 * 2: {f3}");

                f1 = new Fixed<Q8_24>(19);
                f2 = new Fixed<Q8_24>(13);
                f3 = f1.Multiply(f2);
                Console.WriteLine($"19 * 13: {f3}");

                f1 = new Fixed<Q8_24>(248);
                f2 = new Fixed<Q8_24>(10);
                f3 = f1.Divide(f2);
                Console.WriteLine($"248 / 10: {f3}");

                f1 = new Fixed<Q8_24>(625);
                f2 = new Fixed<Q8_24>(1000);
                f3 = f1.Divide(f2);
                Console.WriteLine($"625 / 1000: {f3}");
            }
            
            Console.ReadKey();
        }
    }

    class Fixed<T>where T:Q,new()
    {
        
        Int32 floatingPointNumber; //not a number, just 4bytes
        T precision;
        public Fixed(int integer, bool interpretAsFP = false)
        {
            precision = new T();
            if(interpretAsFP)
                floatingPointNumber = integer;
            else
                floatingPointNumber = precision.Init(integer);
        }

        public Fixed<T> Add(Fixed<T> q)
        {
            return new Fixed<T>(precision.Add(floatingPointNumber, q.floatingPointNumber), true);
        }
        public Fixed<T> Subtract(Fixed<T> q)
        {
            return new Fixed<T>(precision.Subtract(floatingPointNumber, q.floatingPointNumber), true);
        }
        public Fixed<T> Multiply(Fixed<T> q)
        {
            return new Fixed<T>(precision.Multiply(floatingPointNumber, q.floatingPointNumber), true);
        }
        public Fixed<T> Divide(Fixed<T> q)
        {
            return new Fixed<T>(precision.Divide(floatingPointNumber, q.floatingPointNumber), true);
        }
    public override string ToString()
        {
            return precision.ToString(floatingPointNumber);
        }
    }
    abstract class Q
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
        public int Subtract(int a, int b)
        {
            return a - b;
        }
        public abstract int Multiply(int a, int b);
        public abstract int Divide(int a, int b);
        public abstract string ToString(int floatingPointNumber);
        public abstract int Init(int floatingPointNumber);
    }
    class Q24_8 : Q
    {
        public override int Multiply(int a, int b)
        {
            long c = (long)a * (long)b;
            return (int)(c >> 8);
        }
        public override int Divide(int a, int b)
        {
            long c = 0;
            c = (long)(((long)a << 8) / ((long)b << 0));
            return (int)(c >> 0);
        }
        public override string ToString(int n)
        {
            return ((double)(n) / Math.Pow(2, 8)).ToString();
        }
        public override int Init(int floatingPointNumber)
        {
            return floatingPointNumber << 8;
        }
    }
    class Q16_16 : Q
    {
        public override int Multiply(int a, int b)
        {
            long c = (long)a * (long)b;
            return (int)(c >> 16);
        }
        public override int Divide(int a, int b)
        {
            long c = 0;
            c = (long)(((long)a << 16) / ((long)b << 0));
            return (int)(c >> 0);
        }
        public override string ToString(int n)
        {
            return ((double)(n) / Math.Pow(2, 16)).ToString();
        }
        public override int Init(int floatingPointNumber)
        {
            return floatingPointNumber << 16;
        }
    }
    class Q8_24 : Q
    {
        public override int Multiply(int a, int b)
        {
            long c = (long)a * (long)b;
            return (int)(c >> 24);
        }
        public override int Divide(int a, int b)
        {
            long c = 0;
            c = (long)(((long)a << 24) / ((long)b << 0));
            return (int)(c >> 0);
        }
        public override string ToString(int n)
        {
            return ((double)(n) / Math.Pow(2, 24)).ToString();
        }
        public override int Init(int floatingPointNumber)
        {
            return floatingPointNumber << 24;
        }
    }
}