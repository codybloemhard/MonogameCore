using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public sealed class CAnimatedSprite : Renderer
    {
        private Dictionary<string, AnimatedTexture> textures;
        private AnimatedTexture current;
        private float speed = 0f;
        private float frame = 0;
        private string currentName;

        public CAnimatedSprite() : base()
        {
            textures = new Dictionary<string, AnimatedTexture>();
            current = null;
            currentName = "";
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
            currentName = name;
            speed = rate;
            frame = 0;
            origin = current.Origin();
        }

        public void PlayAnimationIfDifferent(string name, float rate)
        {
            if (name == currentName) return;
            if (!textures.ContainsKey(name))
            {
                Debug.PrintError("Could not find animation: ", name);
                return;
            }
            current = textures[name];
            speed = rate;
            frame = 0;
            origin = current.Origin();
        }

        public override void Update(float time)
        {
            if (current == null) return;
            base.Update(time);
            frame += speed * time;
            if (frame >= current.framesX * current.framesY)
                frame = 0;
            if (frame < 0)
                frame = (current.framesX * current.framesY) - 1;
            current.SetFrame((uint)frame);
            origin = current.Origin();
            batch.Draw(current.texture, dest, current.Final, colour, angle, origin, SpriteEffects.None, 0);
        }
    }
}