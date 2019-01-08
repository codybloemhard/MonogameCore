using System;
using System.Collections.Generic;

//DataManager is er om data tussen gamestates te delen.
namespace Core
{
    public static class DataManager
    {
        private static GenericDatabase database;

        static DataManager()
        {
            database = new GenericDatabase();
        }

        public static void DeleteData(string name)
        {
            database.DeleteItem(name);
        }

        public static T GetData<T>(string name)
        {
            T res;
            database.GetData<T>(name, out res);
            return res;
        }

        public static void SetData<T>(string name, T data)
        {
            database.SetData<T>(name, data);
        }
    }
}