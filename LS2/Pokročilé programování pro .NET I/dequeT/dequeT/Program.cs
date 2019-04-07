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
            for (int i = 0; i < 200; i++)
            {
                array.Insert(0, i);
            }

            Console.WriteLine(array.Count);
            Console.WriteLine();





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

        public T this[int index]
        {
            get => array[index + frontColumn / blockSize][index + frontColumn % blockSize];
            set
            {
                if (array[index + frontColumn / blockSize]==null)
                {
                    array[index + frontColumn / blockSize] = new T[blockSize];
                }
                array[index + frontColumn / blockSize][index + frontColumn % blockSize] = value;
            }
        }
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
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator() => new DequeEnum<T>(array, blockSize, frontColumn, backColumn);

        public int IndexOf(T item)
        {
            for (int x = frontColumn + 1; x < backColumn; x++)
            {
                if (array[x / blockSize][x % blockSize].Equals(item))
                    return x - frontColumn-1;
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            for (int i = Count; i < index; i--)
            {
                array[i + 1] = array[i];
            }
            this[index] = item;
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
            int offset;
            if (frontColumn > Size-backColumn)
            {
                offset = 0;
            }
            else
            {
                offset = array.Length;
            }
            T[][] newArray = new T[array.Length*2][];
            for (int i = 0; i < array.Length; i++)
            {
                newArray[i+offset] = array[i];
            }
            array = newArray;
            frontColumn += offset*blockSize;
            backColumn += offset*blockSize;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
    public class DequeEnum<T> : IEnumerator<T>
    {
        public T[][] _array;
        int blockSize;
        int position;
        int beginPosition;
        int endPosition;

        public DequeEnum(T[][] list, int blockSize, int frontColumn, int backColumn)
        {
            _array = list;
            this.blockSize = blockSize;
            this.beginPosition = frontColumn;
            Reset();
            endPosition = backColumn;
        }

        public bool MoveNext()
        {
            position++;
            return (position < endPosition);
        }

        public void Reset()
        {
            position = beginPosition;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public T Current
        {
            get
            {
                try
                {
                    int x = position / blockSize;
                    int y = position % blockSize;
                    return _array[x][y];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }


    // Sometimes I think the compiler ignores my comments...
    // So, why even write comments if they get ignored anyway?
}
