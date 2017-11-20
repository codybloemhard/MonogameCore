using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Core
{
    public struct View
    {
        public float x, y, w, h, mx, my;

        public void SetSize(float w, float h)
        {
            this.w = w;
            this.h = h;
        }

        public void SetTL(float x, float y)
        {
            this.x = x;
            this.y = y;
            mx = x + (w / 2.0f);
            my = y + (h / 2.0f);
        }

        public void SetMiddle(float x, float y)
        {
            mx = x;
            my = y;
            this.x = mx - (w / 2.0f);
            this.y = my - (h / 2.0f);
        }
        //LERP = linear interpolation
        public float Lerp(float a, float b, float t)
        {
            return a + t * (b - a);
        }
        //lerp over a vector
        public Vector2 Lerp(Vector2 oldp, Vector2 newp, float xt, float yt)
        {
            Vector2 ans = new Vector2(0, 0);
            ans.X = Lerp(oldp.X, newp.X, xt);
            ans.Y = Lerp(oldp.Y, newp.Y, yt);
            return ans;
        }
        //polynomial functie van 0 tot 1 die erg oploopt als deze dicht bij 1 is.
        public float ExpTrans(float t)
        {
            return MathHelper.Clamp((float)Math.Pow(t, 9), 0, 1);
        }
        //verplaats de camera met een snelheid die afhangt van hoever weg de
        //player van het midden van het scherm is.
        public Vector2 CameraSpeed(Vector2 playerPos, Vector2 targetPos)
        {
            Vector2 onlyx = new Vector2(mx - playerPos.X, 0);
            Vector2 onlyy = new Vector2(0, my - playerPos.Y);
            float distx = onlyx.Length() / 650.0f;
            float disty = onlyy.Length() / 350.0f;
            distx = ExpTrans(distx);
            disty = ExpTrans(disty);
            return Lerp(new Vector2(x, y), targetPos, distx, disty);
        }
    }

    public static class Camera
    {
        private static Matrix translation, spriteScale, final;
        private static View view;
        private static Vector2 scale, size;
        private static Vector2 screenSize;
        private static GraphicsDeviceManager graphics;

        static Camera()
        {
            translation = Matrix.CreateTranslation(0, 0, 0);
            spriteScale = Matrix.CreateScale(1.0f, 1.0f, 1.0f);
            view = new View();
            view.SetSize(screenSize.X, screenSize.Y);
            graphics = null;
        }

        public static void SetupResolution(uint w, uint h, GraphicsDeviceManager g, GraphicsDevice device)
        {
            Grid.Setup(16, 9, w, h);
            g.PreferredBackBufferWidth = (int)w;
            g.PreferredBackBufferHeight = (int)h;
            g.IsFullScreen = false;
            g.ApplyChanges();
            screenSize = new Vector2(g.PreferredBackBufferWidth, g.PreferredBackBufferHeight);

            float targetAspectRatio = (float)w / (float)h;
            int width = (int)screenSize.X;
            int height = (int)(width / targetAspectRatio);
            if (height > screenSize.Y)
            {
                height = (int)screenSize.Y;
                width = (int)(height * targetAspectRatio);
            }

            Viewport viewport = new Viewport();
            viewport.X = (int)(screenSize.X / 2) - (width / 2);
            viewport.Y = (int)(screenSize.Y / 2) - (height / 2);
            viewport.Width = width;
            viewport.Height = height;
            device.Viewport = viewport;

            scale = new Vector2((float)device.Viewport.Width / screenSize.X,
                                        (float)device.Viewport.Height / screenSize.Y);
            spriteScale = Matrix.CreateScale(scale.X, scale.Y, 1);
            view.SetSize(screenSize.X, screenSize.Y);
        }

        public static void SetCameraTopLeft(Vector2 pos)
        {
            view.SetTL(pos.X, pos.Y);
            pos *= scale;
            translation = Matrix.CreateTranslation(-pos.X, -pos.Y, 0);
            final = Matrix.Multiply(spriteScale, translation);
        }

        public static void SetCameraMiddle(Vector2 pos)
        {
            view.SetMiddle(pos.X, pos.Y);
            pos *= scale;
            pos -= new Vector2(screenSize.X, screenSize.Y) * scale / 2.0f;
            translation = Matrix.CreateTranslation(-pos.X, -pos.Y, 0);
            final = Matrix.Multiply(spriteScale, translation);
        }

        public static void FollowPlayer(Vector2 pos)
        {
            Vector2 playerPos = pos;
            Vector2 halfScreen = new Vector2(screenSize.X, screenSize.Y) / 2.0f;
            pos -= halfScreen;
            float xx = MathHelper.Clamp(pos.X, 0, size.X - (halfScreen.X * 2));
            float yy = MathHelper.Clamp(pos.Y, 0, size.Y - (halfScreen.Y * 2));
            Vector2 gotoPos = view.CameraSpeed(playerPos, new Vector2(xx, yy));
            SetCameraTopLeft(gotoPos);
        }
        
        public static void SetSize(Vector2 s) { size = s; }
        public static void SetScreenSize(Vector2 s) { screenSize = s; }
        public static Vector2 TopLeft { get { return new Vector2(view.x, view.y); } }
        public static Vector2 Center { get { return new Vector2(view.mx, view.my); } }
        public static Vector2 WorldSize { get { return size; } }
        public static Vector2 ScreenSize { get { return screenSize; } }
        public static Matrix TranslationMatrix { get { return translation; } }
        public static Matrix ScaleMatrix { get { return spriteScale; } }
        public static Matrix FinalMatrix { get { return final; } }
    }
}