using System;
using System.Collections.Generic;
//Een Dictionary waarin je elk type in kan opbergen :)
namespace Core
{
    public interface _item { }

    public class Item<T> : _item
    {
        public T data;
        public Item(T t) { data = t; }
    }

    public class GenericDatabase
    {
        private Dictionary<string, _item> items;

        public GenericDatabase()
        {
            items = new Dictionary<string, _item>();
        }

        public void DeleteItem(string name)
        {
            if (items.ContainsKey(name))
                items.Remove(name);
        }

        public bool GetData<T>(string name, out T result)
        {
            if (items.ContainsKey(name))
            {
                result = (items[name] as Item<T>).data;
                return true;
            }
            result = default(T);
            return false;
        }

        public bool SetData<T>(string name, T data)
        {
            if (items.ContainsKey(name))
            {
                (items[name] as Item<T>).data = data;
                return true;
            }
            items.Add(name, new Item<T>(data));
            return false;
        }
    }
}