using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Collections
{
    public class Slide<T> : IEnumerable<T>
    {
        private readonly List<T> items = new List<T>();
        public bool IsFull => items.Count >= Size;

        public Slide(uint size)
        {
            Size = size;
        }

        public void Add(T t)
        {
            while (items.Count >= Size)
                items.RemoveAt(0);
            items.Add(t);
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public T ElementAt(uint index)
        {
            if (index >= Size)
                throw new IndexOutOfRangeException($"index的数值超过了长度{Size}");
            return items.ElementAt((int)index);
        }

        public uint Size { get; set; } = 1;
        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
    }
}
