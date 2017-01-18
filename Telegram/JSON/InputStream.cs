using System;
using System.IO;

namespace Telegram
{
    /// <summary>
    /// Custom MemoryReader.
    /// </summary>
    public class InputStream
    {
        private string data = "";
        private int p = 0;

        public InputStream(string data)
        {
            this.data = data;
        }

        /// <summary>
        /// Returns the next character in the stream and increments the pointer.
        /// </summary>
        public char Next()
        {
            return (p < data.Length) ? data[p++] : '\0';
        }

        /// <summary>
        /// Peeks at the next character without moving the pointer.
        /// </summary>
        public char Peek()
        {
            return (p < data.Length) ? data[p] : '\0';
        }

        /// <summary>
        /// Checks if the EOF has been reached.
        /// </summary>
        public bool EOF()
        {
            return (p == data.Length || data[p] == '\0'); 
        }

        /// <summary>
        /// Easy error messaging.
        /// </summary>
        /// <param name="msg">Error message</param>
        public void Err(string msg)
        {
            throw new Exception(msg + " (char: '" + Peek() + "')");
        }
    }
}
