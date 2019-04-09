using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
/**///releaseSwitch
namespace DequeSpace
{
/**/
    public class Deque<T> : IList<T>
    {
        //  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        //  |  |  |  |  |  |  |  | .|. |  |  |  |  |  |  |  |
        //  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        //  front                                        back
        
        static int blockSize = (int)Math.Pow(2,4);
        /// <summary>
        /// true  -> add to back
        /// false -> add to front
        /// </summary>
        bool direction = true;
        static T[][] array = new T[2][];
        static int frontColumn;
        static int backColumn;


        public Deque()
        {
            frontColumn = blockSize - 1;
            backColumn = blockSize;
        }

        public T this[int index]
        {
            get
            {
                if (index >= Count || index < 0) throw new ArgumentOutOfRangeException();
                if (direction)
                {
                    return array[(index + frontColumn + 1) / blockSize][(index + frontColumn + 1) % blockSize];
                }
                else
                {
                    return array[(Size - index - frontColumn+1) / blockSize][(Size - index - frontColumn+1) % blockSize];
                }
            }
            set
            {
                if (IsReadOnly) throw new InvalidOperationException();
                if (direction)
                {
                    if (array[(index + frontColumn + 1) / blockSize] == null)
                    {
                        array[(index + frontColumn + 1) / blockSize] = new T[blockSize];
                    }
                    array[(index + frontColumn + 1) / blockSize][(index + frontColumn + 1) % blockSize] = value;
                }
                else
                {
                    if (array[(Count - index - frontColumn) / blockSize] == null)
                    {
                        array[(Count - index - frontColumn) / blockSize] = new T[blockSize];
                    }
                    array[(Size - index - frontColumn) / blockSize][(Size - index - frontColumn) % blockSize] = value;
                }
            }
        }
        public int Size => array.Length * blockSize;
        public T First => this[0];
        
        public T Last => this[Count - 1];

        public int Count => backColumn - frontColumn - 1;
        public bool IsReadOnly { get; set; } = false;


        public void Add(T item)
        {
            if (direction)
            {
                AddBack(item);
            }
            else
            {
                AddFront(item);
            }
        }
        private void AddBack(T item)
        {
            if (IsReadOnly) throw new InvalidOperationException();
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
        private void AddFront(T item)
        {
            if (IsReadOnly) throw new InvalidOperationException();
            if (frontColumn == 0)
            {
                DoubleSize();
            }
            if (frontColumn % blockSize == blockSize-1)
            {
                array[frontColumn / blockSize] = new T[blockSize];
            }
            array[frontColumn / blockSize][frontColumn % blockSize] = item;
            frontColumn--;
        }

        public void Clear()
        {
            if (IsReadOnly) throw new InvalidOperationException();
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
            for (int i = 0; i < Count; i++)
            {
                array[i + arrayIndex] = this[i];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            IsReadOnly = true;
            return new DequeEnum<T>(array, blockSize, frontColumn, backColumn, this);
        }

        public int IndexOf(T item)
        {
            if (direction)
            {
                for (int x = frontColumn + 1; x < backColumn; x++)
                {
                    if (Object.Equals(array[x / blockSize][x % blockSize], item))
                        return x - frontColumn - 1;
                }
                return -1;
            }
            else
            {
                for (int x = frontColumn + 1; x < backColumn; x++)
                {
                    if (Object.Equals(array[x / blockSize][x % blockSize], item))
                        return Count - (x - frontColumn);
                }
                return -1;
            }
        }

        public void Insert(int index, T item)
        {
            if (IsReadOnly) throw new InvalidOperationException();
            Add(item);
            for (int i = Count-1; i > index; i--)
            {
                this[i] = this[i-1];
            }
            this[index] = item;
        }

        public bool Remove(T item)
        {
            if (IsReadOnly) throw new InvalidOperationException();
            int index = IndexOf(item);
            if (index==-1)
            {
                return false;
            }
            RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            if (index >= Count || index < 0) throw new ArgumentOutOfRangeException();
            if (IsReadOnly) throw new InvalidOperationException();
            for (int i = index; i < Count-1; i++)
            {
                this[i] = this[i+1];
            }
            backColumn--;
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
        public Deque<T> Reverse()
        {
            int temp1 = frontColumn, temp2 = backColumn;
            var toReturn = new Deque<T>();
            toReturn.direction = false;
            frontColumn = temp1;
            backColumn = temp2;
            return toReturn;
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
        Deque<T> readOnlyFallBack;

        public DequeEnum(T[][] list, int blockSize, int frontColumn, int backColumn, Deque<T> readOnlyFallBack)
        {
            _array = list;
            this.blockSize = blockSize;
            this.beginPosition = frontColumn;
            Reset();
            endPosition = backColumn;
            this.readOnlyFallBack = readOnlyFallBack;
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
            readOnlyFallBack.IsReadOnly = false;
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
    public static class DequeTest
    {
        public static IList<T> GetReverseView<T>(Deque<T> d)
        {
            return d.Reverse();
        }
    }


// Sometimes I think the compiler ignores my comments...
// So, why even write comments if they get ignored anyway?

/**///releaseSwitch
}
/**/
