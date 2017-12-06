using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class CTileableSprite : Component, _renderer
    {
        protected SpriteBatch batch;
        protected TileableTexture texture;
        public Color colour;
        private Vector2 temp;
        private Rectangle dest;

        public CTileableSprite(string name, float xx, float yy) : base()
        {
            this.batch = AssetManager.Batch;
            texture = TextureManager.GetTileable(name);
            if(texture != null)
                texture.Tile(xx, yy);
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