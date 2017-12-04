using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Core
{
    public class Texture
    {
        protected Rectangle bound, final;
        protected Vector2 p, q;
        public Texture2D texture { get; internal set; }
        public Rectangle Final { get { return final; } }

        public Texture(Texture2D texture, Rectangle bound)
        {
            this.texture = texture;
            p = new Vector2();
            q = new Vector2();
            final = new Rectangle();
            this.bound = bound;
            MakeCut(0f, 0f, 1f, 1f);
        }

        public void MakeCut(float pu, float pv, float qu, float qv)
        {
            p.X = pu;
            p.Y = pv;
            q.X = qu;
            q.Y = qv;
            MakeFinal();
        }

        public void MakeCut(Vector2 p, Vector2 q)
        {
            this.p = p;
            this.q = q;
            MakeFinal();
        }

        private void MakeFinal()
        {
            final.X = bound.X + (int)(bound.Width * p.X);
            final.Y = bound.Y + (int)(bound.Height * p.Y);
            final.Width = (int)(bound.Width * q.X) - (int)(bound.Width * p.X);
            final.Height = (int)(bound.Height * q.Y) - (int)(bound.Height * p.Y);
        }
    }

    public class AnimatedTexture : Texture
    {
        private uint framesX, framesY;

        public AnimatedTexture(Texture2D texture, uint framesX, uint framesY
            ,Rectangle bound) : base(texture, bound)
        {
            this.framesX = framesX;
            this.framesY = framesY;
        }

        public void SetFrame(uint frame)
        {
            float w = 1.0f / framesX;
            float h = 1.0f / framesY;
            float x = (float)frame % framesX;
            float y = (float)frame / framesY;
            MakeCut(x, y, w, h);
        }
    }

    public class RawTexture
    {
        public Texture2D texture;
        public bool sheet;
        public string name;
        
        public RawTexture(Texture2D texture, bool sheet, string name)
        {
            this.texture = texture;
            this.sheet = sheet;
            this.name = name;
        }
    }

    internal class RawTexComparer : IComparer<RawTexture>
    {
        public int Compare(RawTexture x, RawTexture y)
        {
            if (x == null) return 1;
            else if (y == null) return -1;
            else if (x == null && y == null) return 0;
            if (x.texture.Width * x.texture.Height >
                y.texture.Width * y.texture.Height)
                return -1;
            else return 1;
        }
    }

    internal class PackingNode
    {
        public PackingNode l, r;
        public bool filled;
        public Rectangle bound;

        public PackingNode(Rectangle bound)
        {
            filled = false;
            this.bound = bound;
            l = null;
            r = null;
        }

        public PackingNode Fill(Rectangle rect)
        {
            if (rect == null) return null;
            if (filled) return null;    
            if (rect.Width > bound.Width) return null;
            if (rect.Height > bound.Height) return null;
            if(rect.Width == bound.Width && rect.Height == bound.Height)
            {
                filled = true;
                return this;
            }
            PackingNode temp = null;
            if (l != null) temp = l.Fill(rect);
            if (temp != null) return temp;
            if (r != null) temp = r.Fill(rect);
            if (temp != null) return temp;
            if (l != null && r != null) return null;
            l = new PackingNode(new Rectangle());
            r = new PackingNode(new Rectangle());
            int diffWidth = rect.Width - rect.Width;
            int diffHeight = rect.Height - rect.Height;
            bool hor = diffWidth >= diffHeight;
            if (bound.Width == rect.Width) hor = false;
            if (bound.Height == rect.Height) hor = true;
            Split(rect, hor);    
            return l.Fill(rect);
        }

        public void Split(Rectangle rect, bool hor)
        { 
            if (hor)
            {
                l.bound = new Rectangle(bound.X, bound.Y, rect.Width, bound.Height);
                r.bound = new Rectangle(bound.X + rect.Width, bound.Y, bound.Width - rect.Width, bound.Height);
            }
            else
            {
                l.bound = new Rectangle(bound.X, bound.Y, bound.Width, rect.Height);
                r.bound = new Rectangle(bound.X, bound.Y + rect.Height, bound.Width, bound.Height - rect.Height);
            }
        }
    }

    public static class TextureManager
    {
        private static Dictionary<string, Texture> textures;
        private static OrderedSet<RawTexture> rawTexs;
        internal static Texture2D atlas;

        static TextureManager()
        {
            textures = new Dictionary<string, Texture>();
            rawTexs = new OrderedSet<RawTexture>(new RawTexComparer());
        }

        public static bool LoadTexture(string name, string file, bool animationsheet)
        {
            if (textures.ContainsKey(name))
            {
                Debug.PrintNotification("Texture name already used: ", name);
                return false;
            }
            Texture2D tex2d = AssetManager.GetResource<Texture2D>(file);
            if (tex2d == null) return false;
            rawTexs.Add(new RawTexture(tex2d, animationsheet, name));
            return true;
        }

        internal static void CalculateTree()
        {
            PackingNode root = new PackingNode(new Rectangle(0, 0, 2048, 2048));
            for (int i = 0; i < rawTexs.Set.Count; i++)
            {
                RawTexture raw = rawTexs.Set[i];
                Rectangle r = new Rectangle(0, 0, raw.texture.Width, raw.texture.Height);
                PackingNode res = root.Fill(r);
                if (res == null) Debug.PrintError("Texture could not be packed: " + raw.name);
                else textures.Add(raw.name, new Texture(raw.texture, res.bound));
            }
        }

        internal static void Bake()
        {
            Color[] data = new Color[2048 * 2048];
            List<Texture> texs = textures.Values.ToList();
            for (int i = 0; i < texs.Count; i++)
            {
                Texture t = texs[i];
                Rectangle r = new Rectangle(0, 0, t.texture.Width, t.texture.Height);
                Color[] local = new Color[r.Width * r.Height];
                t.texture.GetData<Color>(0, r, local, 0, r.Width * r.Height);
                for (int y = 0; y < r.Height; y++)
                    for (int x = 0; x < r.Width; x++)
                        data[To1D(t.Final.X + x, t.Final.Y + y, 2048)] = local[To1D(x, y, r.Width)];
            }
            atlas = AssetManager.GetNewTexture(2048, 2048);
            atlas.SetData<Color>(data);
            for (int i = 0; i < texs.Count; i++)
                texs[i].texture = atlas;
        }
        
        internal static int To1D(int x, int y, int h)
        {
            return y * h + x;
        }

        public static Texture GetTexture(string name)
        {
            if (textures.ContainsKey(name))
                return textures[name];
            Debug.PrintError("Texture not found: " + name);
            return null;
        }
    }
}