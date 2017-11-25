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
        private TagEngine tags;

        public GameObjectManager()
        {
            objects = new List<GameObject>();
            tags = new TagEngine();
        }

        public void Init()
        {
            for (int i = 0; i < Size; i++)
                objects[i].Init();
        }

        public void Update(float time)
        {
            for (int i = 0; i < Size; i++)
                objects[i].Update(time);
        }

        public void Draw()
        {
            for (int i = 0; i < Size; i++)
                objects[i].FinishFrame();
        }

        public void Add(GameObject o)
        {
            objects.Add(o);
        }
        
        public void Destroy(GameObject o)
        {
            objects.Remove(o);
        }

        public void Remove(GameObject o)
        {
            objects.Remove(o);
        }

        public void Clear()
        {
            objects.Clear();
        }

        public GameObject FindWithTag(string tag)
        {
            return tags.FindWithTag(tag, objects);
        }

        public GameObject[] FindAllWithTag(string tag)
        {
            return tags.FindAllWithTag(tag, objects);
        }

        public GameObject[] FindAllWithTags(string[] tags)
        {
            return this.tags.FindAllWithTags(tags, objects);
        }

        public int Size { get { return objects.Count; } }
    }
}