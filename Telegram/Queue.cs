using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram
{
    /// <summary>
    /// Simple Custom Generic Queue class. </summary>
    public class Queue<T>
    {
        // Fields
        private T[] data;
        private byte start, end;
        private byte maxSize = 255;

        public Queue(byte size = 255) { maxSize = size; data = new T[maxSize]; start = 0; end = 0; }

        /// <summary>
        /// Getter for the size of the queue.
        /// </summary>
        public int Size
        {
            get { return (end < start) ? (10 - start + end) : (end - start); }
        }

        /// <summary>
        /// Enqueue an object into the queue.
        /// </summary>
        public void Enqueue(T o)
        {
            if (end >= maxSize) end = 0; // Cycle
            if (end + 1 == start) throw new Exception("Queue out of bounds");

            data[end++] = o;
        }

        /// <summary>
        /// Dequeue an object from the start of the queue.
        /// </summary>
        public T Dequeue()
        {
            if (start >= maxSize) start = 0; // Cycle
            if (start == end) throw new Exception("Queue is empty");

            return data[start++];
        }

        /// <summary>
        /// Peek at the beginning of the queue (next item).
        /// </summary>
        public T Peek()
        {
            return data[(start >= maxSize) ? 0 : start];
        }

        /// <summary>
        /// Peek at the end of the queue (lastly added item).
        /// </summary>
        public T PeekEnd()
        {
            return data[(end >= maxSize) ? 0 : end];
        }
    }
}
