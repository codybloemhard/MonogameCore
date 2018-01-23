using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//Om GameObjects te updaten en te drawen.
namespace Core
{
    public class GameObjectManager
    {
        internal List<GameObject> objects;
        internal List<GameObject> staticObjects;
        private TagEngine tags;

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
            lock (objects)
            {         
                for (int i = 0; i < Size; i++)
                    objects[i].Update(time);
            }
            lock (staticObjects)
            {
                for (int i = 0; i < StaticSize; i++)
                    staticObjects[i].Update(time);
            }
        }

        public void Add(GameObject o, bool isStatic = false)
        {
            if (isStatic) lock(staticObjects) staticObjects.Add(o);
            else lock(objects) objects.Add(o);
        }
        
        public void Destroy(GameObject o, bool isStatic = false)
        {
            if (isStatic) lock(staticObjects) staticObjects.Remove(o);
            else lock(objects) objects.Remove(o);
        }

        internal void Clear()
        {
            lock(staticObjects) staticObjects.Clear();
            lock(objects) objects.Clear();
        }

        internal void SetDirty()
        {
            lock(staticObjects)
                for (int i = 0; i < StaticSize; i++)
                    staticObjects[i].DirtySize = true;
            lock(objects)
                for (int i = 0; i < Size; i++)
                    objects[i].DirtySize = true;
        }
        //use tag engine to find objects
        public GameObject FindWithTag(string tag)
        {
            lock (tags)
            {
                GameObject res;
                lock (staticObjects) res = tags.FindWithTag(tag, staticObjects);
                if (res == null) lock(objects) res = tags.FindWithTag(tag, objects);
                return res;
            }
        }

        public GameObject[] FindAllWithTag(string tag)
        {
            lock (tags)
            {
                GameObject[] res;
                lock(staticObjects) res = tags.FindAllWithTag(tag, staticObjects);
                if (res == null) lock(objects)
                    res = tags.FindAllWithTag(tag, objects);
                return res;
            }
        }

        public GameObject[] FindAllWithTags(string[] tags)
        {
            lock (tags)
            {
                GameObject[] res;
                lock(staticObjects) res = this.tags.FindAllWithTags(tags, staticObjects);
                if (res == null) lock(objects)
                    res = this.tags.FindAllWithTags(tags, objects);
                return res;
            }
        }

        public List<GameObject> StaticObjects { get { return staticObjects; } }
        public List<GameObject> Objects { get { return objects; } }

        public int Size { get { return objects.Count; } }
        public int StaticSize { get { return staticObjects.Count; } }
    }
}