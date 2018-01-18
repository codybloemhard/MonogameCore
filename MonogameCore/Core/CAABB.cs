using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core
{
    public class AABB
    {
        internal float x, y, w, h;

        public AABB(float x, float y, float w, float h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public AABB(AABB orig)
        {
            Copy(orig);
        }

        public bool Intersects(AABB b)
        {
            if (b == null) return false;
            if (x < b.x + b.w && x + w > b.x &&
                y < b.y + b.h && y + h > b.y)
                return true;
            return false;
        }

        public bool Inside(Vector2 p)
        {
            if (p.X > x && p.X < x + w && p.Y > y && p.Y < y + h)
                return true;
            return false;
        }

        public void Copy(AABB orig)
        {
            x = orig.x;
            y = orig.y;
            w = orig.w;
            h = orig.h;
        }

        public string String()
        {
            return x + " , " + y + " , " + w + " , " + h;
        }
    }

    public class CAABB : Component, _collider
    {
        internal AABB aabb;
        private bool sync = false;

        public CAABB(float x, float y, float w, float h) : base()
        {
            sync = false;
            aabb = new AABB(x, y, w, h);
        }

        public CAABB() : base()
        {
            sync = true;
            aabb = new AABB(0, 0, 0, 0);
        }

        public override void Init()
        {
            base.Init();
            aabb = gameObject.GetAABB();
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (sync) aabb = gameObject.GetAABB();
        }
        
        public bool Intersects(_collider o)
        {
            if (o is CAABB)
                return aabb.Intersects((o as CAABB).aabb);
            return false;
        }

        public AABB Minmax()
        {
            return aabb;
        }

        public bool IsActive()
        {
            if (!GO.active) return false;
            return active;
        }

        public GameObject Parent()
        {
            return GO;
        }

        public bool Inside(Vector2 p)
        {
            return aabb.Inside(p);
        }
    }
}