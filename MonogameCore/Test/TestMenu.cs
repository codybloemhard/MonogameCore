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
            text = new Text(this, "Gekste game!", new Vector2(0f, 2f), new Vector2(16f, 1f), font);
            text.colour = new Color(0, 255, 0);
            text.tag = "exampleTag";
            button = new Button(this, "Play here!", "block", () => GameStateManager.RequestChange("game", CHANGETYPE.LOAD),
                font, new Vector2(6, 4), new Vector2(4, 3));
            button.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
            Camera.SetCameraTopLeft(new Vector2(0, 0));
        }
        
        public override void Unload() { }

        public override void Update(float time)
        {
            Text text = ui.FindWithTag("exampleTag") as Text;
            text.text = "Gekste game! " + time;
            base.Update(time);
        }
        
        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}