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

        public static float RAD_TO_DEG = 57.2957795f;
        public static float DEG_TO_RAD = 0.01745329f;

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

        public static Vector2 RandomUnitAngle()
        {
            Vector2 vec = new Vector2();
            double r = random.NextDouble();
            vec.X = (float)Math.Sin(r);
            vec.Y = (float)Math.Cos(r);
            vec.Normalize();
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
        //From:
        //https://tavianator.com/fast-branchless-raybounding-box-intersections/
        //And old code i have written
        public static bool RayBoxIntersection(AABB b, float px, float py, float qx, float qy, ref float ix, ref float iy)
        {
            double tx1 = (b.x - px) / (qx - px);
            double tx2 = (b.x + b.w - px) / 1 / (qx - px);
            double tmin = System.Math.Min(tx1, tx2);
            double tmax = System.Math.Max(tx1, tx2);
            double ty1 = (b.y - py) / (qy - py);
            double ty2 = (b.y + b.h - py) / (qy - py);
            tmin = System.Math.Max(tmin, System.Math.Min(ty1, ty2));
            tmax = System.Math.Min(tmax, System.Math.Max(ty1, ty2));
            ix = (float)tmin * (qx - px) + px;
            iy = (float)tmin * (qy - py) + py;
            return tmax >= tmin && tmax >= 0;
        }

        public static double Clamp(double value, double min, double max)
        {
            if (value > max) value = max;
            else if (value < min) value = min;
            return value;
        }
    }
}