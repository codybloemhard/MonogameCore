using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class UIObjectManager
    {
        private List<UIElement> elements;

        public UIObjectManager()
        {
            elements = new List<UIElement>();
        }

        public void Update()
        {
            for (int i = 0; i < elements.Count; i++) {
                if (elements[i] is Button)
                    (elements[i] as Button).Update();
            }
        }

        public void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].Draw(batch);
            }
        }

        public void Add(UIElement e)
        {
            elements.Add(e);
        }

        public void Remove(UIElement e)
        {
            elements.Remove(e);
        }

        public void Clear()
        {
            elements.Clear();
        }
    }
}