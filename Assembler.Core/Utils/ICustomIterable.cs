using System;

namespace Assembler.Utils
{
    public abstract class ICustomIterable<E>
    {
        /// <summary>
        /// Current item in the Iterator fashion
        /// </summary>
        private int _current = -1;

        /// <summary>
        /// Inner Element array
        /// </summary>
        private E[] innerList = new E[10];

        /// <summary>
        /// Size counter
        /// </summary>
        public int Size { get; private set; } = 0;

        /// <summary>
        /// Add Element to Inner List
        /// </summary>
        /// <param name="instruction"></param>
        protected void Add(E obj)
        {
            // verify size of token array and increment if necessary
            if (Size >= innerList.Length / 2)
            {
                Array.Resize(ref innerList, innerList.Length * 2);
            }

            innerList[Size] = obj;
            Size++;
        }

        /// <summary>
        /// Current Element of the Iterator
        /// </summary>
        public E Current
        {
            get
            {
                if (_current == -1)
                {
                    _current++;
                }
                return innerList[_current];
            }
        }

        /// <summary>
        /// Previous Element of the Iterator
        /// </summary>
        public E Previous
        {
            get
            {
                if (_current <= 0)
                    return default;

                return innerList[_current - 1];
            }
        }

        /// <summary>
        /// Peek next Element of the Iterator, without moving Current
        /// </summary>
        /// <returns>Next Element of Current</returns>
        public E PeekNext()
        {
            if (_current + 1 >= Size)
                return default;

            return innerList[_current + 1];
        }

        /// <summary>
        /// Move to the next Element of the Iterator
        /// </summary>
        /// <returns>True if there is a next Element, False otherwise</returns>
        public bool MoveNext()
        {
            if (_current + 1 >= Size) return false;
            _current++;
            return true;
        }

        /// <summary>
        /// Move back one Element
        /// </summary>
        public void MoveBack()
        {
            if (_current - 1 < 0) return;
            _current--;
        }

        /// <summary>
        /// Reset the Current to the first Element of the Iterator
        /// </summary>
        public void Reset()
        {
            _current = -1;
        }
    }
}
