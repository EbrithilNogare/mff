//#define RecoDex
//#define yield
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
#if !RecoDex
namespace DequeSpace
{
#endif
    public class Deque<T> : IList<T>
    {
        //  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        //  |  |  |  |  |  |  |  | .|. |  |  |  |  |  |  |  |
        //  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        //  front                                        back
        private static int blockSize { get; } = 64;
        /// <summary>
        /// true  -> add to back
        /// false -> add to front
        /// </summary>
        private bool direction = true;
        private DequeArray<T> array;

        public Deque()
        {
            this.array = new DequeArray<T>(blockSize);
        }
        public T this[int index]
        {
            get
            {
                if (index >= Count || index < 0) throw new ArgumentOutOfRangeException();
                if (direction)
                {
                    return array.Data[(index + array.Front + 1) / blockSize][(index + array.Front + 1) % blockSize];
                }
                else
                {
                    return array.Data[(array.Back - index - 1) / blockSize][(array.Back - index - 1) % blockSize];
                }
            }
            set
            {
                if (IsReadOnly) throw new InvalidOperationException();
                if (index >= Count || index < 0) throw new ArgumentOutOfRangeException();
                if (direction)
                {
                    if (array.Data[(index + array.Front + 1) / blockSize] == null)
                    {
                        array.Data[(index + array.Front + 1) / blockSize] = new T[blockSize];
                    }
                    array.Data[(index + array.Front + 1) / blockSize][(index + array.Front + 1) % blockSize] = value;
                }
                else
                {
                    if (array.Data[(array.Back - index - 1) / blockSize] == null)
                    {
                        array.Data[(array.Back - index - 1) / blockSize] = new T[blockSize];
                    }
                    array.Data[(array.Back - index - 1) / blockSize][(array.Back - index - 1) % blockSize] = value;
                }
            }
        }
        public int Capacity => array.Data.Length * blockSize;
        public T First => this[0];
        public T Last => this[Count - 1];
        public int Count => array.Back - array.Front - 1;
        public bool IsReadOnly { get => array.IsReadOnly; set { array.IsReadOnly = value; } }
        public void Add(T item)
        {
            if (IsReadOnly) throw new InvalidOperationException();
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
            if (array.Back == Capacity)
            {
                DoubleSize();
            }
            if (array.Back % blockSize == 0)
            {
                array.Data[array.Back / blockSize] = new T[blockSize];
            }
            array.Data[array.Back / blockSize][array.Back % blockSize] = item;
            array.Back++;
        }
        private void AddFront(T item)
        {
            if (array.Front == 0)
            {
                DoubleSize();
            }
            if (array.Front % blockSize == blockSize - 1)
            {
                array.Data[array.Front / blockSize] = new T[blockSize];
            }
            array.Data[array.Front / blockSize][array.Front % blockSize] = item;
            array.Front--;
        }
        public void Clear()
        {
            if (IsReadOnly) throw new InvalidOperationException();
            this.array.Data = new T[2][];
            this.array.Front = blockSize - 1;
            this.array.Back = blockSize;
        }
        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException();
            if (arrayIndex < 0 || arrayIndex >= array.Length) throw new ArgumentOutOfRangeException();
            if (array.Length - arrayIndex < Count) throw new ArgumentException();

