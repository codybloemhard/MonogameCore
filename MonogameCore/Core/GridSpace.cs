using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
/*Om te voorkomen dat we met pixels werken, nu werken we in game-space
Zelf aantegeven dimensies, resolutie independend.*/
namespace Core
{
    public static class Grid
    {
        private static int _width, _height;
        private static int _screenW, _screenH;
        private static Vector2 _mul;
        private static Vector2 scrnsz, grdsz;

        static Grid() { _mul = default(Vector2); }

        public static void Setup(int width, int height, int screenW, int screenH)
        {
            _width = width;
            _height = height;
            _screenW = screenW;
            _screenH = screenH;
            _mul = new Vector2();
            _mul.X = (float)width / screenW;
            _mul.Y = (float)height / screenH;
            scrnsz = new Vector2(screenW, screenH);
            grdsz = new Vector2(width, height);
        }

        public static Vector2 ToScreenSpace(Vector2 gridP)
        {
            if (_mul == default(Vector2)) return _mul;
            return gridP / _mul;
        }

        public static Vector2 ToGridSpace(Vector2 screenP)
        {
            if (_mul == default(Vector2)) return _mul;
            return screenP * _mul;
        }

        public static Vector2 Scale(Vector2 pixles)
        {
            Vector2 a = pixles * _mul;
            a.X = 1 / a.X;
            a.Y = 1 / a.Y;
            return a;
        }

        public static Vector2 ScreenSize
        {
            get { return scrnsz; }
        }

        public static Vector2 GridSize
        {
            get { return grdsz; }
        }
    }
}