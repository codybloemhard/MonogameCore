using System;
using System.Collections.Generic;

namespace Core
{
    public abstract class _tagged { public string tag; }

    public class TagEngine
    {
        public TagEngine() { }

        public T FindWithTag<T>(string tag, List<T> list)
            where T : _tagged
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].tag == tag)
                    return list[i];
            }
            return null;
        }

        public T[] FindAllWithTag<T>(string tag, List<T> list)
            where T : _tagged
        {
            List<T> objs = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].tag == tag)
                    objs.Add(list[i]);
            }
            if (objs.Count == 0) return null;
            T[] arr = objs.ToArray();
            objs.Clear();
            return arr;
        }

        public T[] FindAllWithTags<T>(string[] tags, List<T> list)
            where T : _tagged
        {
            List<T> objs = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < tags.Length; j++)
                {
                    if (list[i].tag == tags[j])
                    {
                        objs.Add(list[i]);
                        break;
                    }
                }
            }
            if (objs.Count == 0) return null;
            T[] arr = objs.ToArray();
            objs.Clear();
            return arr;
        }
    }
}