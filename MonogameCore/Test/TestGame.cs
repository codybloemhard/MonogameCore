using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            stone0.AddComponent("render", new CRender("block"));
            stone0.AddComponent("collider", new CAABB());
            stone0.Pos = new Vector2(0, 8);
            stone0.Size = new Vector2(8, 1);
            GameObject stone1 = new GameObject("stone", this, 2, true);
            stone1.AddComponent("render", new CRender("block"));
            stone1.AddComponent("collider", new CAABB());
            stone1.Pos = new Vector2(9, 7);
            stone1.Size = new Vector2(2, 2);
            GameObject stone2 = new GameObject("stone", this, 2, true);
            stone2.AddComponent("render", new CRender("block"));
            stone2.AddComponent("collider", new CAABB());
            stone2.Pos = new Vector2(12, 5);
            stone2.Size = new Vector2(3, 0.2f);
            GameObject stone3 = new GameObject("stone", this, 2, true);
            stone3.AddComponent("render", new CRender("block"));
            stone3.AddComponent("collider", new CAABB());
            stone3.Pos = new Vector2(8, 3);
            stone3.Size = new Vector2(3, 0.2f);
            GameObject player = new GameObject("player", this, 1);
            player.AddComponent("render", new CRender("block"));
            player.AddComponent("move", new CPlayerMovement(3.0f));
            player.AddComponent("collider", new CAABB());
            player.AddComponent("shoot", new CShoot());
            player.Pos = new Vector2(1, 1);
            player.Size = new Vector2(0.5f, 0.5f);
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
            base.Update(time);
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}