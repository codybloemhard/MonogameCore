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

        internal void Init()
        {
            for (int i = 0; i < StaticSize; i++)
                staticObjects[i].Init();
            for (int i = 0; i < Size; i++)
                objects[i].Init();
        }

        internal void Update(float time)
        {
            for (int i = 0; i < StaticSize; i++)
                staticObjects[i].Update(time);
            for (int i = 0; i < Size; i++)
                objects[i].Update(time);
        }

        internal void SendToDraw(LayeredRenderer renderer)
        {
            for (int i = 0; i < StaticSize; i++)
                renderer.Add(staticObjects[i]);
            for (int i = 0; i < Size; i++)
                renderer.Add(objects[i]);
        }

        public void Add(GameObject o, bool isStatic = false)
        {
            if (isStatic) staticObjects.Add(o);
            else objects.Add(o);
        }
        
        public void Destroy(GameObject o, bool isStatic = false)
        {
            if (isStatic) staticObjects.Remove(o);
            else objects.Remove(o);
        }
        
        public void Remove(GameObject o, bool isStatic = false)
        {
            if (isStatic) staticObjects.Remove(o);
            else objects.Remove(o);
        }

        internal void Clear()
        {
            staticObjects.Clear();
            objects.Clear();
        }

        public GameObject FindWithTag(string tag)
        {
            GameObject res = tags.FindWithTag(tag, staticObjects);
            if(res == null)
                res = tags.FindWithTag(tag, objects);
            return res;
        }

        public GameObject[] FindAllWithTag(string tag)
        {
            GameObject[] res = tags.FindAllWithTag(tag, staticObjects);
            if (res == null)
                res = tags.FindAllWithTag(tag, objects);
            return res;
        }

        public GameObject[] FindAllWithTags(string[] tags)
        {
            GameObject[] res = this.tags.FindAllWithTags(tags, staticObjects);
            if (res == null)
                res = this.tags.FindAllWithTags(tags, objects);
            return res;
        }

        public int Size { get { return objects.Count; } }
        public int StaticSize { get { return staticObjects.Count; } }
    }
}