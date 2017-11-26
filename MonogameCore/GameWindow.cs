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
        private GraphicsDeviceManager graphics;
        private SpriteBatch batch;
        private Action load;
        public GameStateManager states;
        
        //source: https://stackoverflow.com/questions/4362111/how-do-i-show-a-console-output-window-in-a-forms-application
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public GameWindow(uint width)
        {
            graphics = new GraphicsDeviceManager(this);
            AssetManager.content = Content;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            screenWidth = width;

            AllocConsole();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("MonogameCore made by Cody Bloemhard.");
            Console.WriteLine("Close this console to close the game!");
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
            Camera.SetupResolution(screenWidth, screenHeight, graphics, GraphicsDevice);
            batch = new SpriteBatch(GraphicsDevice);
            states = new GameStateManager(batch);
            load();
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            states.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            states.Draw((float)gameTime.ElapsedGameTime.TotalSeconds, batch, GraphicsDevice);
            base.Draw(gameTime);
        }
    }
}