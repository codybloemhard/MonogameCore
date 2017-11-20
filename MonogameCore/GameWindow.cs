using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Core;
using MonogameCore.Test;

namespace MonogameCore
{
    public class GameWindow : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch batch;
        private Action load;
        public GameStateManager gamestates;

        public GameWindow()
        {
            graphics = new GraphicsDeviceManager(this);
            AssetManager.content = Content;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        public void SetLoad(Action a)
        {
            load = a;
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            Camera.SetupResolution(1920, 1080, graphics, GraphicsDevice);
            batch = new SpriteBatch(GraphicsDevice);
            gamestates = new GameStateManager(batch);
            load();
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            gamestates.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            gamestates.Draw((float)gameTime.ElapsedGameTime.TotalSeconds, batch, GraphicsDevice);
            base.Draw(gameTime);
        }
    }
}