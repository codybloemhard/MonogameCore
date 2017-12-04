using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class CRender : Component
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
            if (texture == null) return;
            base.Update(time);
            if (gameObject.DirtySize)
            {
                temp = Grid.ToScreenSpace(GO.Size);
                dest.Width = (int)temp.X;
                dest.Height = (int)temp.Y;
            }
            temp = Grid.ToScreenSpace(GO.Pos);
            dest.X = (int)temp.X;
            dest.Y = (int)temp.Y;
            batch.Draw(texture.texture, dest, texture.Final, colour);
        }
    }
}