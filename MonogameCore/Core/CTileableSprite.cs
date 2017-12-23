using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public sealed class CTileableSprite : Renderer
    {
        private TileableTexture texture;

        public CTileableSprite(string name, float xx, float yy) : base()
        {
            texture = TextureManager.GetTileable(name);
            if (texture != null)
            {
                texture.Tile(xx, yy);
                origin = texture.Origin();
            }
        }

        public override void Update(float time)
        {
            if (texture == null) return;
            base.Update(time);
            batch.Draw(texture.texture, dest, texture.Final, colour, angle, origin, SpriteEffects.None, 0);
        }
    }
}