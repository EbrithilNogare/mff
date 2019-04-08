using Microsoft.VisualStudio.TestTools.UnitTesting;
using DequeSpace;
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
            var array = new Deque<int>();
            for (int i = 0; i < 20; i++)
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
        public void CountTest2()
        {
            var array = new Deque<int>();
            Assert.AreEqual(0, array.Count);
        }
        [TestMethod()]
        public void GetEnumeratorTest()
        {
            int x = 0;
            var array = new Deque<int>();
            for (int i = x; i < 20; i++)
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
            var array = new Deque<int>();
            for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            array.Clear();
            Assert.AreEqual(0, array.Count);

            for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(20, array.Count);
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
            var array = new Deque<int>();
            for (int i = 0; i < 100; i++)
            {
                array.Add(i);
            }
            var newarray = new int[200];
            array.CopyTo(newarray, 25);

            Assert.AreEqual(0, newarray[25]);
            Assert.AreEqual(1, newarray[26]);
            Assert.AreEqual(99, newarray[124]);
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
            var array = new Deque<int>();
            for (int i = 0; i < 200; i++)
            {
                array.Insert(0, i);
            }
            Assert.AreEqual(200, array.Count, "count not equal");

            int x = 199;
            foreach (var o in array)
            {
                Assert.AreEqual(x, o);
                x--;
            }
        }

        [TestMethod()]
        public void RemoveTest()
        {
            var array = new Deque<int>();
            for (int i = 0; i < 100; i++)
            {
                array.Add(i);
            }

            Assert.AreEqual(true, array.Remove(42));
            Assert.AreEqual(99, array.Count);
            Assert.AreEqual(41, array[41]);
            Assert.AreEqual(43, array[42]);
            Assert.AreEqual(44, array[43]);
        }
        [TestMethod()]
        public void RemoveTest2()
        {
            var array = new Deque<int>();
            for (int i = 0; i < 100; i++)
            {
                array.Add(i);
            }

            
            Assert.AreEqual(false, array.Remove(142));
            Assert.AreEqual(100, array.Count);
        }

        [TestMethod()]
        public void RemoveAtTest()
        {
            var array = new Deque<int>();
            for (int i = 0; i < 100; i++)
            {
                array.Add(i);
            }

            array.RemoveAt(42);
            Assert.AreEqual(99, array.Count);
            Assert.AreEqual(41, array[41]);
            Assert.AreEqual(43, array[42]);
            Assert.AreEqual(44, array[43]);
        }
    }
}