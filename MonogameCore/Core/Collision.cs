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

        internal Collision()
        {
            objects = new List<GameObject>();
        }
        //Naive N^2 method
        internal void Check()
        {
            if (objects.Count == 0) return;
            for (int i = 0; i < objects.Count; i++)
            {
                GameObject A = objects[i];
                if (A == null) continue;
                if (!A.active) continue;
                _collider a = A.Collider;
                for (int j = 0; j < objects.Count; j++)
                {
                    if (i == j) continue;         
                    GameObject B = objects[j];             
                    if (B == null) continue;
                    if (!B.active) continue;      
                    _collider b = B.Collider;
                    if (a.Intersects(b))
                    {
                        A.OnCollision(B);
                        B.OnCollision(A);
                    }
                }
            }
        }

        internal void Add(GameObject o)
        {
            objects.Add(o);
        }

        internal void Remove(GameObject o)
        {
            objects.Remove(o);
        }

        internal void Clear()
        {
            objects.Clear();
        }
    }
}