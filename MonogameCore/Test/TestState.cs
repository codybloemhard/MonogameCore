using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameCore.Test
{
    public class TestState : GameState
    {
        private SpriteFont font;
        private Text text;
        private Button button;
        private GameObject stone;

        public TestState() : base() { }

        public override void Load(SpriteBatch batch)
        {
            font = AssetManager.GetResource<SpriteFont>("mainFont");
            text = new Text("text", new Vector2(0, 0), new Vector2(1, 0.5f));
            text.colour = new Color(0, 255, 0);
            button = new Button("press me", "block", () => GameStateManager.RequestChange(new GameStateChange("game", CHANGETYPE.LOAD)),
                new Vector2(14, 0), new Vector2(2, 1));
            button.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);

            stone = new GameObject(manager);
            stone.AddComponent("render", new CRender(stone, "block", batch));
            stone.Pos = new Vector2(1, 1);
            stone.Size = new Vector2(1, 1);
        }
        
        public override void Unload() { }

        public override void Update(float time)
        {
            button.Update();
            Camera.SetCameraTopLeft(new Vector2(0, 0));
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