using System;
using System.Collections.Generic;

namespace Core
{
    public interface _collider
    {
        bool Intersects(_collider o);
    }

    internal class Collision
    {
        private List<GameObject> objects;
        private List<GameObject> staticObjs;

        internal Collision()
        {
            objects = new List<GameObject>();
            staticObjs = new List<GameObject>();
        }

        internal void Check()
        {
            Check(objects, objects);
            Check(staticObjs, objects);
        }

        //Naive N^2 method
        internal void Check(List<GameObject> LA, List<GameObject> LB)
        {
            if (LA.Count == 0) return;
            if (LB.Count == 0) return;
            bool same = LA == LB;
            for (int i = 0; i < LA.Count; i++)
            {
                GameObject A = LA[i];
                if (A == null) continue;
                if (!A.active) continue;
                _collider a = A.Collider;
                if (a == null) continue;
                for (int j = 0; j < LB.Count; j++)
                {
                    if (i == j && same) continue;         
                    GameObject B = LB[j];             
                    if (B == null) continue;
                    if (!B.active) continue;      
                    _collider b = B.Collider;
                    if (b == null) continue;
                    if (a.Intersects(b))
                    {
                        A.OnCollision(B);
                        B.OnCollision(A);
                    }
                }
            }
        }

        internal void Add(GameObject o, bool isStatic = false)
        {
            if (isStatic) staticObjs.Add(o);
            else objects.Add(o);
        }

        internal void Remove(GameObject o, bool isStatic = false)
        {
            if (isStatic) staticObjs.Add(o);
            else objects.Remove(o);
        }

        internal void Clear()
        {
            objects.Clear();
            staticObjs.Clear();
        }
    }
}