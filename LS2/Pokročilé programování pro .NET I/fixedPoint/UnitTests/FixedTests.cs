using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cuni.Arithmetics.FixedPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicArithmeticTests
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

namespace AdvancedOperatorTests
{
    [TestClass()]
    public class FixedQ24_8
    {
        [TestMethod()]
        public void Convert()
        {
            Fixed<Q24_8> f1 = 3;
            Assert.AreEqual("3", f1.ToString());
        }
        [TestMethod()]
        public void OperatorPlus()
        {
            //Init
            var f1 = new Fixed<Q24_8>(3);
            var f2 = new Fixed<Q24_8>(2);

            //Test
            var f3 = f1 + f2;

            //Check
            Assert.AreEqual("5", f3.ToString());
        }
        [TestMethod()]
        public void OperatorMinus()
        {
            var f1 = new Fixed<Q24_8>(3);
            var f2 = new Fixed<Q24_8>(2);
            var f3 = f1 - f2;
            Assert.AreEqual("1", f3.ToString());
        }
        [TestMethod()]
        public void OperatorMultiply()
        {
            var f1 = new Fixed<Q24_8>(3);
            var f2 = new Fixed<Q24_8>(2);
            var f3 = f1 * f2;
            Assert.AreEqual("6", f3.ToString());
        }
        [TestMethod()]
        public void OperatorDivide()
        {
            var f1 = new Fixed<Q24_8>(3);
            var f2 = new Fixed<Q24_8>(2);
            var f3 = f1 / f2;
            Assert.AreEqual(((double)1.5).ToString(), f3.ToString());
        }

        [TestMethod()]
        public void OperatorPlusInt()
        {
            var f1 = new Fixed<Q24_8>(3);
            var f3 = f1 + 2;
            Assert.AreEqual("5", f3.ToString());
        }

        [TestMethod()]
        public void Equation()
        {
            var f1 = new Fixed<Q24_8>(3);
            var f2 = new Fixed<Q24_8>(3);
            Assert.AreEqual(true, f1 == f2);
        }
        [TestMethod()]
        public void Equation2()
        {
            var f1 = new Fixed<Q24_8>(3);
            var f2 = new Fixed<Q24_8>(4);
            Assert.AreEqual(false, f1 == f2);
        }

        [TestMethod()]
        public void NEquation()
        {
            var f1 = new Fixed<Q24_8>(3);
            var f2 = new Fixed<Q24_8>(3);
            Assert.AreEqual(false, f1 != f2);
        }
        [TestMethod()]
        public void NEquation2()
        {
            var f1 = new Fixed<Q24_8>(3);
            var f2 = new Fixed<Q24_8>(4);
            Assert.AreEqual(true, f1 != f2);
        }
    }

    [TestClass()]
    public class FixedQ16_16
    {
        [TestMethod()]
        public void Convert()
        {
            Fixed<Q16_16> f1 = 3;
            Assert.AreEqual("3", f1.ToString());
        }
        [TestMethod()]
        public void OperatorPlus()
        {
            //Init
            var f1 = new Fixed<Q16_16>(3);
            var f2 = new Fixed<Q16_16>(2);

            //Test
            var f3 = f1 + f2;

            //Check
            Assert.AreEqual("5", f3.ToString());
        }
        [TestMethod()]
        public void OperatorMinus()
        {
            var f1 = new Fixed<Q16_16>(3);
            var f2 = new Fixed<Q16_16>(2);
            var f3 = f1 - f2;
            Assert.AreEqual("1", f3.ToString());
        }
        [TestMethod()]
        public void OperatorMultiply()
        {
            var f1 = new Fixed<Q16_16>(3);
            var f2 = new Fixed<Q16_16>(2);
            var f3 = f1 * f2;
            Assert.AreEqual("6", f3.ToString());
        }
        [TestMethod()]
        public void OperatorDivide()
        {
            var f1 = new Fixed<Q16_16>(3);
            var f2 = new Fixed<Q16_16>(2);
            var f3 = f1 / f2;
            Assert.AreEqual(((double)1.5).ToString(), f3.ToString());
        }

        [TestMethod()]
        public void OperatorPlusInt()
        {
            var f1 = new Fixed<Q16_16>(3);
            var f3 = f1 + 2;
            Assert.AreEqual("5", f3.ToString());
        }

        [TestMethod()]
        public void Equation()
        {
            var f1 = new Fixed<Q16_16>(3);
            var f2 = new Fixed<Q16_16>(3);
            Assert.AreEqual(true, f1 == f2);
        }
        [TestMethod()]
        public void Equation2()
        {
            var f1 = new Fixed<Q16_16>(3);
            var f2 = new Fixed<Q16_16>(4);
            Assert.AreEqual(false, f1 == f2);
        }

        [TestMethod()]
        public void NEquation()
        {
            var f1 = new Fixed<Q16_16>(3);
            var f2 = new Fixed<Q16_16>(3);
            Assert.AreEqual(false, f1 != f2);
        }
        [TestMethod()]
        public void NEquation2()
        {
            var f1 = new Fixed<Q16_16>(3);
            var f2 = new Fixed<Q16_16>(4);
            Assert.AreEqual(true, f1 != f2);
        }
    }

