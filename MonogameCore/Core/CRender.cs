using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class Renderer : Component
    {
        public Color colour;
        protected SpriteBatch batch;
        protected Vector2 temp;
        protected Rectangle dest;
        protected float angle = 0f;
        protected Vector2 origin, offset;

        public Renderer() : base()
        {
            this.batch = AssetManager.Batch;
            colour = Color.White;
            dest = new Rectangle();
            offset = new Vector2();
        }

        public void Rotate(float degrees)
        {
            angle += MathH.DEG_TO_RAD * degrees;
        }

        public void SetRotation(float degrees)
        {
            angle = MathH.DEG_TO_RAD * degrees;
        }

        public float GetRotationDegrees { get { return angle * MathH.RAD_TO_DEG; } }
        public float GetRotationRadians { get { return angle; } }

        public override void Update(float time)
        {
            base.Update(time);
            if (gameObject.DirtySize)
            {
                temp = Grid.ToScreenSpace(GO.Size);
                dest.Width = (int)temp.X + 1;
                dest.Height = (int)temp.Y + 1;
                offset.X = dest.Width / 2;
                offset.Y = dest.Height / 2;
            }
            temp = Grid.ToScreenSpace(GO.Pos);
            dest.X = (int)(temp.X + offset.X);
            dest.Y = (int)(temp.Y + offset.Y);
        }
    }

    public sealed class CRender : Renderer
    {
        private Texture texture;

        public CRender(string name) : base()
        {       
            texture = TextureManager.GetTexture(name);
            origin = texture.Origin();
        }

        public override void Update(float time)
        {
            if (texture == null) return;
            base.Update(time);
            batch.Draw(texture.texture, dest, texture.Final, colour, angle, origin, SpriteEffects.None, 0);
        }
    }
}