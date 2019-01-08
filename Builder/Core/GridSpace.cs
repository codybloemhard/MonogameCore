using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core
{
    public static class Grid
    {
        private static uint _width, _height;
        private static uint _screenW, _screenH;
        private static Vector2 _mul;
        private static Vector2 scrnsz, grdsz;
        internal static uint dirty = 0;

        static Grid() { _mul = default(Vector2); }

        public static void Setup(uint width, uint height, uint screenW, uint screenH)
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

        public static Vector2 ScaleSprite(Vector2 pixles)
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