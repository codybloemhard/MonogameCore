using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public sealed class GameObject : _tagged
    {
        private GameState context;
        private bool dirtybounds = true, dirtyscale = true;
        private Vector2 localpos, pos, localsize, size;
        private List<GameObject> childs;
        private Dictionary<string, Component> components;
        private Component[] comparray;//for fast iteration
        private Renderer renderer;
        private float gtime;
        private GameObject parent;
        private AABB bounds;
        private List<GameObject> done;
        private _collider collider;
        private bool isStatic;
        private uint layer = 0;
        public bool active = true;

        public GameObject(GameState context, uint layer = 0, bool isStatic = false)
        {
            this.isStatic = isStatic;
            this.context = context;
            this.layer = layer;
            context.objects.Add(this, isStatic);
            context.renderer.Add(this);
            construct();
        }

        public GameObject(string tag, GameState context, uint layer = 0, bool isStatic = false)
        {
            this.isStatic = isStatic;
            this.context = context;
            this.layer = layer;
            context.objects.Add(this, isStatic);
            context.renderer.Add(this);
            construct();
            this.tag = tag;
        }

        private void construct()
        {
            bounds = new AABB(0, 0, 0, 0);
            dirtybounds = true;
            dirtyscale = true;
            childs = new List<GameObject>();
            components = new Dictionary<string, Component>();
            comparray = new Component[0];
            done = new List<GameObject>();
        }

        public void Update(float gameTime)
        {
            if (!active) return;
            gtime = gameTime;
            Component col = (collider as Component);
            if (col != null)
                col.Update(gameTime);
            if(comparray != null)
                for (int i = 0; i < comparray.Length; i++)
                    comparray[i].Update(gameTime);
            if (isStatic) return;
            if (parent != null)
            {
                Pos = parent.Pos + localpos;
                Size = parent.Size * localsize;
            }
        }

        public void FinishFrame()
        {
            if (!active) return;
            if (renderer != null)
                renderer.Update(gtime);
            dirtyscale = false;
            done.Clear();
        }
        //This will get called when it collides with something
        public void OnCollision(GameObject other)
        {
            if (!active) return;
            if (done.Contains(other)) return;
            done.Add(other);
            for (int i = 0; i < comparray.Length; i++)
                comparray[i].OnCollision(other);
        }
        public bool DirtyBounds { get { return dirtybounds; } }
        public bool DirtySize { get { return dirtyscale; } internal set { dirtyscale = value; } }
        //GetBounds creates a rectangle that matches the dimensions of the drawn sprite
        /*System with a diryflag ensures we do not have to calculate new bounds
        Everytime we either check for collision or updadate our position.*/
        public AABB GetAABB()
        {
            if (!dirtybounds) return bounds;
            bounds.x = pos.X;
            bounds.y = pos.Y;
            bounds.w = size.X;
            bounds.h = size.Y;
            dirtybounds = false;
            return bounds;
        }
        /*Find with Tag functies zoals in Unity3D, zodat
        we niet harde links hoeven te leggen tussen objecten.
        Het zou een zooi worden als we straks een grotere game maken
        en objecten onderling zouden linken in de main class.
        */
        public GameObject FindWithTag(string tag)
        {
            return context.objects.FindWithTag(tag);
        }

        public GameObject[] FindAllWithTag(string tag)
        {
            return context.objects.FindAllWithTag(tag);
        }

        public GameObject[] FindAllWithTags(string[] tags)
        {
            return context.objects.FindAllWithTags(tags);
        }

        //this method uses the rectangle created in GetBounds to check if two sprites collide
        public bool Collides(GameObject e)
        {
            if (GetAABB().Intersects(e.GetAABB())) return true;
            return false;
        }
        //methods om components te beheren
        public bool HasComponent(string name)
        {
            return components.ContainsKey(name);
        }
        public T GetComponent<T>() where T : class
        {
            for (int i = 0; i < comparray.Length; i++)
            {
                if (comparray[i] is T)
                    return comparray[i] as T;
            }
            return default(T);
        }
        public T GetComponent<T>(string name) where T : class
        {
            if(components.ContainsKey(name))
                return components[name] as T;
            return default(T);
        }
        public void AddComponent(Component com, string name = "")
        {
            com.gameObject = this;
            com.Init();
            if (com is Renderer)
                renderer = com as Renderer;
            else if (com is _collider)
            {
                collider = com as _collider;
                context.collision.Add(collider, isStatic);
            }
            else
            {
                if (name == "")
                    name = MathH.random.NextDouble().ToString();
                components.Add(name, com);
                comparray = new Component[components.Count];
                components.Values.CopyTo(comparray, 0);
            }
        }
        public int ComponentCount { get { return components.Count; }  }
        //methods om parent-childs relaties te beheren
        public GameObject Parent { get { return parent; } }
        public GameObject[] Childeren { get { return childs.ToArray(); } }
        public GameObject GetChild(int i)
        {
            if (i < 0 || i > childs.Count - 1)
                return null;
            return childs[i];
        }
        internal void AddChild(GameObject obj)
        {
            childs.Add(obj);
        }
        internal void RemoveChild(GameObject obj)
        {
            childs.Remove(obj);
        }
        public void SetParent(GameObject obj)
        {
            parent = obj;
            parent.childs.Add(this);
        }
        public void DeChild()
        {
            parent.RemoveChild(this);
            parent = null;
        }
        public void Destroy()
        {
            for (int i = 0; i < childs.Count; i++)
                childs[i].Destroy();
            childs.Clear();
            context.objects.Destroy(this, isStatic);
            context.collision.Remove(this, isStatic);
            context.renderer.Remove(this);
        }
        //locale en wereld coordinaten
        public Vector2 Pos
        {
            get { return pos; }
            set
            {
                pos = value;
                dirtybounds = true;
            }
        }
        public Vector2 LocalPos
        {
            get
            {
                if (parent == null) return Vector2.Zero;
                return localpos;
            }
            set
            {
                if (parent == null) return;
                localpos = value;
                Pos = parent.pos + localpos;
                dirtybounds = true;
            }
        }
        public Vector2 Size
        {
            get { return size; }
            set { size = value; dirtybounds = true; dirtyscale = true; }
        }

        public Vector2 LocalSize
        {
            get { return size; }
            set
            {
                localsize = value;
                size = parent.Size * localsize;
                dirtybounds = true;
                dirtyscale = true;
            }
        }

        public RaycastResult Raycast(Vector2 origin, Vector2 direction, RAYCASTTYPE type)
        {
            return context.collision.Raycast(origin, direction, type);
        }

        public GameObjectManager Manager { get { return context.objects; } }
        public Renderer Renderer { get { return renderer;  } }
        public _collider Collider { get { return collider; } }
        public bool IsStatic { get { return isStatic; } }
        public uint Layer { get { return layer; } }
        public GameState Context { get { return context; } }
    }
}