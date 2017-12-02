using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class Line
    {
        public Vector2 p, q;
        private Vector2 diff;
        private float angle;
        private Rectangle r;
        private Color colour;

        public Line(Vector2 p, Vector2 q, Color colour = default(Color))
        {
            this.p = p;
            this.q = q;
            diff = q - p;
            angle = (float)Math.Atan2(diff.Y, diff.X);
            r = new Rectangle((int)Grid.ToScreenSpace(p).X,
                            (int)Grid.ToScreenSpace(p).Y,
                            (int)Grid.ToScreenSpace(new Vector2(diff.Length(), 1)).X, 1);
            if (colour == default(Color)) colour = Color.White;
            this.colour = colour;
        }

        public Line(float px, float py, float qx, float qy, Color colour = default(Color))
        {
            p = new Vector2(px, py);
            q = new Vector2(qx, qy);
            diff = q - p;
            angle = (float)Math.Atan2(diff.Y, diff.X);
            r = new Rectangle((int)Grid.ToScreenSpace(p).X,
                            (int)Grid.ToScreenSpace(p).Y,
                            (int)Grid.ToScreenSpace(new Vector2(diff.Length(), 1)).X, 1);
            if (colour == default(Color)) colour = Color.White;
            this.colour = colour;
        }

        public void Render(SpriteBatch batch)
        {
            batch.Draw(AssetManager.placeholder, r, null, colour, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }

    public class LineRenderer
    {
        private List<Line> lines;

        public LineRenderer()
        {
            lines = new List<Line>();
        }

        public void Add(Line l)
        {
            lines.Add(l);
        }

        public void Remove(Line l)
        {
            lines.Remove(l);
        }

        public void Clear()
        {
            lines.Clear();
        }

        public void Render(SpriteBatch batch)
        {
            for (int i = 0; i < lines.Count; i++)
                lines[i].Render(batch);
        }
    }
}