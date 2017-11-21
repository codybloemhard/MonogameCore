using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//spreekt voorzich.
namespace Core
{
    public class Particle
    {
        public Vector2 pos, size, dir;
        public Color colour;
        public float transparency, speed, derivSize, derivAlpha;
        public uint lives;

        public Particle() {  }

        public void Update(float deltaTime)
        {
            if (lives == 0) return;
            lives--;
            pos += dir * speed * deltaTime;
            size *= derivSize;
            if (size.X < 0 || size.Y < 0) size = Vector2.Zero;
            transparency = EditColourChannel(transparency);
        }

        float EditColourChannel(float b)
        {
            float c = b * derivAlpha;
            if (c > 255) c = 255;
            if (c < 0) c = 0;
            return (float)c;
        }
    }

    public class ParticleEmitter
    {
        private int size, index;
        private Particle[] particles;
        private Vector2 ppos, psize, pdir;
        private Color pcolour;
        private float pspeed, pderivAlpha, pderivSize;
        private uint plives;
        private Texture2D texture;

        public ParticleEmitter(string texture, int size)
        {
            this.size = size;
            index = 0;
            this.texture = AssetManager.GetResource<Texture2D>(texture);
            particles = new Particle[size];
            for (int i = 0; i < size; i++)
                particles[i] = new Particle();
        }

        public void Emit(Vector2 pos, Vector2 size, Vector2 dir, Color colour, float speed, uint lives, float derivAlpha, float derivSize, int rate)
        {
            psize = size;
            dir.Normalize();
            pdir = dir;
            pcolour = new Color(colour.R, colour.G, colour.B, colour.A);
            pspeed = speed;
            plives = lives;
            pderivAlpha = derivAlpha;
            pderivSize = derivSize;

            ppos = pos;
            if (index + rate > this.size)
                index = 0;
            for (int i = 0; i < rate; i++)
                Spawn(particles[index + i]);
            index += rate;
        }
        
        public void Update(float deltaTime)
        {
            for (int i = 0; i < size; i++)
                particles[i].Update(deltaTime);
        }

        private void Spawn(Particle p)
        {
            p.pos = ppos;
            p.dir = pdir;
            p.size = psize;
            p.speed = pspeed;
            p.lives = plives;
            p.colour = pcolour;
            p.derivAlpha = pderivAlpha;
            p.derivSize = pderivSize;
            p.transparency = 1.0f;
        }

        public void Draw(SpriteBatch batch)
        {
            Vector2 sizemul = Grid.ScaleSprite(new Vector2(texture.Width, texture.Height));
            for (int i = 0; i < size; i++)
            {
                if (particles[i].lives <= 0) continue;
                batch.Draw(texture, Grid.ToScreenSpace(particles[i].pos), null, particles[i].colour * particles[i].transparency, 0.0f, Vector2.Zero, particles[i].size * sizemul, SpriteEffects.None, 0.0f);
            }
        }

        public int Size
        {
            get { return size; }
        }
    }
}