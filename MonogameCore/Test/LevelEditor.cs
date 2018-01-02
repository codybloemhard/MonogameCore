using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Core;
using System.IO;
using System.Collections.Generic;

namespace MonogameCore.Test
{
    public class LevelEditor : GameState
    {
        private List<GameObject> allObjects = new List<GameObject>();
        private const string url = "../../../../Content/level.txt";

        public override void Load(SpriteBatch batch)
        {
            Button button = new Button(this, "Finish", "block", () => Finish(),
                AssetManager.GetResource<SpriteFont>("mainFont"), new Vector2(14, 0), new Vector2(2, 1));
        }

        public override void Unload() { }

        public override void Update(float time)
        {
            base.Update(time);
            if (Input.GetKey(PressAction.PRESSED, Keys.Enter))
            {
                GameObject newObject = new GameObject("new", this, 0, true);
                newObject.AddComponent(new CRender("block"));
                newObject.AddComponent(new CAABB());
                newObject.AddComponent(new CLevelEditorObject(newObject));
                newObject.Pos = Input.GetMousePosition();
                newObject.Size = new Vector2(1f, 1f);
            }
            if (Input.GetKey(PressAction.DOWN, Keys.Right))
                Camera.SetCameraTopLeft(Grid.ToGridSpace(Camera.TopLeft) + new Vector2(0.01f, 0));
            else if (Input.GetKey(PressAction.DOWN, Keys.Left))
                Camera.SetCameraTopLeft(Grid.ToGridSpace(Camera.TopLeft) + new Vector2(-0.01f, 0));
            if (Input.GetKey(PressAction.DOWN, Keys.Up))
                Camera.SetCameraTopLeft(Grid.ToGridSpace(Camera.TopLeft) + new Vector2(0, -0.01f));
            else if (Input.GetKey(PressAction.DOWN, Keys.Down))
                Camera.SetCameraTopLeft(Grid.ToGridSpace(Camera.TopLeft) + new Vector2(0, 0.01f));
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }

        public void Finish()
        {
            WriteLevel();
            ReadLevel();
            //GameStateManager.RequestChange("game", CHANGETYPE.LOAD);
        }

        public void WriteLevel()
        {
            using (BinaryWriter w = new BinaryWriter(File.Open(url, FileMode.Open)))
            {
                int count = CLevelEditorObject.objectList.Count;
                w.Write(count);
                for (int i = 0; i < count; i++)
                {
                    GameObject obj = CLevelEditorObject.objectList[i];
                    w.Write(obj.Pos.X);
                    w.Write(obj.Pos.Y);
                    w.Write(obj.Size.X);
                    w.Write(obj.Size.Y);
                    w.Write(obj.tag);
                }
            }
        }

        public void ReadLevel()
        {
            if (!File.Exists(url)) return;
            using (BinaryReader r = new BinaryReader(File.Open(url, FileMode.Open)))
            {
                int count = r.ReadInt32();
                for(int i = 0; i < count; i++)
                {
                    Console.WriteLine(r.ReadSingle());
                    Console.WriteLine(r.ReadSingle());
                    Console.WriteLine(r.ReadSingle());
                    Console.WriteLine(r.ReadSingle());
                    Console.WriteLine(r.ReadString());
                }
            }
        }
    }
}