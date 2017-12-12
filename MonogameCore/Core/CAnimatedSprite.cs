using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class CAnimatedSprite : Component, _renderer
    {
        private SpriteBatch batch;
        private Dictionary<string, AnimatedTexture> textures;
        private AnimatedTexture current;
        private Vector2 temp;
        private Rectangle dest;
        public Color colour;
        private float speed = 0f;
        private float frame = 0;

        public CAnimatedSprite() : base()
        {
            batch = AssetManager.Batch;
            colour = Color.White;
            dest = new Rectangle();
            textures = new Dictionary<string, AnimatedTexture>();
            current = null;
        }

        public void AddAnimation(string name, string texture)
        {
            AnimatedTexture tex = TextureManager.GetAnimation(texture);
            if(tex == null)
            {
                Debug.PrintError("Could not find texture: ", texture);
                return;
            }
            textures.Add(name, tex as AnimatedTexture);
        }

        public void PlayAnimation(string name, float rate)
        {
            if (!textures.ContainsKey(name))
            {
                Debug.PrintError("Could not find animation: ", name);
                return;
            }
            current = textures[name];
            speed = rate;
            frame = 0;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (current == null) return;
            if (gameObject.DirtySize)
            {
                temp = Grid.ToScreenSpace(GO.Size);
                dest.Width = (int)temp.X;
                dest.Height = (int)temp.Y;
            }
            temp = Grid.ToScreenSpace(GO.Pos);
            dest.X = (int)temp.X;
            dest.Y = (int)temp.Y;
            frame += speed * time;
            if (frame >= current.framesX * current.framesY)
                frame = 0;
            if (frame < 0)
                frame = (current.framesX * current.framesY) - 1;
            current.SetFrame((uint)frame);
            batch.Draw(current.texture, dest, current.Final, colour);
        }
    }
}