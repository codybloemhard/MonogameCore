using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core
{
    public interface _collider
    {
        bool Intersects(_collider o);
    }

    public enum RAYCASTTYPE { DYNAMIC, STATIC, ALL}

    public class RaycastResult
    {
        public bool hit;
        public GameObject obj;
        public Vector2 intersection;
        public float distance;
        public RaycastResult()
        {
            hit = false;
            obj = null;
            intersection = Vector2.Zero;
            distance = float.MaxValue;
        }
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
        
        public RaycastResult Raycast(Vector2 origin, Vector2 direction, RAYCASTTYPE type)
        {
            RaycastResult dynamicRes = new RaycastResult();
            RaycastResult staticRes = new RaycastResult();
            if(type == RAYCASTTYPE.DYNAMIC || type == RAYCASTTYPE.ALL)
                dynamicRes = Raycast(origin, direction, objects);
            if (type == RAYCASTTYPE.STATIC || type == RAYCASTTYPE.ALL)
                staticRes = Raycast(origin, direction, staticObjs);
            if (!dynamicRes.hit) return staticRes;
            if (!staticRes.hit) return dynamicRes;
            if (dynamicRes.distance > staticRes.distance)
                return staticRes;
            return dynamicRes;
        }

        private RaycastResult Raycast(Vector2 origin, Vector2 direction, List<GameObject> list)
        {
            float px = origin.X, py = origin.Y;
            float qx = px + direction.X, qy = py + direction.Y;
            float minDist = float.MaxValue;
            RaycastResult result = new RaycastResult();
            for(int i = 0; i < list.Count; i++)
            {
                if (!list[i].active) continue;
                _collider a = list[i].Collider;
                if (a == null) continue;
                float ix = 0, iy = 0;
                bool hit = MathH.RayBoxIntersection((a as CAABB).aabb, px, py, qx, qy, ref ix, ref iy);
                if (!hit) continue;
                Vector2 inter = new Vector2(ix, iy);
                float dist = Vector2.Distance(origin, inter);
                if(dist < minDist)
                {
                    minDist = dist;
                    result.distance = dist;
                    result.hit = true;
                    result.obj = list[i];
                    result.intersection = inter;
                }
            }
            return result;
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