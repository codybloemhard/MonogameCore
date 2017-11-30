using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core
{
    public interface _collider
    {
        bool Intersects(_collider o);
        AABB Minmax();
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

    public class Node
    {
        public Node parent;
        public Node[,] childs;
        public List<_collider> colliders;
        public CAABB cbound;

        public Node(Node parent, AABB bound)
        {
            this.parent = parent;
            colliders = new List<_collider>();
            MakeBound(bound);
        }

        public void Add(_collider element)
        {
            if (colliders.Count < 10)
                colliders.Add(element);
            else if(childs == null)
            {
                childs = new Node[2, 2];
                AABB[,] splits = Split();
                for (int i = 0; i < 2; i++)
                    for (int j = 0; j < 2; j++)
                        childs[i, j] = new Node(this, splits[i, j]);
                Choose(element);
                for (int i = 0; i < colliders.Count; i++)
                    Choose(colliders[i]);
            }
            else
                Choose(element);
        }

        public void Draw(LineRenderer lines)
        {
            lines.Add(new Line(cbound.aabb.x, cbound.aabb.y, cbound.aabb.x + cbound.aabb.w, cbound.aabb.y));
            //lines.Add(new Line(cbound.aabb.x, cbound.aabb.y + cbound.aabb.h, cbound.aabb.x + cbound.aabb.w, cbound.aabb.y + cbound.aabb.h));
            lines.Add(new Line(cbound.aabb.x, cbound.aabb.y, cbound.aabb.x, cbound.aabb.y + cbound.aabb.h));
            //lines.Add(new Line(cbound.aabb.x + cbound.aabb.w, cbound.aabb.y, cbound.aabb.x + cbound.aabb.w, cbound.aabb.y + cbound.aabb.h));
            if (childs != null)
                for (int i = 0; i < 2; i++)
                    for (int j = 0; j < 2; j++)
                        childs[i, j].Draw(lines);
        }

        public AABB[,] Split()
        {
            AABB[,] aabbs = new AABB[2, 2];
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                {
                    float w = cbound.aabb.w / 2;
                    float h = cbound.aabb.h / 2;
                    aabbs[i, j] = new AABB(cbound.aabb.x + i * w, cbound.aabb.y + j * h, w, h);
                }
            return aabbs;
        }

        public void MakeBound(AABB nbound)
        {
            if (cbound == null)
                cbound = new CAABB(nbound.x, nbound.y, nbound.w, nbound.h);
            else cbound.aabb = nbound;
        }

        public void Choose(_collider col)
        {
            for(int i = 0; i < 2; i++)
                for(int j = 0; j < 2; j++)
                {
                    if (col.Minmax().Intersects(childs[i, j].cbound.Minmax()))
                        childs[i, j].Add(col);
                }
        }
    }

    public class QuadTree
    {
        private Node root;
        private AABB minmax;

        public QuadTree()
        {
            root = new Node(null, new AABB(0,0,0,0));
            minmax = new AABB(0,0,0,0);
        }

        public void Add(GameObject go)
        {
            if (go == null) return;
            if (!go.active) return;
            _collider col = go.Collider;
            if (col == null) return;
            root.Add(col);
            AABB ominmax = col.Minmax();
            if (ominmax.x < minmax.x)
                minmax.x = ominmax.x;
            if (ominmax.y < minmax.y)
                minmax.y = ominmax.y;
            if (ominmax.x + ominmax.w > minmax.x + minmax.w)
                minmax.w = ominmax.x + ominmax.w - minmax.x;
            if (ominmax.y + ominmax.h > minmax.y + minmax.h)
                minmax.h = ominmax.y + ominmax.h - minmax.y;
            root.MakeBound(minmax);
        }

        public void Clear()
        {
            root.childs = null;
        }

        public void DrawTree(LineRenderer lines)
        {
            root.Draw(lines);
        }
    }

    internal class Collision
    {
        private List<GameObject> objects;
        private List<GameObject> staticObjs;
        private LineRenderer lines;
        private QuadTree tree;

        internal Collision(LineRenderer lines)
        {
            objects = new List<GameObject>();
            staticObjs = new List<GameObject>();
            this.lines = lines;
            tree = new QuadTree();
        }

        internal void Check()
        {
            lines.Clear();
            tree.Clear();
            for (int i = 0; i < objects.Count; i++)
                tree.Add(objects[i]);
            for (int i = 0; i < staticObjs.Count; i++)
                tree.Add(staticObjs[i]);
            tree.DrawTree(lines);
            CheckN2(objects, objects);
            CheckN2(staticObjs, objects);
        }

        public void Check(_collider a, _collider b, GameObject A, GameObject B)
        {
            if (a.Intersects(b))
            {
                A.OnCollision(B);
                B.OnCollision(A);
            }
        }

        bool GetCollider(GameObject x, out _collider col)
        {
            col = null;
            if (x == null) return false;
            if (!x.active) return false;
            col = x.Collider;
            if (col == null) return false;
            return true;
        }

        //Naive N^2 method
        internal void CheckN2(List<GameObject> LA, List<GameObject> LB)
        {
            if (LA.Count == 0) return;
            if (LB.Count == 0) return;
            bool same = LA == LB;
            for (int i = 0; i < LA.Count; i++)
            {
                _collider a;
                if(!GetCollider(LA[i], out a)) continue;
                for (int j = 0; j < LB.Count; j++)
                {
                    if (i == j && same) continue;
                    _collider b;
                    if (!GetCollider(LB[j], out b)) continue;
                    Check(a, b, LA[i], LB[j]);
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
            if (o == null) return;
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