using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public static class Camera
    {
        private static Matrix translation;
        private static Vector2 screenSize, size;
        private static GraphicsDeviceManager graphics;
        private static GraphicsDevice device;
        private static Vector2 tl;

        static Camera()
        {
            translation = Matrix.CreateTranslation(0, 0, 0);
            graphics = null;
        }

        internal static void SetupEnv(GraphicsDeviceManager g, GraphicsDevice d)
        {
            graphics = g;
            device = d;
        }

        public static void SetupResolution(uint w, uint h, bool fullscreen)
        {
            graphics.IsFullScreen = fullscreen;
            if (fullscreen)
            {
                graphics.PreferredBackBufferWidth = device.DisplayMode.Width;
                graphics.PreferredBackBufferHeight = device.DisplayMode.Height;
            }
            else
            {
                graphics.PreferredBackBufferWidth = (int)w;
                graphics.PreferredBackBufferHeight = (int)h;
            }
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();

            screenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Grid.Setup(16, 9, (uint)screenSize.X, (uint)screenSize.Y);
            Grid.dirty = 2;

            float targetAspectRatio = (float)w / (float)h;
            int width = (int)screenSize.X;
            int height = (int)(width / targetAspectRatio);
            if (height > screenSize.Y)
            {
                height = (int)screenSize.Y;
                width = (int)(height * targetAspectRatio);
            }
            
            Viewport viewport = new Viewport();
            viewport.X = 0;
            viewport.Y = 0;
            viewport.Width = width;
            viewport.Height = height;
            device.Viewport = viewport;
        }
        
        public static void SetCameraTopLeft(Vector2 pos)
        {
            Vector2 trans = Grid.ToScreenSpace(pos);
            translation = Matrix.CreateTranslation(-trans.X, -trans.Y, 0);
            tl = pos;
        }
        
        public static void SetCameraMiddle(Vector2 pos)
        {
            Vector2 trans = pos;
            trans -= new Vector2(16f, 9f) / 2.0f;
            tl = trans;
            trans = Grid.ToScreenSpace(pos);
            translation = Matrix.CreateTranslation(-trans.X, -trans.Y, 0);
        }
        
        public static void SetSize(Vector2 s) { size = s; }
        public static void SetScreenSize(Vector2 s) { screenSize = s; }
        public static Vector2 TopLeft { get { return tl; } }
        public static Vector2 Center { get { return tl - new Vector2(8, 4.5f); } }
        public static Vector2 WorldSize { get { return size; } }
        public static Vector2 ScreenSize { get { return screenSize; } }
        public static Matrix TranslationMatrix { get { return translation; } }
    }
}