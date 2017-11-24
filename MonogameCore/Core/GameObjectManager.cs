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
        private List<GameObject> objs;//For finding multiple objs.

        public GameObjectManager()
        {
            objects = new List<GameObject>();
            objs = new List<GameObject>();
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
        /*Achterlichende functies for FindWithTag etc.
        Zie GameObject waarom.*/
        public GameObject FindWithTag(string tag)
        {
            for (int i = 0; i < Size; i++)
            {
                if (objects[i].tag == tag)
                    return objects[i];
            }
            return null;
        }

        public GameObject[] FindAllWithTag(string tag)
        {
            for (int i = 0; i < Size; i++)
            {
                if (objects[i].tag == tag)
                    objs.Add(objects[i]);
            }
            if (objs.Count == 0) return null;
            GameObject[] arr = objs.ToArray();
            objs.Clear();
            return arr;
        }

        public GameObject[] FindAllWithTags(string[] tags)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < tags.Length; j++)
                {
                    if (objects[i].tag == tags[j])
                    {
                        objs.Add(objects[i]);
                        break;
                    }
                }
            }
            if (objs.Count == 0) return null;
            GameObject[] arr = objs.ToArray();
            objs.Clear();
            return arr;
        }

        public int Size { get { return objects.Count; } }
    }
}