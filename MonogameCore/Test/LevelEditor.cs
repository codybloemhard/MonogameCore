using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Core;
using System.IO;
using System.Collections.Generic;

namespace MonogameCore.Test
{
    class LevelEditor : GameState
    {
        private bool moving;
        private Vector2 grabPoint, offset, prevMousePos;

        public override void Load(SpriteBatch batch)
        {
            Button button = new Button(this, "Finish", "block", () => Finish(),
                AssetManager.GetResource<SpriteFont>("mainFont"), new Vector2(14, 0), new Vector2(2, 1));
        }

        public override void Unload()
        {
        }

        public override void Update(float time)
        {
            base.Update(time);
            // Camera.SetCameraTopLeft(new Vector2(0, 0));
            if (Input.GetKey(PressAction.PRESSED, Keys.Enter))
            {
                GameObject newObject = new GameObject("new", this, 0, true);
                newObject.AddComponent(new CRender("block"));
                newObject.AddComponent(new CAABB());
                newObject.AddComponent(new CLevelEditorObject(newObject));
                newObject.Pos = Input.GetMousePosition();
                newObject.Size = new Vector2(1, 1);
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
            string test;

            using (StreamWriter fileWriter = new StreamWriter("../../../../Content/level.txt", false))
            {
                fileWriter.AutoFlush = true;

                for (int i = 0; i < CLevelEditorObject.objectList.Count; i++)
                {
                    fileWriter.Write(MathH.CompressFloat(CLevelEditorObject.objectList[i].Pos.X));
                    fileWriter.Write(MathH.CompressFloat(CLevelEditorObject.objectList[i].Pos.Y));
                    fileWriter.Write(MathH.CompressSTring(CLevelEditorObject.objectList[i].tag, 5));
                }                
            }

            using (StreamReader fileReader = new StreamReader("../../../../Content/level.txt"))
            {
                test = fileReader.ReadLine();
                Console.WriteLine(test);
            }
            if (test != null)
            {
                Console.WriteLine(MathH.UncompressFloat(test.Substring(0, 2)));
                Console.WriteLine(MathH.UncompressFloat(test.Substring(2, 2)));
                Console.WriteLine(MathH.UncompressString(test.Substring(4, 5)));
            }

            GameStateManager.RequestChange("game", CHANGETYPE.LOAD);
        }
    }
}