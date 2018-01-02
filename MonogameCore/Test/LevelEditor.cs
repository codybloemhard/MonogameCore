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
            using (StreamWriter fileWriter = new StreamWriter(url, false))
            {
                fileWriter.AutoFlush = true;
                for (int i = 0; i < CLevelEditorObject.objectList.Count; i++)
                {
                    fileWriter.Write(MathH.CompressFloat(CLevelEditorObject.objectList[i].Pos.X));
                    fileWriter.Write(MathH.CompressFloat(CLevelEditorObject.objectList[i].Pos.Y));
                    fileWriter.Write(MathH.CompressFloat(CLevelEditorObject.objectList[i].Size.X));
                    fileWriter.Write(MathH.CompressFloat(CLevelEditorObject.objectList[i].Size.Y));
                    fileWriter.Write(MathH.CompressString(CLevelEditorObject.objectList[i].tag, 5));
                }
            }
            ReadLevel();
            //GameStateManager.RequestChange("game", CHANGETYPE.LOAD);
        }

        public void WriteLevel()
        {
            
        }

        public void ReadLevel()
        {
            string content;
            using (StreamReader fileReader = new StreamReader(url))
            {
                content = fileReader.ReadLine();
                Console.WriteLine(content);
            }
            int len = content.Length;
            int togo = len;
            while (content != null && togo > 0)
            {
                int done = len - togo;
                Console.WriteLine(MathH.UncompressFloat(content.Substring(done + 0, 2)));
                Console.WriteLine(MathH.UncompressFloat(content.Substring(done + 2, 2)));
                Console.WriteLine(MathH.UncompressFloat(content.Substring(done + 4, 2)));
                Console.WriteLine(MathH.UncompressFloat(content.Substring(done + 6, 2)));
                Console.WriteLine(MathH.UncompressString(content.Substring(done + 8, 5)));
                togo -= 13;
            }
        }
    }
}