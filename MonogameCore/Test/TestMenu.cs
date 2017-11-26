using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameCore.Test
{
    public class TestMenu : GameState
    {
        private Text text, testTimer;
        private Button button;

        public TestMenu() : base() { }

        public override void Load(SpriteBatch batch)
        {
            SpriteFont font = AssetManager.GetResource<SpriteFont>("mainFont");
            text = new Text(this, "Gekste game!", new Vector2(0f, 2f), new Vector2(16f, 1f), font);
            text.colour = new Color(0, 255, 0);
            text.tag = "exampleTag";
            testTimer = new Text(this, "time: ", new Vector2(0, 8), new Vector2(16f, 1f), font);
            testTimer.colour = new Color(0, 255, 0);
            testTimer.tag = "timerText";
            button = new Button(this, "Play here!", "block", () => GameStateManager.RequestChange("game", CHANGETYPE.LOAD),
                font, new Vector2(6, 4), new Vector2(4, 3));
            button.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
            //others
            Timers.Add("timer", 10, changeTextColour);
            Camera.SetCameraTopLeft(new Vector2(0, 0));
        }

        private void changeTextColour()
        {
            ui.FindWithTag("timerText").colour = new Color(255, 0, 0);
        }

        public override void Unload()
        {
            Timers.FindWithTag("timer").Reset();
        }

        public override void Update(float time)
        {
            string msg = "Time: " + MathH.Float(Timers.FindWithTag("timer").TimeLeft, 2);
            (ui.FindWithTag("timerText") as Text).text = msg;
            base.Update(time);
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}