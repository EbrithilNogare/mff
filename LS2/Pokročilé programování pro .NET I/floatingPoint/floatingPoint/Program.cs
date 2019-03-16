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
            Console.WriteLine($"3: {f1}");

            var f2 = new Fixed<Q24_8>(2);
            var f3 = f1.Add(f2);
            Console.WriteLine($"3 + 2: {f3}");

            Console.ReadKey();
        }
    }

    class Fixed<T> where T : Q24_8
    {
        public T number;
        public Fixed(int number) {
            this.number = number;
        }
        public double Add(Fixed<T> q)
        {
            return number.Add(q.number);
        }
    }
    abstract class Q
    {
        //public abstract double Add(Q n);
    }
    /*
    class Fixed<Q24_8> : Fixed{
        byte i1, i2, i3, f1;
        public Fixed(int n, bool obo = false)
        {
            i3 = (byte)(n & 0xFF);
            i2 = (byte)(n >> 8);
            i1 = (byte)(n >> 16);
            f1 = (byte)(0);
        }
        public Fixed<Q24_8> Add(Fixed<Q24_8> n)
        {
            Int32 a = BitConverter.ToInt32(new byte[] { i1, i2, i3, f1 }, 0);
            Int32 b = BitConverter.ToInt32(new byte[] { n.i1, n.i2, n.i3, n.f1 }, 0);
            return new Fixed<Q24_8>(a + b, true);
        }
    }
    */
    class Q24_8 : Q
    {
        public byte i1, i2, i3, f1;
        public Q24_8 (int n)
        {
            i3 = (byte)(n & 0xFF);
            i2 = (byte)(n >> 8);
            i1 = (byte)(n >> 16);
            f1 = (byte)(0);

        }
        public double Add(Q24_8 n)
        {
            Int32 a = BitConverter.ToInt32(new byte[] { i1, i2, i3, f1 }, 0);
            return (a + (int)n)/8;
        }
        public static implicit operator int(Q24_8 q)
        {
            return BitConverter.ToInt32(new byte[] { q.i1, q.i2, q.i3, q.f1 }, 0);
        }
    }
    /*/
    class Q16_16 : Q
    {
        private byte i1, i2, f1, f2;
        public Q16_16(byte i1, byte i2, byte f1, byte f2)
        {
            this.i1 = i1;
            this.i2 = i2;
            this.f1 = f1;
            this.f2 = f2;
        }
        public static explicit operator Q16_16(int integer)
        {
            byte i1 = (byte)(integer & 0xFF);
            byte i2 = (byte)(integer >> 8);
            byte f1 = (byte)(0);
            byte f2 = (byte)(0);
            return new Q16_16(i1, i2, f1, f2);
        }        
        public override string ToString()
        {
            return String.Format(i1 + i2 + "." + f1 + f2);
        }
    }
    class Q8_24 : Q
    {
        private byte i1, f1, f2, f3;
        public Q8_24(byte i1, byte f1, byte f2, byte f3)
        {
            this.i1 = i1;
            this.f1 = f1;
            this.f2 = f2;
            this.f3 = f3;
        }
        public static explicit operator Q8_24(int integer)
        {
            byte i1 = (byte)(integer & 0xFF);
            byte f1 = (byte)(0);
            byte f2 = (byte)(0);
            byte f3 = (byte)(0);
            return new Q8_24(i1, f1, f2, f3);
        }
        public override string ToString()
        {
            return String.Format(i1 + "." + f1 + f2 + f3);
        }
    }/**/
}