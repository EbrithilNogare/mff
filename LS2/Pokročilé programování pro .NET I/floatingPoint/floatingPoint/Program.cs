using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace floatingPoint
{
    class Program
    {
        static void Main(string[] args)
        {
            var f1 = new Fixed<Q24_8>(3);
            var f2 = new Fixed<Q24_8>(2);
            var f3 = f1.Multiply(f2);
            Console.WriteLine($"a:{f1}, b={f2}, vysledek: {f3}");


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
        public abstract string ToString(int floatingPointNumber);
        public abstract int Init(int floatingPointNumber);
    }
    class Q24_8 : Q
    {
        public override int Multiply(int a, int b)
        {
            a = a & 0x00FF;
            b = b & 0x00FF;
            long c = a * b;


            return (byte)(c >> 8);
        }
        public override string ToString(int n)
        {
            byte f1 = (byte)(n >> 0);
            byte i3 = (byte)(n >> 8);
            byte i2 = (byte)(n >> 16);
            byte i1 = (byte)(n >> 24);

            if (f1 == 0)
                return (i1 * 16 + i2 * 8 + i3).ToString();
            else
                return String.Format(i1 * 16 + i2 * 8 + i3 + "." + f1);
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
            a = a & 0x00FF;
            b = b & 0x00FF;
            long c = a * b;


            return (byte)(c >> 16);
        }
        public override string ToString(int n)
        {
            byte f2 = (byte)(n >> 0);
            byte f1 = (byte)(n >> 8);
            byte i1 = (byte)(n >> 16);
            byte i2 = (byte)(n >> 24);

            if (f1 == 0 && f2 == 0)
                return (i2 * 8 + i1).ToString();
            else
                return String.Format(i2 * 8 + i1 + "." + f1 + f2);
        }
        public override int Init(int floatingPointNumber)
        {
            return floatingPointNumber << 16;
        }
    }
}