using Microsoft.VisualStudio.TestTools.UnitTesting;
using dequeT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dequeT.Tests
{
    [TestClass()]
    public class DequeTests
    {
        [TestMethod()]
        public void DequeTest()
        {
            var array = new Deque<int>();
            return;
        }
        [TestMethod()]
        public void AddTest()
        {
            var array = new Deque<int>(); for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            return;
        }
        [TestMethod()]
        public void CountTest()
        {
            var array = new Deque<int>();
            for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(20, array.Count);
        }
        [TestMethod()]
        public void GetEnumeratorTest()
        {
            int x = 0;
            var array = new Deque<int>();
            for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            foreach (var o in array)
            {
                Assert.AreEqual(x, o);
                x++;
            }
            return;
        }

        [TestMethod()]
        public void ClearTest()
        {
            var array = new Deque<int>(); for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            array.Clear();
            Assert.AreEqual(0, array.Count);
        }

        [TestMethod()]
        public void ContainsTest()
        {
            var array = new Deque<int>();
            for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(true, array.Contains(10));
        }
        [TestMethod()]
        public void Contains2Test()
        {
            var array = new Deque<int>();
            for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(false, array.Contains(20));
        }

        [TestMethod()]
        public void CopyToTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void IndexOfTest()
        {
            var array = new Deque<int>();
            for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(10, array.IndexOf(10));
        }
        [TestMethod()]
        public void IndexOfTest2()
        {
            var array = new Deque<int>();
            for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(-1, array.IndexOf(20));
        }

        [TestMethod()]
        public void InsertTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void RemoveTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void RemoveAtTest()
        {
            throw new NotImplementedException();
        }
    }
}