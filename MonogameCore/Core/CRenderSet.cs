using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    //Rendered een set van gelijksoortige objecten
    public class CRenderSet : CRender
    {
        private int[,] set;
        private Color[] colours;
        private Vector2 pos, size;
        private Vector2 blocksize, sizemultiplier;
        private Texture2D texture;

        public CRenderSet(string sprite, SpriteBatch batch)
            : base(sprite, batch) { }

        public void InitSet(int[,] set, string texture, Color[] colours, Vector2 pos, Vector2 size)
        {
            this.colours = colours;
            blocksize = new Vector2(size.X / set.GetLength(0), size.Y / set.GetLength(1));
            this.texture = AssetManager.GetResource<Texture2D>(texture);
            sizemultiplier = blocksize * Grid.ScaleSprite(new Vector2(this.texture.Width, this.texture.Height));
            
            this.set = set;
            this.pos = pos;
            this.size = size;
        }
        public int[,] Set { set { set = value; } }

        public override void Update(float time)
        {
            base.Update(time);
            if (!active) return;
            Render();
        }

        private void Render()
        { 
            if (set == null) return;
            if (texture == null) return;
            for (int x = 0; x < set.GetLength(0); x++)
                for(int y = 0; y < set.GetLength(1); y++)
                {
                    int i = set[x, y];
                    Color c;
                    if (i >= 0 && i < colours.Length) c = colours[i];
                    else c = Color.White;
                    Vector2 cstep = blocksize * new Vector2(x, y);
                    batch.Draw(texture, Grid.ToScreenSpace(pos + (cstep)), null, c, 0.0f, Vector2.Zero, sizemultiplier, SpriteEffects.None, 0.0f);
                }
        }
    }
}