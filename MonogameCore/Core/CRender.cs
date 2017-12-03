using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class CRender : Component
    {
        protected SpriteBatch batch;
        protected Texture2D sprite;
        public Color colour;
        private Vector2 p = new Vector2(0.2f, 0.2f), q = new Vector2(0.8f,0.8f);
        private Rectangle dest, src;
        private Vector2 temp;

        public CRender(string sprite) : base()
        {
            this.batch = AssetManager.Batch;
            this.sprite = AssetManager.GetResource<Texture2D>(sprite);
            colour = Color.White;
            dest = new Rectangle();
            src = new Rectangle();
        }

        public override void Update(float time)
        {
            if (sprite == null) return;
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
            src.X = (int)(sprite.Width * p.X);
            src.Y = (int)(sprite.Height * p.Y);
            src.Width = (int)(sprite.Width * q.X) - src.X;
            src.Height = (int)(sprite.Height * q.Y) - src.Y;
            batch.Draw(sprite, dest, src, colour);
        }
    }
}