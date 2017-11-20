using System;
using Microsoft.Xna.Framework;

namespace Core
{
    public static class MathH
    {
        public static Random random;

        static MathH()
        {
            random = new Random();
        }

        public static Vector2 RandomUnitVector()
        {
            Vector2 vec = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
            vec.Normalize();
            double r = random.NextDouble();
            if (r < 0.5f) vec.X *= -1;
            r = random.NextDouble();
            if (r < 0.5f) vec.Y *= -1;
            return vec;
        }
    }
}