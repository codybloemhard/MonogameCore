using System;
using System.Collections.Generic;

namespace Core
{
    internal class LayeredRenderer
    {
        private OrderedSet<GameObject> set;
        private LayerComparer comparer;
        private object locker = new object();

        internal LayeredRenderer()
        {
            comparer = new LayerComparer();
            set = new OrderedSet<GameObject>(comparer);
        }

        internal void Add(GameObject go)
        {
            set.Add(go);
        }

        internal void Remove(GameObject go)
        {
            set.Remove(go);
        }
        
        internal void Clear()
        {
            set.Clear();
        }

        internal void Render()
        {
            for (int i = set.Set.Count - 1; i >= 0; i--)
                set.Set[i].FinishFrame();
        }
    }

    internal class LayerComparer : IComparer<GameObject>
    {
        public int Compare(GameObject a, GameObject b)
        {
            if (a == null && b == null) return 0;
            if (a != null && b == null) return 1;
            if (a == null && b != null) return -1;
            uint la = a.Layer;
            uint lb = b.Layer;
            if (la > lb) return 1;
            if (lb > la) return -1;
            return 0;
        }
    }
}