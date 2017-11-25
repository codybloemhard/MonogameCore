using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameCore.Test
{
    public class TestGame : GameState
    {
        private GameObject stone;

        public TestGame() : base() { }

        public override void Load(SpriteBatch batch)
        {
            stone = new GameObject(this);
            stone.AddComponent("render", new CRender("block", batch));
            stone.Pos = new Vector2(0, 8);
            stone.Size = new Vector2(4, 1);
        }

        public override void Unload()
        {
            
        }

        public override void Update(float time)
        {
            Camera.SetCameraTopLeft(new Vector2(0, 0));
            base.Update(time);
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}