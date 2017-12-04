using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Core
{
    public static class AssetManager
    {
        private static SpriteBatch batch;
        private static GenericDatabase database;
        public static ContentManager content;
        internal static Texture2D placeholder;
        internal static GraphicsDevice device;

        static AssetManager()
        {
            database = new GenericDatabase();
        }

        internal static void LoadPlaceholder()
        {
            placeholder = new Texture2D(device, 1, 1);
            placeholder.SetData<Color>(new Color[] { Color.White });
        }

        internal static Texture2D GetNewTexture(uint w, uint h)
        {
            return new Texture2D(device, (int)w, (int)h);
        }

        public static T GetResource<T>(string name)
        {
            if (content == null) return default(T);
            T res;
            if (database.GetData<T>(name, out res))
                return res;
            try { res = content.Load<T>(name); } 
            catch(Exception){
                Debug.PrintError("Asset \"" + name + "\" is not found!");
                return default(T);
            }
            database.SetData<T>(name, res);
            Debug.PrintNotification("Asset loaded: ", "\"" + name + "\"");
            return res;
        }

        internal static SpriteBatch Batch
        {
            get
            {
                if (batch == null) Debug.PrintError("Batch is null!");
                return batch;
            }
            set
            {
                batch = value;
            }
        }
    }
}