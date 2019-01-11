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
            for (int i = 0; i < objects.Count; i++)
                objects[i].Update(time);
            for (int i = 0; i < staticObjects.Count; i++)
                staticObjects[i].Update(time);
        }

        internal void UpdateDebugInfo() {
            if (objects != null) Debug.dynamicObjects = objects.Count;
            if (staticObjects != null) Debug.staticObjects = staticObjects.Count;
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

        internal void Clear()
        {
            staticObjects.Clear();
            objects.Clear();
        }

        internal void SetDirty()
        {
            for (int i = 0; i < staticObjects.Count; i++)
                staticObjects[i].DirtySize = true;
            for (int i = 0; i < objects.Count; i++)
                objects[i].DirtySize = true;
        }
        //use tag engine to find objects
        public GameObject FindWithTag(string tag)
        {
            GameObject res;
            res = tags.FindWithTag(tag, staticObjects);
            if (res == null) res = tags.FindWithTag(tag, objects);
            return res;
        }

        public GameObject[] FindAllWithTag(string tag)
        {
            GameObject[] res;
            res = tags.FindAllWithTag(tag, staticObjects);
            if (res == null) res = tags.FindAllWithTag(tag, objects);
            return res;
        }

        public GameObject[] FindAllWithTags(string[] tags)
        {
            GameObject[] res;
            res = this.tags.FindAllWithTags(tags, staticObjects);
            if (res == null) res = this.tags.FindAllWithTags(tags, objects);
            return res;
        }
    }
}