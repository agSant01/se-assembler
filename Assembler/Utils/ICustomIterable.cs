using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Utils
{
    public abstract class ICustomIterable<E>
    {
        private int _current = -1;

        private E[] innerList = new E[10];

        public int Size { get; private set; } = 0;

        /// <summary>
        /// Add E to inner list
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
        /// Current element of the Iterator
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
        /// Previous element of the Iterator
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
        /// Peek next element of the Iterator, without moving the Current
        /// </summary>
        /// <returns></returns>
        public E PeekNext()
        {
            if (_current + 1 >= Size)
                return default;

            return innerList[_current + 1];
        }

        /// <summary>
        /// Move to the next element of the Iterator
        /// </summary>
        /// <returns>True if there is a next element, False otherwise</returns>
        public bool MoveNext()
        {
            if (_current + 1 >= Size) return false;
            _current++;
            return true;
        }

        /// <summary>
        /// Move back one element
        /// </summary>
        public void MoveBack()
        {
            if (_current - 1 < 0) return;
            _current--;
        }

        /// <summary>
        /// Reset the Current to the first element of the Iterator
        /// </summary>
        public void Reset()
        {
            _current = -1;
        }
    }
}
