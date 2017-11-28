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

        public static string Float(float x, int p)
        {
            string res = "" + Math.Round(x, p);
            int c = -1;
            for(int i = 0; i < res.Length; i++)
            {
                if (c != -1) c++;//masterrace
                if (res[i] == '.' || res[i] == ',') c = 0;
            }
            if(c == -1)
            {
                res += '.';
                for(uint i = 0; i < p; i++)
                    res += '0';
            }
            else for (uint i = 0; i < Math.Max(0, p - c); i++)
                res += '0';
            return res;
        }
    }
}