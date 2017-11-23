using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameCore.Test
{
    public class TestMenu : GameState
    {
        private SpriteFont font;
        private Text text;
        private Button button;
        
        public TestMenu() : base() { }

        public override void Load(SpriteBatch batch)
        {
            font = AssetManager.GetResource<SpriteFont>("mainFont");
            text = new Text("Gekste game!", new Vector2(0f, 2f), new Vector2(16f, 1f));
            text.colour = new Color(0, 255, 0);
            button = new Button("Play here!", "block", () => GameStateManager.RequestChange("game", CHANGETYPE.LOAD),
                new Vector2(6, 4), new Vector2(4, 3));
            button.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
            Camera.SetCameraTopLeft(new Vector2(0, 0));
        }
        
        public override void Unload() { }

        public override void Update(float time)
        {
            button.Update();
            base.Update(time);
        }
        
        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            device.Clear(Color.Black);
            batch.Begin();
            {
                text.Draw(batch, font);
                button.Draw(batch, font);
            }
            batch.End();
            base.Draw(time, batch, device);
        }
    }
}