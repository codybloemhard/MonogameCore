using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
/*Deze deelt pointers uit en laad pas als de asset nooit eerder
is opgevraagd.
*/
namespace Core
{
    public static class AssetManager
    {
        internal static SpriteBatch batch;
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

        public static T GetResource<T>(string name)
        {
            if (content == null) return default(T);
            T res;
            if (database.GetData<T>(name, out res))
                return res;
            try { res = content.Load<T>(name); } 
            catch(Exception){
                return default(T);
            }
            database.SetData<T>(name, res);
            return res;
        }


    }
}