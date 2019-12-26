using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    public class SnakeQueue<T> where T: ICloneable
    {
        private readonly Queue<T> _inner = new Queue<T>();

        public SnakeQueue(Queue<T> queue)
        {
            _inner = queue;
        }

        public int Count
        {
            get
            {
                return _inner.Count;
            }
        }

        public T Last { get; private set; }

        public void Enqueue(T item)
        {
            _inner.Enqueue(item);
            Last = (T)item.Clone();
        }

        public T Dequeue()
        {
            return _inner.Dequeue();
        }

        public T Peek()
        {
            return _inner.Peek();
        }
    }
}
