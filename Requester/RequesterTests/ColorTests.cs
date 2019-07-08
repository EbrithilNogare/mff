using Microsoft.VisualStudio.TestTools.UnitTesting;
using Requester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requester.Tests
{
    [TestClass()]
    public class ColorTests
    {
        [TestMethod()]
        public void ColorInitTest()
        {
            Color color = new Color("#ffffff");
            Assert.AreEqual((byte)255, color.r);
            Assert.AreEqual((byte)255, color.g);
            Assert.AreEqual((byte)255, color.b);
        }
        [TestMethod()]
        public void ColoHexTest()
        {
            Color color = new Color("#a82");
            Assert.AreEqual((byte)170, color.r);
            Assert.AreEqual((byte)136, color.g);
            Assert.AreEqual((byte)34, color.b);
        }
        [TestMethod()]
        public void ColoHexException1Test()
        {
            Assert.ThrowsException<ArgumentException>(() => {
                Color color = new Color("#5a82");
            });
        }
        [TestMethod()]
        public void ColoHexException2Test()
        {
            Assert.ThrowsException<ArgumentException>(() => {
                Color color = new Color("#g00");
            });
        }
        [TestMethod()]
        public void ColoRGBTest()
        {
            Color color = new Color("rgb(170,136,34)");
            Assert.AreEqual((byte)170, color.r);
            Assert.AreEqual((byte)136, color.g);
            Assert.AreEqual((byte)34, color.b);
        }
        [TestMethod()]
        public void ColoRGBWithSpacesTest()
        {
            Color color = new Color("rgb(170, 136, 34)");
            Assert.AreEqual((byte)170, color.r);
            Assert.AreEqual((byte)136, color.g);
            Assert.AreEqual((byte)34, color.b);
        }
        [TestMethod()]
        public void ColorException1Test()
        {
            Assert.ThrowsException<ArgumentException>(() => {
                Color color = new Color("rgba(170, 136, 34, 0.2)");
                });
        }
    }
}