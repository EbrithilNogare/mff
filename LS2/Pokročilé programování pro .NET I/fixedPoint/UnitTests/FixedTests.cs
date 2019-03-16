using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cuni.Arithmetics.FixedPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass()]
    public class FixedQ24_8
    {
        [TestMethod()]
        public void Init()
        {
            var f1 = new Fixed<Q24_8>(3);
            Assert.AreEqual("3", f1.ToString());
        }
        [TestMethod()]
        public void Add()
        {
            var f1 = new Fixed<Q24_8>(3);
            var f2 = new Fixed<Q24_8>(2);
            var f3 = f1.Add(f2);
            Assert.AreEqual("5", f3.ToString());
        }
        [TestMethod()]
        public void Subtract()
        {
            var f1 = new Fixed<Q24_8>(3);
            var f2 = new Fixed<Q24_8>(2);
            var f3 = f1.Subtract(f2);
            Assert.AreEqual("1", f3.ToString());
        }
        [TestMethod()]
        public void Multiply()
        {
            var f1 = new Fixed<Q24_8>(3);
            var f2 = new Fixed<Q24_8>(2);
            var f3 = f1.Multiply(f2);
            Assert.AreEqual("6", f3.ToString());
        }
        [TestMethod()]
        public void MultiplyHigher()
        {
            var f1 = new Fixed<Q24_8>(19);
            var f2 = new Fixed<Q24_8>(13);
            var f3 = f1.Multiply(f2);
            Assert.AreEqual("247", f3.ToString());
        }
        [TestMethod()]
        public void Divide()
        {
            var f1 = new Fixed<Q24_8>(3);
            var f2 = new Fixed<Q24_8>(2);
            var f3 = f1.Divide(f2);
            Assert.AreEqual(((double)1.5).ToString(), f3.ToString());
        }
        [TestMethod()]
        public void DivideHigher()
        {
            var f1 = new Fixed<Q24_8>(248);
            var f2 = new Fixed<Q24_8>(10);
            var f3 = f1.Divide(f2);
            Assert.AreEqual(((double)24.796875).ToString(), f3.ToString());
        }
        [TestMethod()]
        public void DivideHigher2()
        {
            var f1 = new Fixed<Q24_8>(625);
            var f2 = new Fixed<Q24_8>(1000);
            var f3 = f1.Divide(f2);
            Assert.AreEqual(((double)0.625).ToString(), f3.ToString());
        }

    }

    [TestClass()]
    public class FixedQ16_16
    {
        [TestMethod()]
        public void Init()
        {
            var f1 = new Fixed<Q16_16>(3);
            Assert.AreEqual("3", f1.ToString());
        }
        [TestMethod()]
        public void Add()
        {
            var f1 = new Fixed<Q16_16>(3);
            var f2 = new Fixed<Q16_16>(2);
            var f3 = f1.Add(f2);
            Assert.AreEqual("5", f3.ToString());
        }
        [TestMethod()]
        public void Subtract()
        {
            var f1 = new Fixed<Q16_16>(3);
            var f2 = new Fixed<Q16_16>(2);
            var f3 = f1.Subtract(f2);
            Assert.AreEqual("1", f3.ToString());
        }
        [TestMethod()]
        public void Multiply()
        {
            var f1 = new Fixed<Q16_16>(3);
            var f2 = new Fixed<Q16_16>(2);
            var f3 = f1.Multiply(f2);
            Assert.AreEqual("6", f3.ToString());
        }
        [TestMethod()]
        public void MultiplyHigher()
        {
            var f1 = new Fixed<Q16_16>(19);
            var f2 = new Fixed<Q16_16>(13);
            var f3 = f1.Multiply(f2);
            Assert.AreEqual("247", f3.ToString());
        }
        [TestMethod()]
        public void DivideHigher()
        {
            var f1 = new Fixed<Q16_16>(248);
            var f2 = new Fixed<Q16_16>(10);
            var f3 = f1.Divide(f2);
            Assert.AreEqual(((double)24.7999877929688).ToString(), f3.ToString());
        }
        [TestMethod()]
        public void DivideHigher2()
        {
            var f1 = new Fixed<Q16_16>(625);
            var f2 = new Fixed<Q16_16>(1000);
            var f3 = f1.Divide(f2);
            Assert.AreEqual(((double)0.625).ToString(), f3.ToString());
        }

    }

    [TestClass()]
    public class FixedQ8_24
    {
        [TestMethod()]
        public void Init()
        {
            var f1 = new Fixed<Q8_24>(3);
            Assert.AreEqual("3", f1.ToString());
        }
        [TestMethod()]
        public void Add()
        {
            var f1 = new Fixed<Q8_24>(3);
            var f2 = new Fixed<Q8_24>(2);
            var f3 = f1.Add(f2);
            Assert.AreEqual("5", f3.ToString());
        }
        [TestMethod()]
        public void Subtract()
        {
            var f1 = new Fixed<Q8_24>(3);
            var f2 = new Fixed<Q8_24>(2);
            var f3 = f1.Subtract(f2);
            Assert.AreEqual("1", f3.ToString());
        }
        [TestMethod()]
        public void Multiply()
        {
            var f1 = new Fixed<Q8_24>(3);
            var f2 = new Fixed<Q8_24>(2);
            var f3 = f1.Multiply(f2);
            Assert.AreEqual("6", f3.ToString());
        }
        [TestMethod()]
        public void MultiplyHigher()
        {
            var f1 = new Fixed<Q8_24>(19);
            var f2 = new Fixed<Q8_24>(13);
            var f3 = f1.Multiply(f2);
            Assert.AreEqual("-9", f3.ToString());
        }
        [TestMethod()]
        public void DivideHigher()
        {
            var f1 = new Fixed<Q8_24>(248);
            var f2 = new Fixed<Q8_24>(10);
            var f3 = f1.Divide(f2);
            Assert.AreEqual(((double)-0.799999952316284).ToString(), f3.ToString());
        }
        [TestMethod()]
        public void DivideHigher2()
        {
            var f1 = new Fixed<Q8_24>(625);
            var f2 = new Fixed<Q8_24>(1000);
            var f3 = f1.Divide(f2);
            Assert.AreEqual(((double)-4.70833331346512).ToString(), f3.ToString());
        }

    }
}