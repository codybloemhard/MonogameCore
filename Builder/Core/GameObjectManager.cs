using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//Om GameObjects te updaten en te drawen.
namespace Core
{
    public class GameObjectManager
    {
        private List<GameObject> objects;
        private List<GameObject> staticObjects;
        private TagEngine tags;
        private object lObjects = new object(), 
            lStaticObjects = new object(), 
            lTags = new object();

        internal GameObjectManager()
        {
            objects = new List<GameObject>();
            staticObjects = new List<GameObject>();
            tags = new TagEngine();
        }

        internal void Update(float time)
        {
            if (Grid.dirty > 0)
            {
                Grid.dirty--;
                SetDirty();
            }
            lock (lObjects)
            {         
                for (int i = 0; i < objects.Count; i++)
                    objects[i].Update(time);
            }
            lock (lStaticObjects)
            {
                for (int i = 0; i < staticObjects.Count; i++)
                    staticObjects[i].Update(time);
            }
        }

        internal void UpdateDebugInfo() {
            lock(lObjects) lock (lStaticObjects) {
                if(objects != null) Debug.dynamicObjects = objects.Count;
                if(staticObjects != null) Debug.staticObjects = staticObjects.Count;
            }
        }

        public void Add(GameObject o, bool isStatic = false)
        {
            if (isStatic) lock(lStaticObjects) staticObjects.Add(o);
            else lock(lObjects) objects.Add(o);
        }
        
        public void Destroy(GameObject o, bool isStatic = false)
        {
            if (isStatic) lock(lStaticObjects) staticObjects.Remove(o);
            else lock(lObjects) objects.Remove(o);
        }

        internal void Clear()
        {
            lock(lStaticObjects) staticObjects.Clear();
            lock (lObjects) objects.Clear();
        }

        internal void SetDirty()
        {
            lock(lStaticObjects)
                for (int i = 0; i < staticObjects.Count; i++)
                    staticObjects[i].DirtySize = true;
            lock(lObjects)
                for (int i = 0; i < objects.Count; i++)
                    objects[i].DirtySize = true;
        }
        //use tag engine to find objects
        public GameObject FindWithTag(string tag)
        {
            lock (lTags)
            {
                GameObject res;
                lock (lStaticObjects) res = tags.FindWithTag(tag, staticObjects);
                if (res == null) lock(lObjects) res = tags.FindWithTag(tag, objects);
                return res;
            }
        }

        public GameObject[] FindAllWithTag(string tag)
        {
            lock (lTags)
            {
                GameObject[] res;
                lock(lStaticObjects) res = tags.FindAllWithTag(tag, staticObjects);
                if (res == null) lock(lObjects) res = tags.FindAllWithTag(tag, objects);
                return res;
            }
        }

        public GameObject[] FindAllWithTags(string[] tags)
        {
            lock (lTags)
            {
                GameObject[] res;
                lock(lStaticObjects) res = this.tags.FindAllWithTags(tags, staticObjects);
                if (res == null) lock(lObjects) res = this.tags.FindAllWithTags(tags, objects);
                return res;
            }
        }
    }
}