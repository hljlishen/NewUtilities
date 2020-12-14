using System;
using System.Collections.Generic;

namespace Utilities.Collections
{
    public class OneOnOneMap<Item1Type, Item2Type>
    {
        private readonly Dictionary<Item1Type, Item2Type> map = new Dictionary<Item1Type, Item2Type>();
        private readonly Dictionary<Item2Type, Item1Type> reverseMap = new Dictionary<Item2Type, Item1Type>();

        public void Add(Item1Type t1, Item2Type t2)
        {
            if (map.ContainsKey(t1))
                throw new ItemAlreadyExistException($"{t1}对象已经存在于映射中");
            if (reverseMap.ContainsKey(t2))
                throw new ItemAlreadyExistException($"{t2}对象已经存在于映射中");

            map.Add(t1, t2);
            reverseMap.Add(t2, t1);
        }

        public Item1Type Find(Item2Type t2) => reverseMap[t2];
        public Item2Type Find(Item1Type t1) => map[t1];

        public void Remove(Item1Type t1)
        {
            if (!map.ContainsKey(t1))
                return;

            var t2 = map[t1];
            map.Remove(t1);
            reverseMap.Remove(t2);
        }

        public void Remove(Item2Type t2)
        {
            if (!reverseMap.ContainsKey(t2))
                return;

            var t1 = reverseMap[t2];
            map.Remove(t1);
            reverseMap.Remove(t2);
        }

        public IEnumerable<Item1Type> Item1s => map.Keys;
        public IEnumerable<Item2Type> Item2s => map.Values;

        public bool ContainsItem(Item1Type t1) => map.ContainsKey(t1);
        public bool ContainsItem(Item2Type t2) => reverseMap.ContainsKey(t2);
    }

    public class ItemAlreadyExistException : Exception
    {
        public ItemAlreadyExistException(string msg) : base(msg)
        { 
        }
    }
}