    [TestClass()]
    public class FixedQ8_24
    {
        [TestMethod()]
        public void Convert()
        {
            Fixed<Q8_24> f1 = 3;
            Assert.AreEqual("3", f1.ToString());
        }
        [TestMethod()]
        public void OperatorPlus()
        {
            //Init
            var f1 = new Fixed<Q8_24>(3);
            var f2 = new Fixed<Q8_24>(2);

            //Test
            var f3 = f1 + f2;

            //Check
            Assert.AreEqual("5", f3.ToString());
        }
        [TestMethod()]
        public void OperatorMinus()
        {
            var f1 = new Fixed<Q8_24>(3);
            var f2 = new Fixed<Q8_24>(2);
            var f3 = f1 - f2;
            Assert.AreEqual("1", f3.ToString());
        }
        [TestMethod()]
        public void OperatorMultiply()
        {
            var f1 = new Fixed<Q8_24>(3);
            var f2 = new Fixed<Q8_24>(2);
            var f3 = f1 * f2;
            Assert.AreEqual("6", f3.ToString());
        }
        [TestMethod()]
        public void OperatorDivide()
        {
            var f1 = new Fixed<Q8_24>(3);
            var f2 = new Fixed<Q8_24>(2);
            var f3 = f1 / f2;
            Assert.AreEqual(((double)1.5).ToString(), f3.ToString());
        }

        [TestMethod()]
        public void OperatorPlusInt()
        {
            var f1 = new Fixed<Q8_24>(3);
            var f3 = f1 + 2;
            Assert.AreEqual("5", f3.ToString());
        }

        [TestMethod()]
        public void Equation()
        {
            var f1 = new Fixed<Q8_24>(3);
            var f2 = new Fixed<Q8_24>(3);
            Assert.AreEqual(true, f1 == f2);
        }
        [TestMethod()]
        public void Equation2()
        {
            var f1 = new Fixed<Q8_24>(3);
            var f2 = new Fixed<Q8_24>(4);
            Assert.AreEqual(false, f1 == f2);
        }

        [TestMethod()]
        public void NEquation()
        {
            var f1 = new Fixed<Q8_24>(3);
            var f2 = new Fixed<Q8_24>(3);
            Assert.AreEqual(false, f1 != f2);
        }
        [TestMethod()]
        public void NEquation2()
        {
            var f1 = new Fixed<Q8_24>(3);
            var f2 = new Fixed<Q8_24>(4);
            Assert.AreEqual(true, f1 != f2);
        }
    }

    [TestClass()]
    public class Fixed
    {
        [TestMethod()]
        public void Convert24to8()
        {
            Fixed<Q24_8> f1 = 1;
            Fixed<Q24_8> f2 = 32;
            Fixed<Q24_8> f3 = f1 / f2;
            Fixed<Q8_24> f4 = new Fixed<Q8_24>();

            f4 = (Fixed<Q8_24>)f3;

            Assert.AreEqual(f3.ToString(), f4.ToString());
        }
        [TestMethod()]
        public void Convert24to16()
        {
            Fixed<Q24_8> f1 = 1;
            Fixed<Q24_8> f2 = 32;
            Fixed<Q24_8> f3 = f1 / f2;
            Fixed<Q16_16> f4 = new Fixed<Q16_16>();

            f4 = (Fixed<Q16_16>)f3;

            Assert.AreEqual(f3.ToString(), f4.ToString());
        }
        [TestMethod()]
        public void Convert16to24()
        {
            Fixed<Q16_16> f1 = 1;
            Fixed<Q16_16> f2 = 32;
            Fixed<Q16_16> f3 = f1 / f2;
            Fixed<Q24_8> f4 = new Fixed<Q24_8>();

            f4 = (Fixed<Q24_8>)f3;

            Assert.AreEqual(f3.ToString(), f4.ToString());
        }
        [TestMethod()]
        public void Convert16to8()
        {
            Fixed<Q16_16> f1 = 1;
            Fixed<Q16_16> f2 = 32;
            Fixed<Q16_16> f3 = f1 / f2;
            Fixed<Q8_24> f4 = new Fixed<Q8_24>();

            f4 = (Fixed<Q8_24>)f3;

            Assert.AreEqual(f3.ToString(), f4.ToString());
        }
        [TestMethod()]
        public void Convert8to24()
        {
            Fixed<Q8_24> f1 = 1;
            Fixed<Q8_24> f2 = 32;
            Fixed<Q8_24> f3 = f1 / f2;
            Fixed<Q24_8> f4 = new Fixed<Q24_8>();

            f4 = (Fixed<Q24_8>)f3;

            Assert.AreEqual(f3.ToString(), f4.ToString());
        }
        [TestMethod()]
        public void Convert8to16()
        {
            Fixed<Q8_24> f1 = 1;
            Fixed<Q8_24> f2 = 32;
            Fixed<Q8_24> f3 = f1 / f2;
            Fixed<Q16_16> f4 = new Fixed<Q16_16>();

            f4 = (Fixed<Q16_16>)f3;

            Assert.AreEqual(f3.ToString(), f4.ToString());
        }
    }
}