            if (direction)
            {
                for (int i = 0; i < Count; i++)
                {
                    array[i + arrayIndex] = this[i];
                }
            }
            else
            {
                for (int i = 0; i < Count; i++)
                {
                    array[i + arrayIndex] = this[Count - i - 1];
                }
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            IsReadOnly = true;
            #if yield
            return new DequeYieldEnum<T>(array, Count, blockSize, direction);
            #else
            return new DequeEnum<T>(array, Count, blockSize, direction);
            #endif
        }
        public int IndexOf(T item)
        {
            if (direction)
            {
                for (int x = array.Front + 1; x < array.Back; x++)
                {
                    if (Object.Equals(array.Data[x / blockSize][x % blockSize], item))
                        return x - array.Front - 1;
                }
                return -1;
            }
            else
            {
                for (int x = array.Back - 1; x > array.Front; x--)
                {
                    if (Object.Equals(array.Data[x / blockSize][x % blockSize], item))
                        return Count - (x - array.Front);
                }
                return -1;
            }
        }
        public void Insert(int index, T item)
        {
            if (IsReadOnly) throw new InvalidOperationException();

            bool shorterDirection = Count / 2 > index;

            if (shorterDirection) Reverse();

            Add(item);
            for (int i = Count - 1; i > (shorterDirection?(Count-1-index):index); i--)
            {
                this[i] = this[i - 1];
            }

            if (shorterDirection) Reverse();
            this[index] = item;
        }
        public bool Remove(T item)
        {
            if (IsReadOnly) throw new InvalidOperationException();
            int index = IndexOf(item);
            if (index == -1)
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

            bool shorterDirection = Count / 2 > index;

            if (shorterDirection) Reverse();

            for (int i = (shorterDirection ? (Count - 1 - index) : index); i < Count - 1; i++)
            {
                this[i] = this[i + 1];
            }
            if (direction)
                array.Back--;
            else
                array.Front++;
            if (shorterDirection) Reverse();
        }
        private void DoubleSize()
        {
            int offset;
            if (array.Front > Capacity - array.Back)
            {
                offset = 0;
            }
            else
            {
                offset = array.Data.Length;
            }
            T[][] newArray = new T[array.Data.Length * 2][];
            for (int i = 0; i < array.Data.Length; i++)
            {
                newArray[i + offset] = array.Data[i];
            }
            array.Data = newArray;
            array.Front += offset * blockSize;
            array.Back += offset * blockSize;
        }
        public Deque<T> GetReversed()
        {
            var reversed = new Deque<T>();
            reversed.array = this.array;
            reversed.direction = !this.direction;
            return reversed;

        }
        public void Reverse()
        {
            if (IsReadOnly) throw new InvalidOperationException();
            direction = !direction;
        }
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        public class DequeEnum<U> : IEnumerator<U>
        {
            public DequeArray<U> array;
            int blockSize;
            int position;
            int beginPosition;
            int endPosition;
            int count;
            bool direction;

            public DequeEnum(DequeArray<U> deque, int count, int blockSize, bool direction)
            {
                array = deque;
                this.blockSize = blockSize;
                this.beginPosition = deque.Front;
                this.count = count;
                Reset();
                endPosition = deque.Back;
                this.direction = direction;
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
                array.IsReadOnly = false;
                GC.SuppressFinalize(this);
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public U Current
            {
                get
                {
                    int x, y;
                    if (direction)
                    {
                        x = position / blockSize;
                        y = position % blockSize;
                    }
                    else
                    {
                        x = (beginPosition + count - (position - beginPosition - 1)) / blockSize;
                        y = (beginPosition + count - (position - beginPosition - 1)) % blockSize;
                    }

                    if (x >= 0 && x < array.Data.Length)
                    {
                        return array.Data[x][y];
                    }
                    throw new InvalidOperationException();
                }
            }
        }
        public class DequeYieldEnum<U> : IEnumerator<U>
        {
            public DequeArray<U> array;
            int blockSize;
            int position;
            int beginPosition;
            int endPosition;
            int count;
            bool direction;

            public DequeYieldEnum(DequeArray<U> deque, int count, int blockSize, bool direction)
            {
                array = deque;
                this.blockSize = blockSize;
                this.beginPosition = deque.Front;
                this.count = count;
                Reset();
                endPosition = deque.Back;
                this.direction = direction;
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
                array.IsReadOnly = false;
                GC.SuppressFinalize(this);
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public U Current
            {
                get
                {
                    int x, y;
                    if (direction)
                    {
                        x = position / blockSize;
                        y = position % blockSize;
                    }
                    else
                    {
                        x = (beginPosition + count - (position - beginPosition - 1)) / blockSize;
                        y = (beginPosition + count - (position - beginPosition - 1)) % blockSize;
                    }

                    if (x >= 0 && x < array.Data.Length)
                    {
                        return array.Data[x][y];
                    }
                    throw new InvalidOperationException();
                }
            }
        }
    }
    public class DequeArray<T>
    {
        public T[][] Data { get; set; }
        public int Front { get; set; }
        public int Back { get; set; }
        public bool IsReadOnly { get; set; }
        public DequeArray(int blockSize)
        {
            Data = new T[2][];
            Front = blockSize - 1;
            Back = blockSize;
            IsReadOnly = false;
        }
    }
    public static class DequeTest
    {
        public static Deque<T> GetReverseView<T>(Deque<T> deque)
        {
            if (deque == null) throw new ArgumentNullException();
            return deque.GetReversed();
        }
    }

    // Sometimes I think the compiler ignores my comments...
    // So, why even write comments if they get ignored anyway?

#if !RecoDex
}
#endif
