using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core
{
    public interface _collider
    {
        bool Intersects(_collider o);
        AABB Minmax();
        bool IsActive();
        GameObject Parent();
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

    public class Batch
    {
        public List<_collider> colliders;
        public CAABB bound;
        public Batch(List<_collider> colliders, CAABB bound)
        {
            this.colliders = colliders;
            this.bound = bound;
        }
    }
    //recursive node used for the quadtree
    public class Node
    {
        public Node parent;
        public Node[,] childs;
        public List<_collider> colliders;
        public CAABB cbound;
        public uint depth;
        public uint counter;

        public Node(Node parent, AABB bound, uint depth)
        {
            this.parent = parent;
            this.depth = depth;
            colliders = new List<_collider>();
            MakeBound(bound);
            counter = 0;
        }
        //add a collider to this batch and see if it needs splitting
        public void Add(_collider element)
        {
            if (counter < 10 || depth >= 7)
            {
                colliders.Add(element);
                counter++;
            }
            else if (childs == null)
            {
                childs = new Node[2, 2];
                AABB[,] splits = Split();
                for (int i = 0; i < 2; i++)
                    for (int j = 0; j < 2; j++)
                        childs[i, j] = new Node(this, splits[i, j], depth + 1);
                Choose(element);
                for (int i = 0; i < colliders.Count; i++)
                    Choose(colliders[i]);
                colliders.Clear();
            }
            else
                Choose(element);
        }

        public void Draw(LineRenderer lines, Color colour)
        {
            lines.Add(new Line(cbound.aabb.x, cbound.aabb.y, cbound.aabb.x + cbound.aabb.w, cbound.aabb.y, colour));
            lines.Add(new Line(cbound.aabb.x, cbound.aabb.y, cbound.aabb.x, cbound.aabb.y + cbound.aabb.h, colour));
            if (childs != null)
                for (int i = 0; i < 2; i++)
                    for (int j = 0; j < 2; j++)
                        childs[i, j].Draw(lines, colour);
        }
        //split this batch into four childbatches
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
        //see in what child batch it should go
        public void Choose(_collider col)
        {
            for(int i = 0; i < 2; i++)
                for(int j = 0; j < 2; j++)
                {
                    if (col.Minmax().Intersects(childs[i, j].cbound.Minmax()))
                        childs[i, j].Add(col);
                }
        }
        
        public void GetBatches(List<Batch> batches)
        {
            if(childs != null)
            {
                for (int i = 0; i < 2; i++)
                    for (int j = 0; j < 2; j++)
                        childs[i, j].GetBatches(batches);
                return;
            }
            batches.Add(new Batch(colliders, cbound));
        }
        //run objects through the batches down
        public void CheckOther(_collider col)
        {
            if (!cbound.Intersects(col))
            {
                return;
            }
            if(childs == null)
            {
                for(int i = 0; i < colliders.Count; i++)
                    CollisionMath.Check(colliders[i], col);
            }
            else
            {
                for (int i = 0; i < 2; i++)
                    for (int j = 0; j < 2; j++)
                        childs[i, j].CheckOther(col);
            }
        }
    }

    public class QuadTree
    {
        private Node root;
        private AABB minmax;
        private uint counter = 0;
        private List<_collider> cols;

        public QuadTree()
        {
            root = new Node(null, new AABB(0,0,0,0), 0);
            minmax = new AABB(0,0,0,0);
            cols = new List<_collider>();
        }

        public void Add(_collider col)
        {
            if (col == null) return;
            cols.Add(col);
            AABB added = col.Minmax();
            AABB old = new AABB(minmax);
            //find outerbound to cut
            float minx = Math.Min(added.x, old.x);
            float miny = Math.Min(added.y, old.y);
            float maxx = Math.Max(added.x + added.w, old.x + old.w);
            float maxy = Math.Max(added.y + added.h, old.y + old.h);
            minmax.x = minx;
            minmax.y = miny;
            minmax.w = maxx - minx;
            minmax.h = maxy - miny;
            
            root.MakeBound(minmax);
            counter++;
        }

        public void Build()
        {
            root.childs = null;
            for (int i = 0; i < cols.Count; i++)
                root.Add(cols[i]);
        }

        public void Clear()
        {
            cols.Clear();
            root.childs = null;
            counter = 0;
        }

        public List<Batch> GetBatches()
        {
            List<Batch> batches = new List<Batch>();
            root.GetBatches(batches);
            return batches;
        }

        public void CheckSelf()
        {
            List<Batch> batches = GetBatches();
            for (int i = 0; i < batches.Count; i++)
                for (int j = 0; j < batches[i].colliders.Count; j++)
                    CollisionMath.CheckN2(batches[i].colliders, batches[i].colliders);
        }

        public void CheckOther(List<_collider> list)
        {
            for (int i = 0; i < list.Count; i++)
                root.CheckOther(list[i]);
        }

        public void DrawTree(LineRenderer lines, Color colour)
        {
            root.Draw(lines, colour);
        }

        public void PrintMinMax()
        {
            Console.WriteLine(minmax.String());
        }

        public uint Count { get { return counter; } }
    }

    internal class Collision
    {
        private List<_collider> dynamics;
        private List<_collider> statics;
        private LineRenderer lines;
        private QuadTree dynamicTree, staticTree;
        private object lDynamics = new object(), lStatics = new object();

        internal Collision(LineRenderer lines)
        {
            dynamics = new List<_collider>();
            statics = new List<_collider>();
            this.lines = lines;
            dynamicTree = new QuadTree();
            staticTree = new QuadTree();
        }

        internal void Check()
        {
            lock (lDynamics) lock(lStatics)
            {
                lines.Clear();
                dynamicTree.Clear();
                for (int i = 0; i < dynamics.Count; i++)
                    dynamicTree.Add(dynamics[i]);
                dynamicTree.Build();
                CheckQuad();
                dynamicTree.DrawTree(lines, Color.Red);
                staticTree.DrawTree(lines, Color.Blue);
            }
        }
        
        internal void CheckQuad()
        {
            dynamicTree.CheckSelf();//check tree against itself
            //staticTree.CheckOther(dynamics);//has bugs?
            CollisionMath.CheckN2(dynamics, statics);//quick fix
        }

        internal void Add(_collider o, bool isStatic = false)
        {
            lock (lDynamics) lock(lStatics)
            {
                if (o == null) return;
                if (isStatic)
                {
                    statics.Add(o);
                    staticTree.Clear();
                    for (int i = 0; i < statics.Count; i++)
                        staticTree.Add(statics[i]);
                    staticTree.Build();
                    }
                else dynamics.Add(o);
            }         
        }

        internal void Remove(GameObject o, bool isStatic = false)
        {
            lock (lDynamics) lock(lStatics)
            {
                if (o.Collider == null) return;
                if (isStatic)
                {
                    statics.Remove(o.Collider);
                    staticTree.Clear();
                    for (int i = 0; i < statics.Count; i++)
                        staticTree.Add(statics[i]);
                    staticTree.Build();
                }
                else dynamics.Remove(o.Collider);
            }            
        }

        internal void Clear()
        {
            lock (lDynamics) lock(lStatics)
            {
                dynamics.Clear();
                statics.Clear();
            }
        }
        //find objects according to RAYCASTTYPE
        internal RaycastResult Raycast(Vector2 origin, Vector2 direction, RAYCASTTYPE type)
        {
            RaycastResult dynamicRes = new RaycastResult();
            RaycastResult staticRes = new RaycastResult();
            if (type == RAYCASTTYPE.DYNAMIC || type == RAYCASTTYPE.ALL)
                lock(lDynamics) dynamicRes = CollisionMath.Raycast(origin, direction, dynamics);
            if (type == RAYCASTTYPE.STATIC || type == RAYCASTTYPE.ALL)
                lock(lStatics) staticRes = CollisionMath.Raycast(origin, direction, statics);
            if (!dynamicRes.hit) return staticRes;
            if (!staticRes.hit) return dynamicRes;
            if (dynamicRes.distance > staticRes.distance)
                return staticRes;
            return dynamicRes;
        }
    }

    internal static class CollisionMath
    {
        internal static void Check(_collider a, _collider b)
        {
            if (a.Intersects(b))
            {
                a.Parent().OnCollision(b.Parent());
                b.Parent().OnCollision(a.Parent());
            }
        }
        //check all objects in A against all objects in B
        internal static void CheckN2(List<_collider> LA, List<_collider> LB)
        {
            if (LA == null) return;
            if (LB == null) return;
            if (LA.Count == 0) return;
            if (LB.Count == 0) return;
            bool same = LA == LB;
            for (int i = 0; i < LA.Count; i++)
            {
                if (!LA[i].IsActive()) continue;
                for (int j = 0; j < LB.Count; j++)
                {
                    if (i == j && same) continue;
                    if (!LB[j].IsActive()) continue;
                    Check(LA[i], LB[j]);
                }
            }
        }
        //find closest object colliding with the ray.
        internal static RaycastResult Raycast(Vector2 origin, Vector2 direction, List<_collider> list)
        {
            float px = origin.X, py = origin.Y;
            float qx = px + direction.X, qy = py + direction.Y;
            float minDist = float.MaxValue;
            RaycastResult result = new RaycastResult();
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].IsActive()) continue;
                float ix = 0, iy = 0;
                bool hit = MathH.RayBoxIntersection(list[i].Minmax(), px, py, qx, qy, ref ix, ref iy);
                if (!hit) continue;
                Vector2 inter = new Vector2(ix, iy);
                float dist = Math.Abs(Vector2.Distance(origin, inter));
                if (dist < minDist)
                {
                    minDist = dist;
                    result.distance = dist;
                    result.hit = true;
                    result.obj = list[i].Parent();
                    result.intersection = inter;
                }
            }
            return result;
        }
    }
}