using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class CRender : Component
    {
        protected Vector2 sizemul;
        protected SpriteBatch batch;
        protected Texture2D sprite;
        public Color colour;

        public CRender(string sprite) : base()
        {
            this.batch = AssetManager.batch;
            if(batch == null) { Console.WriteLine("Could not fin SpriteBatch!"); }
            this.sprite = AssetManager.GetResource<Texture2D>(sprite);
            if (this.sprite == null) Console.WriteLine("Could not find Sprite");
            colour = Color.White;
        }

        public override void Update(float time)
        {
            if (sprite == null) return;
            base.Update(time);
            if (gameObject.DirtySize)
                sizemul = gameObject.Size * Grid.ScaleSprite(new Vector2(sprite.Width, sprite.Height));
            batch.Draw(sprite, Grid.ToScreenSpace(gameObject.Pos), null, colour, 0.0f, Vector2.Zero, sizemul, SpriteEffects.None, 0.0f);
        }
    }
}