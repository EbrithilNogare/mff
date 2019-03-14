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

            var a = new Q24_8(8);





            Console.ReadKey();
        }
    }
    class Fixed<T> where T : new()
    {
        T number;
        public Fixed(int integer)
        {
            var instance = Activator.CreateInstance(typeof(T),
                  new object[] { int }) as T;
            number = new instance(integer);
        }

        public T Add() {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return number.ToString();
        }
    }
    
    struct Q24_8
    {
        private byte i1, i2, i3, f1;
        public Q24_8(int integer)
        {
            i1 = (byte)(integer & 0xFF);
            i2 = (byte)(integer >> 8);
            i3 = (byte)(integer >> 16);
            f1 = (byte)(0);
        }
        public static explicit operator Q24_8(int integer)
        {
            return new Q24_8(integer);
        }
    }
    /*/
    struct Q16_16
    {
        private byte i1, i2, f1, f2;
        public Q16_16(int integer)
        {
            i1 = (byte)(integer & 0xFF);
            i2 = (byte)(integer >> 8);
            f1 = f2 = (byte)(0);
        }
    }
    struct Q8_24
    {
        private byte i1, f1, f2, f3;
        public Q8_24(int integer)
        {
            i1 = (byte)(integer & 0xFF);
            f1 = f2 = f3 = (byte)(0);
        }
    }
    /*/
}