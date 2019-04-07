using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dequeT
{
    class Program
    {
        static void Main(string[] args)
        {
            var array = new Deque<int>();
            for (int i = 0; i < 300; i++)
            {
                array.Add(i);
            }
            foreach(var o in array)
                Console.WriteLine(o);

            Console.WriteLine("done");
            Console.ReadKey();
        }
    }
    public class Deque<T> : IList<T>
    {
        //  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        //  |  |  |  |  |  |  |  | .|. |  |  |  |  |  |  |  |
        //  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        //  front                                        back
        
        static int blockSize = (int)Math.Pow(2,4);
        T[][] array;
        int frontColumn;
        int backColumn;

        public Deque()
        {
            Clear(); // init
        }

        public T this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Size => array.Length * blockSize;

        public bool IsReadOnly => false;

        public int Count => backColumn - frontColumn - 1;

        public void Add(T item)
        {
            if (backColumn == Size)
            {
                DoubleSize();
            }
            if (backColumn % blockSize == 0)
            {
                array[backColumn / blockSize] = new T[blockSize];
            }
            array[backColumn / blockSize][backColumn % blockSize] = item;
            backColumn++;
        }

        public void Clear()
        {
            array = new T[2][];
            frontColumn = blockSize - 1;
            backColumn = blockSize;
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public Deque<T> GetEnumerator()
        {
            return new DequeEnum<T>(array);
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }
        

        private void DoubleSize()
        {
            int offset = array.Length / 2;
            T[][] newArray = new T[array.Length*2][];
            for (int i = offset; i < offset * 3; i++)
            {
                newArray[i] = array[i - offset];
            }
            array = newArray;
            frontColumn += offset*blockSize;
            backColumn += offset*blockSize;
        }
    }
    public class DequeEnum<T> : IEnumerator
    {
        public Deque<T>[][] _array;

        int position = -1;

        public DequeEnum(Deque<T>[][] list)
        {
            _array = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _array.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Deque<T> Current
        {
            get
            {
                try
                {
                    return _array[position / _array[0].Length][position % _array[0].Length];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }


}
