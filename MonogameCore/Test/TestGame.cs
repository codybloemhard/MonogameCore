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
            GameObject stone = new GameObject("stone", this, 1, true);
            stone.AddComponent("render", new CRender("block", batch));
            stone.AddComponent("collider", new CAABB());
            stone.Pos = new Vector2(0, 8);
            stone.Size = new Vector2(4, 1);
            GameObject player = new GameObject("player", this, 0, true);
            player.AddComponent("render", new CRender("block", batch));
            player.AddComponent("move", new CPlayerMovement(3.0f));
            player.AddComponent("collider", new CAABB());
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
            text.text = "Position: " + player.Pos.X + " , " + player.Pos.Y;
            base.Update(time);
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}