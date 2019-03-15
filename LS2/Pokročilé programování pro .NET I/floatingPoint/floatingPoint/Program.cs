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
    class Fixed<T> where T : Q
    {
        private T t;
        public Fixed(T t)
        {
            this.t = t;
        }

        public Q24_8 Add(Fixed<Q24_8> n)
        {
            var nn = n.t;
            Q24_8 tt = t;
            Int32 a = BitConverter.ToInt32(new byte[] { tt.i1, tt.i2, tt.i3, tt.f1, }, 0);
            Int32 b = BitConverter.ToInt32(new byte[] { nn.i1, nn.i2, nn.i3, nn.f1, }, 0);
            return (a + b);
        }

        public override string ToString()
        {
            return t.ToString();
        }

    }

    abstract class Q {

    }
    
    class Q24_8:Q
    {
        public byte i1, i2, i3, f1;
        public Q24_8(byte i1, byte i2, byte i3, byte f1)
        {
            this.i1 = i1;
            this.i2 = i2;
            this.i3 = i3;
            this.f1 = f1;
        }
        public static implicit operator Q24_8(int integer)
        {
            byte i3 = (byte)(integer & 0xFF);
            byte i2 = (byte)(integer >> 8);
            byte i1 = (byte)(integer >> 16);
            byte f1 = (byte)(0);
            return new Q24_8(i1, i2, i3, f1);
        }
        public override string ToString()
        {
            return String.Format(i1*16 + i2*8 + i3 + "." + f1);
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