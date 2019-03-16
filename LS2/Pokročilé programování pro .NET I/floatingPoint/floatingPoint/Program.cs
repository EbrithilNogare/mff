using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("UnitTests")]

namespace FixedPoint
{
    public class Fixed<T>where T:Q,new()
    {        
        Int32 fixedPointNumber; //not a number, just 4bytes
        T precision;
        public Fixed(int integer, bool interpretAsFP = false)
        {
            precision = new T();
            if(interpretAsFP)
                fixedPointNumber = integer;
            else
                fixedPointNumber = precision.Init(integer);
        }

        public Fixed<T> Add(Fixed<T> q)
        {
            return new Fixed<T>(precision.Add(fixedPointNumber, q.fixedPointNumber), true);
        }
        public Fixed<T> Subtract(Fixed<T> q)
        {
            return new Fixed<T>(precision.Subtract(fixedPointNumber, q.fixedPointNumber), true);
        }
        public Fixed<T> Multiply(Fixed<T> q)
        {
            return new Fixed<T>(precision.Multiply(fixedPointNumber, q.fixedPointNumber), true);
        }
        public Fixed<T> Divide(Fixed<T> q)
        {
            return new Fixed<T>(precision.Divide(fixedPointNumber, q.fixedPointNumber), true);
        }
    public override string ToString()
        {
            return precision.ToString(fixedPointNumber);
        }
    }
    public abstract class Q
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
        public abstract string ToString(int fixedPointNumber);
        public abstract int Init(int fixedPointNumber);
    }
    public class Q24_8 : Q
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
        public override int Init(int fixedPointNumber)
        {
            return fixedPointNumber << 8;
        }
    }
    public class Q16_16 : Q
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
        public override int Init(int fixedPointNumber)
        {
            return fixedPointNumber << 16;
        }
    }
    public class Q8_24 : Q
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
        public override int Init(int fixedPointNumber)
        {
            return fixedPointNumber << 24;
        }
    }
}