using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telegram;

namespace TelegramTests
{
    [TestClass]
    public class QueueTest
    {
        [TestMethod]
        public void QueueInit()
        {
            Queue<int> q = new Queue<int>();
            Assert.IsNotNull(q, "QueueInit> Failed to initialize");
        }

        [TestMethod]
        public void QueueEnqueue()
        {
            Queue<int> q = new Queue<int>();
            Assert.IsTrue(q.Size == 0, "QueueEnqueue> Invalid Size");

            int o = 6;
            q.Enqueue(o);
            Assert.IsTrue(q.Size == 1, "QueueEnqueue> Invalid size");
            Assert.IsTrue(q.Peek() == o, "QueueEnqueue> Invalid item");
        }

        [TestMethod]
        public void QueueDequeue()
        {
            Queue<int> q = new Queue<int>();
            int o = 6; q.Enqueue(o);
            Assert.IsTrue(q.Size == 1, "QueueEnqueue> Invalid Size");

            int x = q.Dequeue();
            Assert.IsTrue(q.Size == 0, "QueueEnqueue> Invalid size");
            Assert.IsTrue(x == o, "QueueEnqueue> Invalid item");
        }

        [TestMethod]
        public void QueueSize()
        {
            Queue<int> q = new Queue<int>(20);
            Assert.IsTrue(q.Size == 0, "QueueSize> Invalid Size");

            for (int i = 1; i <= 10; i++)
            {
                q.Enqueue(0);
                Assert.IsTrue(q.Size == i, "QueueSize> Invalid Size Loop");
            }
            
            for (int i = 9; i > 0; i--)
            {
                q.Dequeue();
                Assert.IsTrue(q.Size == i, "QueueSize> Invalid Size Loop");
            }

        }

        [TestMethod]
        public void QueueCycle()
        {
            Queue<int> q = new Queue<int>(10);

            for (int i = 1; i<=10; i++)     // 10x Enqueue. {start = 0; end = 10}
                q.Enqueue(i);

            for (int i = 1; i<=9; i++)      // 9x Dequeue.  {start = 9; end = 10}
            {
                int o = q.Dequeue();
                Assert.IsTrue(o == i, "QueueCycle> Invalid element found");
            }
            Assert.IsTrue(q.Size == 1, "QueueCycle> Invalid Size");

            q.Enqueue(11);                  // Enqueue      {start = 9; end = 0}
            Assert.IsTrue(q.Size == 2, "QueueCycle> Invalid Size");
            Assert.IsTrue(q.Peek() == 10, "QueueCycle> Invalid element found");
            q.Enqueue(12);                  // Enqueue      {start = 9; end = 1}
            Assert.IsTrue(q.Size == 3, "QueueCycle> Invalid Size");
            Assert.IsTrue(q.Peek() == 10, "QueueCycle> Invalid element found");
            q.Dequeue();                    // Dequeue      {start = 10; end = 1}
            Assert.IsTrue(q.Size == 2, "QueueCycle> Invalid Size");
            Assert.IsTrue(q.Peek() == 11, "QueueCycle> Invalid element found");
            q.Dequeue();                    // Dequeue      {start = 0; end = 1}
            Assert.IsTrue(q.Size == 1, "QueueCycle> Invalid Size");
            Assert.IsTrue(q.Peek() == 12, "QueueCycle> Invalid element found");

        }

        [TestMethod]
        public void QueueCollide()
        {
            Queue<int> q = new Queue<int>(10);

            try
            {
                for (int i = 0; i < 11; i++) q.Enqueue(i);
            }
            catch (Exception e) { Assert.IsTrue(e.Message == "Queue out of bounds", "QueueCollide> Unexpected exception"); }

            try
            {
                for (int i = 0; i < 11; i++) q.Dequeue();
            }
            catch (Exception e) { Assert.IsTrue(e.Message == "Queue is empty", "QueueCollide> Unexpected exception"); }
        }
    }
}
