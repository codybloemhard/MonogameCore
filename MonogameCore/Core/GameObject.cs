using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class Bounds
    {
        public float x, y, w, h;

        public Bounds(float x, float y, float w, float h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public bool Intersects(Bounds b)
        {
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
    }
    
    public sealed class GameObject : _tagged
    {
        private bool dirtybounds = true, dirtyscale = true;
        private Vector2 localpos, pos, localsize, size;
        private List<GameObject> childs;
        private Dictionary<string, Component> components;
        private Component[] comparray;//for fast iteration
        private CRender renderer;
        private float gtime;
        protected GameObject parent;
        protected GameObjectManager manager;
        protected Bounds bounds;
        public bool active = true;

        public GameObject(GameState context)
        {
            this.manager = context.objects;
            manager.Add(this);
            construct();
        }
        public GameObject(string tag, GameState context)
        {
            this.manager = context.objects;
            manager.Add(this);
            construct();
            this.tag = tag;
        }
        public GameObject(GameObjectManager manager)
        {
            this.manager = manager;
            manager.Add(this);
            construct();
        }
        public GameObject(string tag, GameObjectManager manager)
        {
            this.manager = manager;
            manager.Add(this);
            construct();
            this.tag = tag;
        }
        private void construct()
        {
            bounds = new Bounds(0, 0, 0, 0);
            dirtybounds = true;
            dirtyscale = true;
            childs = new List<GameObject>();
            components = new Dictionary<string, Component>();
            comparray = new Component[0];
        }

        public void Init()
        {
            for (int i = 0; i < comparray.Length; i++)
                comparray[i].Init();
        }
        public void Update(float gameTime)
        {
            if (!active) return;
            gtime = gameTime;
            for (int i = 0; i < comparray.Length; i++)
                comparray[i].Update(gameTime);
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
            dirtybounds = false;
            dirtyscale = false;
        }
        public bool DirtyBounds { get { return dirtybounds; } }
        public bool DirtySize { get { return dirtyscale; } }
        //GetBounds creates a rectangle that matches the dimensions of the drawn sprite
        /*System with a diryflag ensures we do not have to calculate new bounds
        Everytime we either check for collision or updadate our position.*/
        public Bounds GetBounds()
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
            return manager.FindWithTag(tag);
        }

        public GameObject[] FindAllWithTag(string tag)
        {
            return manager.FindAllWithTag(tag);
        }

        public GameObject[] FindAllWithTags(string[] tags)
        {
            return manager.FindAllWithTags(tags);
        }

        //this method uses the rectangle created in GetBounds to check if two sprites collide
        public bool Collides(GameObject e)
        {
            if (GetBounds().Intersects(e.GetBounds())) return true;
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
        public void AddComponent(string name, Component com)
        {
            com.gameObject = this;
            if (com is CRender)
                renderer = com as CRender;
            else
            {
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
        public void AddChild(GameObject obj)
        {
            childs.Add(obj);
        }
        public void RemoveChild(GameObject obj)
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
            manager.Destroy(this);
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
            get { return localpos; }
            set
            {
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

        public GameObjectManager Manager { get { return manager; } }
        public CRender Renderer { get { return renderer;  } }
    }
}