using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameCore.Test
{
    public class TestState : GameState
    {
        private SpriteFont bigFont, mainFont;
        private Text titleText, scoreText;
        private Button gotoGame;
        private ParticleEmitter emitter;
        private Random rand;
        private float timer = 0f;

        public TestState() { }

        public void Load(SpriteBatch batch)
        {
            bigFont = AssetManager.GetResource<SpriteFont>("menuFont");
            mainFont = AssetManager.GetResource<SpriteFont>("mainFont");
            titleText = new Text("Faking game boizz", Vector2.Zero, new Vector2(16, 3));
            titleText.colour = Color.Red;
            scoreText = new Text("Highscore: 0", new Vector2(0, 7), new Vector2(16, 1));
            scoreText.colour = new Color(0, 255, 0);
            gotoGame = new Button("Play", "block", () => GameStateManager.RequestChange(new GameStateChange("game", CHANGETYPE.LOAD)),
            new Vector2(6, 3), new Vector2(4, 3));
            gotoGame.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
            emitter = new ParticleEmitter("block", 200);
            rand = new Random();
        }

        public void Unload() { }

        public Color RandomColor()
        {
            Color[] colours = { Color.Cyan, Color.Blue, Color.Orange, Color.Yellow, Color.Green, Color.Purple, Color.Red };
            return colours[rand.Next(0, 6)];
        }

        public void Update(float time)
        {
            scoreText.text = "Highscore: " + Highscores.Highscore;
            gotoGame.Update();
            timer += time;
            if (timer > 0.5f)
            {
                timer = 0f;
                Vector2 pos = new Vector2((float)rand.NextDouble() * 16, (float)rand.NextDouble() * 9);
                for (int j = 0; j < 20; j++)
                    emitter.Emit(pos, Vector2.One / 1.0f, MathH.RandomUnitVector(),
                    RandomColor(), 5.0f, 120, 1.0f, 0.99f, 1);
            }
            emitter.Update(time);
        }

        public void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            device.Clear(Color.Black);
            batch.Begin();
            {
                emitter.Draw(batch);
                titleText.Draw(batch, bigFont);
                scoreText.Draw(batch, mainFont);
                gotoGame.Draw(batch, mainFont);
            }
            batch.End();
        }
    }
}