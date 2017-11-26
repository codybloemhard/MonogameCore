using System;
using System.Collections.Generic;

namespace Core
{
    internal class LayeredRenderer
    {
        private List<GameObject> orderedSet;
        private LayerComparer comparer;

        internal LayeredRenderer()
        {
            orderedSet = new List<GameObject>();
            comparer = new LayerComparer();
        }

        internal void Add(GameObject go)
        {
            uint layer = go.layer;
            int index = orderedSet.BinarySearch(go, comparer);
            if (index < 0)
                orderedSet.Insert(~index, go);
        }

        internal void Render()
        {
            for (int i = orderedSet.Count - 1; i >= 0; i--)
                orderedSet[i].FinishFrame();
            orderedSet.Clear();
        }
    }

    internal class LayerComparer : IComparer<GameObject>
    {
        public int Compare(GameObject a, GameObject b)
        {
            if (a == null && b == null) return 0;
            if (a != null && b == null) return 1;
            if (a == null && b != null) return -1;
            uint la = a.layer;
            uint lb = b.layer;
            if (la > lb) return 1;
            if (lb > la) return -1;
            return 0;
        }
    }
}