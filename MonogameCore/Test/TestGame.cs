using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonogameCore.Test
{
    public class TestGame : GameState
    {
        public TestGame() : base() { }

        public override void Load(SpriteBatch batch)
        {
            //UI
            SpriteFont font = AssetManager.GetResource<SpriteFont>("mainFont");
            Text text = new Text(this, "Position: ", new Vector2(0f, 0f), new Vector2(16f, 1f), font);
            text.colour = new Color(0, 255, 0);
            text.tag = "positionText";
            Button button = new Button(this, "Menu!", "block", () => GameStateManager.RequestChange("menu", CHANGETYPE.LOAD),
                font, new Vector2(14, 0), new Vector2(2, 1));
            button.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
            //Objects
            GameObject stone0 = new GameObject("stone", this, 2, true);
            stone0.Pos = new Vector2(0, 8);
            stone0.Size = new Vector2(8, 1);
            stone0.AddComponent(new CRender("block"));
            stone0.AddComponent(new CAABB());
            GameObject stone1 = new GameObject("stone", this, 2, true);
            stone1.Pos = new Vector2(9, 7);
            stone1.Size = new Vector2(2, 2);
            stone1.AddComponent(new CRender("block"));
            stone1.AddComponent(new CAABB());
            GameObject stone2 = new GameObject("stone", this, 2, true);
            stone2.Pos = new Vector2(12, 5);
            stone2.Size = new Vector2(3, 0.2f);
            stone2.AddComponent(new CRender("block"));
            stone2.AddComponent(new CAABB());
            GameObject stone3 = new GameObject("stone", this, 2, true);
            stone3.Pos = new Vector2(8, 3);
            stone3.Size = new Vector2(3, 0.2f);
            stone3.AddComponent(new CRender("block"));
            stone3.AddComponent(new CAABB());
            GameObject killer = new GameObject("killer", this, 2);
            killer.AddComponent(new CRender("suprise"));
            killer.AddComponent(new CAABB());
            killer.Pos = new Vector2(3, 5);
            killer.Size = new Vector2(1, 1);
            killer.Renderer.colour = Color.Red;
            GameObject player = new GameObject("player", this, 1);
            player.AddComponent(new CRender("dude"));
            player.AddComponent(new CPlayerMovement(3.0f));
            player.AddComponent(new CAABB());
            player.AddComponent(new CShoot());
            player.AddComponent(new CHealthBar(5, player));
            player.Pos = new Vector2(1, 1);
            player.Size = new Vector2(0.5f, 1.0f);
            uint max = 1000;
            for (int i = 0; i < max; i++)
            {
                float t = (float)i / (float)max * 2 * MathH.PI;
                float rad = 2;
                GameObject stone = new GameObject("stone", this, 2, true);
                stone.Pos = new Vector2(5, 3) + new Vector2((float)Math.Sin(t) * rad, (float)Math.Cos(t) * rad);
                stone.Size = new Vector2(0.1f, 0.1f);
                stone.AddComponent(new CRender("block"));
                stone.AddComponent(new CAABB());
            }
        }
        
        public override void Unload()
        {
            
        }

        public override void Update(float time)
        {
            Camera.SetCameraTopLeft(new Vector2(0, 0));
            Text text = ui.FindWithTag("positionText") as Text;
            GameObject player = objects.FindWithTag("player");
            text.text = "Position: " + MathH.Float(player.Pos.X, 2) + " , " + MathH.Float(player.Pos.Y, 2);
            if (Input.GetKey(PressAction.PRESSED, Keys.P))
                Debug.SwitchMode();
            base.Update(time);
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}