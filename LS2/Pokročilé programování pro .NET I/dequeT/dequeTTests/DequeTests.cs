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
        // init
        [TestMethod()]
        public void DequeTestInit()
        {
            var array = new Deque<int>();
            return;
        }
        // Add
        [TestMethod()]
        public void AddTest()
        {
            const int repetition = 20;
            var array = new Deque<int>();
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(repetition, array.Count);
        }
        [TestMethod()]
        public void AddLargeTest()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(repetition, array.Count);
        }
        [TestMethod()]
        public void AddTestReverse()
        {
            const int repetition = 20;
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < repetition; i++)
            {
                reverseView.Add(i);
            }
            Assert.AreEqual(repetition, array.Count);
            Assert.AreEqual(repetition, reverseView.Count);
            for (int i = 0; i < repetition; i++)
            {
                Assert.AreEqual(i, array[repetition - i - 1]);
            }
        }
        [TestMethod()]
        public void AddTestLargeReverse()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < repetition; i++)
            {
                reverseView.Add(i);
            }
            Assert.AreEqual(repetition, array.Count);
            Assert.AreEqual(repetition, reverseView.Count);
            for (int i = 0; i < repetition; i++)
            {
                Assert.AreEqual(i, array[repetition - i - 1]);
            }
        }
        // Indexer
        [TestMethod()]
        public void IndexerGetTest()
        {
            const int repetition = 20;
            var array = new Deque<int>();
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(0, array[0]);
            Assert.AreEqual(repetition - 1, array[repetition - 1]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var temp = array[-1];
            });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var temp = array[repetition];
            });
        }
        [TestMethod()]
        public void IndexerGetLargeTest()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(0, array[0]);
            Assert.AreEqual(repetition - 1, array[repetition - 1]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var temp = array[-1];
            });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var temp = array[repetition];
            });
        }
        [TestMethod()]
        public void IndexerGetReversedTest()
        {
            const int repetition = 20;
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(0, reverseView[repetition - 1]);
            Assert.AreEqual(repetition - 1, reverseView[0]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var temp = reverseView[-1];
            });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var temp = reverseView[repetition];
            });
        }
        [TestMethod()]
        public void IndexerGetReversedLargeTest()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(0, reverseView[repetition - 1]);
            Assert.AreEqual(repetition - 1, reverseView[0]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var temp = reverseView[-1];
            });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var temp = reverseView[repetition];
            });
        }
        [TestMethod()]
        public void IndexerSetTest()
        {
            const int repetition = 20;
            var array = new Deque<int>();
            for (int i = 0; i < repetition; i++)
            {
                array.Add(1);
            }
            array[0] = 3;
            array[repetition - 1] = 2;
            Assert.AreEqual(2, array[repetition - 1]);
            Assert.AreEqual(3, array[0]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                array[-1] = 42;
            });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                array[repetition] = 42;
            });
        }
        [TestMethod()]
        public void IndexerSetLargeTest()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            for (int i = 0; i < repetition; i++)
            {
                array.Add(1);
            }
            array[0] = 3;
            array[repetition - 1] = 2;
            Assert.AreEqual(2, array[repetition - 1]);
            Assert.AreEqual(3, array[0]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                array[-1] = 42;
            });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                array[repetition] = 42;
            });
        }
        [TestMethod()]
        public void IndexerImplicitSetLargeTest()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            for (int i = 0; i < repetition; i++)
            {
                array.Add(1);
            }
            for (int i = 0; i < repetition; i++)
            {
                array[i] = i;
            }
            for (int i = 0; i < repetition; i++)
            {
                array[i] = i;
            }
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                array[-1] = 42;
            });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                array[repetition] = 42;
            });
        }
        [TestMethod()]
        public void IndexerSetReversedTest()
        {
            const int repetition = 20;
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < repetition; i++)
            {
                array.Add(1);
            }
            reverseView[0] = 3;
            reverseView[repetition - 1] = 2;
            Assert.AreEqual(2, reverseView[repetition - 1]);
            Assert.AreEqual(3, reverseView[0]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                reverseView[-1] = 42;
            });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                reverseView[repetition] = 42;
            });
        }
        [TestMethod()]
        public void IndexerSetReversedLargeTest()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < repetition; i++)
            {
                array.Add(1);
            }
            reverseView[0] = 3;
            reverseView[repetition - 1] = 2;
            Assert.AreEqual(2, reverseView[repetition - 1]);
            Assert.AreEqual(3, reverseView[0]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                reverseView[-1] = 42;
            });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                reverseView[repetition] = 42;
            });
        }
        // First
        [TestMethod()]
        public void FirstTest()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i + 1);
            }
            Assert.AreEqual(1, array.First);
        }
        [TestMethod()]
        public void FirstReversedTest()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i + 1);
            }
            Assert.AreEqual(repetition, reverseView.First);
        }
        // Last
        [TestMethod()]
        public void LastTest()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i + 1);
            }
            Assert.AreEqual(repetition, array.Last);
        }
        [TestMethod()]
        public void LastReversedTest()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i + 1);
            }
            Assert.AreEqual(1, reverseView.Last);
        }
        // Count
        [TestMethod()]
        public void CountTest()
        {
            const int repetition = 400;
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(repetition, array.Count);
            Assert.AreEqual(repetition, reverseView.Count);
        }
        [TestMethod()]
        public void CountLargeTest()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i);
            }
            for (int i = 0; i < repetition; i++)
            {
                reverseView.Add(i);
            }
            Assert.AreEqual(2*repetition, array.Count);
            Assert.AreEqual(2*repetition, reverseView.Count);
        }
        [TestMethod()]
        public void CountReverseTest()
        {
            const int repetition = 400;
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < repetition; i++)
            {
                reverseView.Add(i);
            }
            Assert.AreEqual(repetition, array.Count);
            Assert.AreEqual(repetition, reverseView.Count);
        }
        [TestMethod()]
        public void CountZeroTest()
        {
            var array = new Deque<int>();
            Assert.AreEqual(0, array.Count);
        }
        [TestMethod()]
        public void CountZeroReversedTest()
        {
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            Assert.AreEqual(0, reverseView.Count);
        }
        // ReadOnly
        [TestMethod()]
        public void ReadOnlyTest()
        {
            const int repetition = 40;
            var array = new Deque<int>();
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(false, array.IsReadOnly);
            foreach (int s in array)
            {
                Assert.AreEqual(true, array.IsReadOnly);
            }
            Assert.AreEqual(false, array.IsReadOnly);
        }
        [TestMethod()]
        public void ReadOnlyReversedTest()
        {
            const int repetition = 40;
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(false, reverseView.IsReadOnly);
            foreach (int s in array)
            {
                Assert.AreEqual(true, reverseView.IsReadOnly);
            }
            Assert.AreEqual(false, reverseView.IsReadOnly);
            foreach (int s in reverseView)
            {
                Assert.AreEqual(true, array.IsReadOnly);
            }
            Assert.AreEqual(false, array.IsReadOnly);
        }
        // GetEnumerator
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
        }
        [TestMethod()]
        public void GetEnumeratorLargeTest()
        {
            const int repetition = 10000;
            int x = 0;
            var array = new Deque<int>();
            for (int i = x; i < repetition; i++)
            {
                array.Add(i);
            }
            foreach (var o in array)
            {
                Assert.AreEqual(x, o);
                x++;
            }
        }
        [TestMethod()]
        public void GetEnumeratorReversedTest()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i);
            }
            int x = repetition-1;
            foreach (var o in reverseView)
            {
                Assert.AreEqual(x--, o);
            }
        }
        // Clear
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
        public void ClearReversedTest()
        {
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            reverseView.Clear();
            Assert.AreEqual(0, array.Count);

            for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(20, reverseView.Count);
        }
        // Contains
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
        public void ContainsReversedTest()
        {
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(true, reverseView.Contains(10));
        }
        [TestMethod()]
        public void ContainsNotTest()
        {
            var array = new Deque<int>();
            for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(false, array.Contains(20));
        }
        // CopyTo
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
        public void CopyToReversedTest()
        {
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < 100; i++)
            {
                array.Add(i);
            }
            var newarray = new int[200];
            reverseView.CopyTo(newarray, 25);

            Assert.AreEqual(0, newarray[25]);
            Assert.AreEqual(1, newarray[26]);
            Assert.AreEqual(99, newarray[124]);
        }
        [TestMethod()]
        public void CopyToTestExceptions()
        {
            var array = new Deque<int>();
            for (int i = 0; i < 100; i++)
            {
                array.Add(i);
            }
            int[] newarray = null;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                array.CopyTo(newarray, 25);
            });
            newarray = new int[200];
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                array.CopyTo(newarray, -1);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                array.CopyTo(newarray, 190);
            });
        }
        // IndexOf
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
        public void IndexOfLargeTest()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            for (int i = 0; i < repetition+1; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(repetition, array.IndexOf(repetition));
        }
        [TestMethod()]
        public void IndexOfReverseTest()
        {
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(9, reverseView.IndexOf(10));
        }
        [TestMethod()]
        public void IndexOfNotTest()
        {
            var array = new Deque<int>();
            for (int i = 0; i < 20; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(-1, array.IndexOf(20));
        }
        // Insert
        [TestMethod()]
        public void InsertBasicTest()
        {
            var array = new Deque<int>();
            array.Insert(0, 1);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(1, array.First);
            Assert.AreEqual(1, array.Last);
        }
        [TestMethod()]
        public void InsertBasicReversedTest()
        {
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            array.Insert(0, 3);
            array.Insert(0, 2);
            array.Insert(0, 1);
            reverseView.Insert(0, 4);
            reverseView.Insert(0, 5);
            reverseView.Insert(0, 6);

            for (int i = 1; i < 7; i++)
            {
                Assert.AreEqual(i, array[i-1]);
            }

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
        public void InsertLargeTest()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            for (int i = 0; i < repetition; i++)
            {
                array.Insert(0, i);
            }
            Assert.AreEqual(repetition, array.Count, "count not equal");

            int x = repetition - 1;
            foreach (var o in array)
            {
                Assert.AreEqual(x, o);
                x--;
            }
        }
        [TestMethod()]
        public void InsertReverseTest()
        {
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < 200; i++)
            {
                reverseView.Insert(0, i);
            }
            Assert.AreEqual(200, array.Count, "count not equal");
            int x = 0;
            foreach (var o in array)
            {
                Assert.AreEqual(x++, o);
            }
        }
        // Remove
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
        public void RemoveLargeTest()
        {
            var array = new Deque<int>();
            for (int i = 0; i < 10000; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(true, array.Remove(9942));
            Assert.AreEqual(9999, array.Count);
            Assert.AreEqual(9941, array[9941]);
            Assert.AreEqual(9943, array[9942]);
            Assert.AreEqual(9944, array[9943]);
        }
        [TestMethod()]
        public void RemoveReverseTest()
        {
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < 3; i++)
            {
                array.Add(i);
            }
            Assert.AreEqual(true, reverseView.Remove(0));
            Assert.AreEqual(2, array.Count);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
        }
        [TestMethod()]
        public void RemoveNotTest()
        {
            var array = new Deque<int>();
            for (int i = 0; i < 100; i++)
            {
                array.Add(i);
            }            
            Assert.AreEqual(false, array.Remove(142));
            Assert.AreEqual(100, array.Count);
        }
        // RemoveAt
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
        [TestMethod()]
        public void RemoveAtReverseTest()
        {
            var array = new Deque<int>();
            var reverseView = DequeTest.GetReverseView(array);
            for (int i = 0; i < 3; i++)
            {
                array.Add(i);
            }
            reverseView.RemoveAt(0);
            Assert.AreEqual(2, array.Count);
            Assert.AreEqual(0, array[0]);
            Assert.AreEqual(1, array[1]);
        }
        // SupresedTests
        [TestMethod()]
        public void AddNullTest()
        {
            var array = new Deque<string>();
            var reverseView = DequeTest.GetReverseView(array);
            array.Add("tacocat");
            array.Add(null);
            array.Add("tacocat"); // writen reversed :D
            array.Add(null);
            Assert.AreEqual(1, array.IndexOf(null));
            Assert.AreEqual(0, reverseView.IndexOf(null));
        }
        [TestMethod()]
        public void SelfEqualityTest()
        {
            var array = new Deque<int> { 0, 1, 2, 3, 4, 5 };
            int i = 0;
            foreach (var item in array)
            {
                Assert.AreEqual(item, array[i++]);
            }
        }
        [TestMethod()]
        public void SelfEqualityLargeTest()
        {
            const int repetition = 10000;
            var array = new Deque<int>();
            for (int i = 0; i < repetition; i++)
            {
                array.Add(i);
            }
            int k = 0;
            foreach (var item in array)
            {
                Assert.AreEqual(item, array[k++]);
            }
            k = 0;
            foreach (var item in array)
            {
                Assert.AreEqual(item, k++);
            }
        }
        [TestMethod()]
        public void SelfEqualityReversedTest()
        {
            var array = new Deque<int> { 0, 1, 2, 3, 4, 5 };
            var reverseView = DequeTest.GetReverseView(array);
            int i = 0;
            foreach (var item in reverseView)
            {
                Assert.AreEqual(item, reverseView[i++]);
            }
        }
        [TestMethod()]
        public void EnumExceptionsTest()
        {
            var array = new Deque<int> { 0, 1, 2, 3, 4, 5 };
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                foreach (var item in array)
                {
                    array.Add(42);
                }
            });
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                foreach (var item in array)
                {
                    array.Clear();
                }
            });
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                foreach (var item in array)
                {
                    array.Insert(0, 42);
                }
            });
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                foreach (var item in array)
                {
                    array.Remove(42);
                }
            });
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                foreach (var item in array)
                {
                    array.RemoveAt(0);
                }
            });
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                foreach (var item in array)
                {
                    array.Reverse();
                }
            });
            var reverseView = DequeTest.GetReverseView(array);
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                foreach (var item in array)
                {
                    reverseView.Add(42);
                }
            });
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                foreach (var item in array)
                {
                    reverseView.Clear();
                }
            });
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                foreach (var item in array)
                {
                    reverseView.Insert(0, 42);
                }
            });
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                foreach (var item in array)
                {
                    reverseView.Remove(42);
                }
            });
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                foreach (var item in array)
                {
                    reverseView.RemoveAt(0);
                }
            });
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                foreach (var item in array)
                {
                    reverseView.Reverse();
                }
            });
        }
    }
}