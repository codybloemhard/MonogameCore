using System;
using System.Collections.Generic;

namespace Core
{
    public class OrderedSet<T>
    {
        private List<T> orderedSet;
        private IComparer<T> comparer;

        public OrderedSet(IComparer<T> comparer)
        {
            orderedSet = new List<T>();
            this.comparer = comparer;
        }

        internal void Add(T obj)
        {
            lock (orderedSet)
            {
                int index = orderedSet.BinarySearch(obj, comparer);
                if (index < 0)
                    orderedSet.Insert(~index, obj);
                else
                    orderedSet.Insert(index, obj);
            }
        }

        public List<T> Set { get { return orderedSet; } }

        public void Clear()
        {
            lock(orderedSet) orderedSet.Clear();
        }

        public void Remove(T obj)
        {
            lock(orderedSet)
                if (orderedSet.Contains(obj))
                    orderedSet.Remove(obj);
        }
    }
}