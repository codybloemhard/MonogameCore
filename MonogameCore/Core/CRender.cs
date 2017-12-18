using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public interface _renderer { void Update(float time); }

    public class CRender : Component, _renderer
    {
        protected SpriteBatch batch;
        protected Texture texture;
        public Color colour;
        private Vector2 temp;
        private Rectangle dest;

        public CRender(string name) : base()
        {
            this.batch = AssetManager.Batch;
            texture = TextureManager.GetTexture(name);
            colour = Color.White;
            dest = new Rectangle();
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (texture == null) return;
            if (gameObject.DirtySize)
            {
                temp = Grid.ToScreenSpace(GO.Size);
                dest.Width = (int)temp.X + 1;
                dest.Height = (int)temp.Y + 1;
            }
            temp = Grid.ToScreenSpace(GO.Pos);
            dest.X = (int)temp.X;
            dest.Y = (int)temp.Y;
            batch.Draw(texture.texture, dest, texture.Final, colour);
        }
    }
}