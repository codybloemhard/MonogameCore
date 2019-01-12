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

    internal class UniformGrid {
        private Dictionary<Tuple<int, int>, List<_collider>> grid;
        private int unitSize;

        public UniformGrid(int unitSize) {
            this.unitSize = unitSize;
            grid = new Dictionary<Tuple<int, int>, List<_collider>>();
        }

        public void CheckSelf() {
            foreach(var unit in grid.Values)
                CollisionMath.CheckN2(unit, unit);
        }

        public void CheckAgainst(UniformGrid other) {
            foreach (var key in grid.Keys) {
                if (!other.grid.ContainsKey(key)) continue;
                CollisionMath.CheckN2(grid[key], other.grid[key]);
            }
        }

        public void Add(_collider col) {
            AABB mm = col.Minmax();
            for (int x = (int)(mm.x / unitSize); x <= Math.Ceiling(mm.x + mm.w); x++)
                for (int y = (int)(mm.y / unitSize); y <= Math.Ceiling(mm.y + mm.h); y++) {
                    var pos = new Tuple<int, int>(x, y);
                    if (grid.ContainsKey(pos))
                        grid[pos].Add(col);
                    else
                        grid.Add(pos, new List<_collider>() { col });
                }
        }

        public void Remove(_collider col) {
            AABB mm = col.Minmax();
            for (int x = (int)(mm.x / unitSize); x <= Math.Ceiling(mm.x + mm.w); x++)
                for (int y = (int)(mm.y / unitSize); y <= Math.Ceiling(mm.y + mm.h); y++) {
                    var pos = new Tuple<int, int>(x, y);
                    if (grid.ContainsKey(pos))
                        grid[pos].Remove(col);
                }
        }

        public void CleanSoft() {
            foreach (var unit in grid.Values)
                unit.Clear();
        }

        public void CleanHard() {
            foreach (var unit in grid.Values)
                unit.Clear();
            grid.Clear();
        }

        public void ChangeUnitSize(int size, List<_collider> list) {
            unitSize = size;
            CleanHard();
            for (int i = 0; i < list.Count; i++)
                Add(list[i]);
        }

        public void ChangeUnitSize(int size) {
            var list = new List<_collider>();
            foreach (var l in grid.Values)
                for (int i = 0; i < l.Count; i++)
                    list.Add(l[i]);
            ChangeUnitSize(size, list);
        }

        public void DebugRender(LineRenderer lr, Color c) {
            foreach(var key in grid.Keys) {
                lr.Add(new Line(key.Item1, key.Item2, key.Item1 + unitSize, key.Item2, c));
                lr.Add(new Line(key.Item1, key.Item2, key.Item1, key.Item2 + unitSize, c));
                lr.Add(new Line(key.Item1 + unitSize, key.Item2 + unitSize, key.Item1, key.Item2 + unitSize, c));
                lr.Add(new Line(key.Item1 + unitSize, key.Item2 + unitSize, key.Item1 + unitSize, key.Item2, c));
            }
        }
    }

    internal class Collision
    {
        private List<_collider> dynamics;
        private List<_collider> statics;
        private UniformGrid gridDynamic, gridStatic;
        private LineRenderer lines;

        internal Collision(LineRenderer lines)
        {
            dynamics = new List<_collider>();
            statics = new List<_collider>();
            this.lines = lines;
            gridDynamic = new UniformGrid(64);
            gridStatic = new UniformGrid(64);
        }

        internal void Check()
        {
            //CollisionMath.CheckN2(dynamics, dynamics);
            //CollisionMath.CheckN2(dynamics, statics);
            if (Debug.Mode == DEBUGMODE.DEBUG) {
                lines.Clear();
                gridDynamic.DebugRender(lines, Color.Red);
            }
            gridDynamic.CleanSoft();
            for (int i = 0; i < dynamics.Count; i++)
                gridDynamic.Add(dynamics[i]);
            gridDynamic.CheckSelf();
            gridDynamic.CheckAgainst(gridStatic);
        }

        internal void Add(_collider o, bool isStatic = false)
        {
            if (o == null) return;
            if (isStatic) {
                statics.Add(o);
                gridStatic.Add(o);
            }
            else {
                dynamics.Add(o);
                gridDynamic.Add(o);
            }
        }

        internal void Remove(GameObject o, bool isStatic = false)
        {
            if (o.Collider == null) return;
            if (isStatic) {
                statics.Remove(o.Collider);
                gridStatic.Remove(o.Collider);
            }
            else {
                dynamics.Remove(o.Collider);
                gridDynamic.Remove(o.Collider);
            }
        }

        internal void Clear()
        {
            dynamics.Clear();
            statics.Clear();
            gridDynamic.CleanSoft();
            gridStatic.CleanSoft();
        }
        //find objects according to RAYCASTTYPE
        internal RaycastResult Raycast(Vector2 origin, Vector2 direction, RAYCASTTYPE type)
        {
            RaycastResult dynamicRes = new RaycastResult();
            RaycastResult staticRes = new RaycastResult();
            if (type == RAYCASTTYPE.DYNAMIC || type == RAYCASTTYPE.ALL)
                dynamicRes = CollisionMath.Raycast(origin, direction, dynamics);
            if (type == RAYCASTTYPE.STATIC || type == RAYCASTTYPE.ALL)
                staticRes = CollisionMath.Raycast(origin, direction, statics);
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