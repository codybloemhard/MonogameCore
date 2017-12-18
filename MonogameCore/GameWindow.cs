using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Core;
using System.Runtime.InteropServices;

namespace Core
{
    public sealed class GameWindow : Game
    {
        private uint screenWidth;
        private bool fullscreen;
        private GraphicsDeviceManager graphics;
        private SpriteBatch batch;
        private Action load;
        public GameStateManager states;
        
        //source: https://stackoverflow.com/questions/4362111/how-do-i-show-a-console-output-window-in-a-forms-application
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public GameWindow(uint width, bool fullscreen = false)
        {
            graphics = new GraphicsDeviceManager(this);
            AssetManager.content = Content;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            screenWidth = width;
            this.fullscreen = fullscreen;

            AllocConsole();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("MonogameCore running!");
            Console.WriteLine("Close this console to close the game!");
            Console.ForegroundColor = ConsoleColor.White;
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
            uint screenHeight = screenWidth / 16 * 9;
            Camera.SetupEnv(graphics, GraphicsDevice);
            Camera.SetupResolution(screenWidth, screenHeight, fullscreen);
            batch = new SpriteBatch(GraphicsDevice);
            AssetManager.Batch = batch;
            AssetManager.device = GraphicsDevice;
            AssetManager.LoadPlaceholder();
            states = new GameStateManager(batch);
            load();
            TextureManager.CalculateTree();
            TextureManager.Bake();
            GameStateManager.LoadStartingState();
            this.IsFixedTimeStep = false;
        }
        
        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update();
            float time = Time.Elapsed;
            Debug.Update(Time.Elapsed);
            states.Update(time * Time.timeScale);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Time.UpdateFps();
            GraphicsDevice.Clear(Color.Black);
            states.Draw(Time.Elapsed, batch, GraphicsDevice);
            base.Draw(gameTime);
        }
    }
}