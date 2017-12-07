using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameCore.Test
{
    class CHealthBar : Component
    {
        private int HP;
        Text healthBar;
        public CHealthBar(int HP, GameObject GO)
        {
            this.HP = HP;
            List<string> text = new List<string>();
            text.Add("a");
            text.Add("b");
            text.Add("adfsadfadsfdsf");
            healthBar = new Text(GO.Context, text, new Vector2(0, 0), new Vector2(3, 1), AssetManager.GetResource<SpriteFont>("mainFont"));
            healthBar.AddGameObject(GO);
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
            HP -= 1;
        }
    }
}
