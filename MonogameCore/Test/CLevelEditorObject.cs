using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameCore.Test
{
    class CLevelEditorObject : Component
    {
        private List<string> components = new List<string>();
        private float precision = 2;
        private string backup;
        private static bool staticGrabbed;
        private bool grabbed, axisAligned;
        private static GameObject selected;
        private Vector2 grabPoint;
        private MultipleLinesText properties;
        private float input;

        public CLevelEditorObject(GameObject GO)
        {
            List<string> text = new List<string>();
            text.Add("XSize:");
            text.Add("YSize:");
            text.Add("Texture:");
            text.Add("Colour:");
            text.Add("Components:");
            properties = new MultipleLinesText(GO.Context, text, new Vector2(10, 3), new Vector2(0, 0), AssetManager.GetResource<SpriteFont>("mainFont"));
            properties.Pos = new Vector2(16, 9) - properties.Size;
            selected = GO;
        }

        public override void Update(float time)
        {
            base.Update(time);

            if (!(GO.Pos.X + GO.Size.X < 16 - properties.Size.X || GO.Pos.Y + GO.Size.Y < properties.Pos.Y))
                properties.Pos = new Vector2(0, 9) - new Vector2(0, properties.Size.Y);
            else 
                properties.Pos = new Vector2(16, 9) - properties.Size;

            Vector2 mousePos = Input.GetMouseWorldPosition();
            if (Input.GetMouseButton(PressAction.PRESSED, MouseButton.LEFT))
            {
                if (GO.GetAABB().Inside(mousePos))
                {
                    selected = GO;
                    if (!staticGrabbed)
                    {
                        staticGrabbed = true;
                        grabbed = true;
                        grabPoint = mousePos - GO.Pos;
                    }
                }

                else
                {
                    if (selected == GO)
                        selected = null;
                }
            }
            else if (Input.GetMouseButton(PressAction.RELEASED, MouseButton.LEFT))
            {
                grabbed = staticGrabbed = false;
                grabPoint = Vector2.Zero;
            }

            if (selected == GO)
            {
                (GO.Renderer as CRender).colour = new Color(180, 180, 180);
                properties.active = true;
            }
            else
            {
                (GO.Renderer as CRender).colour = Color.White;
                if (properties.Hover == -1 && properties.selected == -1)
                {
                    properties.active = false;
                }
            }

            if (Input.GetKey(PressAction.DOWN, Keys.LeftShift))
                axisAligned = true;
            else
                axisAligned = false;

            if (grabbed)
            {
                if (!axisAligned)
                    GO.Pos = mousePos - grabPoint;
                else
                    GO.Pos = new Vector2((int)(mousePos.X*precision) - (int)(grabPoint.X* precision), (int)(mousePos.Y* precision) - (int)(grabPoint.Y* precision))/ precision;
            }

            HandleProperties();
        }

        public void HandleProperties()
        {
            if (Input.GetMouseButton(PressAction.PRESSED, MouseButton.LEFT))
            {
                if (properties.Hover != -1)
                {
                    if (properties.Hover != properties.selected)
                    {
                        if (properties.selected != -1)
                        {
                            HandleInput();
                            properties.text[properties.selected] = backup;
                            Console.WriteLine("ok");
                        }
                        backup = properties.text[properties.Hover];
                    }
                    properties.text[properties.Hover] = "";
                }
                else if (properties.selected != -1)
                {
                    HandleInput();
                    properties.text[properties.selected] = backup;
                }
                properties.selected = properties.Hover;
            }
            if (properties.selected != -1)
            {
                properties.text[properties.selected] = Input.Type(properties.text[properties.selected]);
            }
        }

        public void HandleInput()
        {
            if (properties.text[properties.selected] != "")
            {
                if (backup == "XSize:")
                {
                    if (float.TryParse(properties.text[properties.selected], out input))
                        GO.Size = new Vector2(input, GO.Size.Y);
                }
                if (backup == "YSize:")
                {
                    if (float.TryParse(properties.text[properties.selected], out input))
                        GO.Size = new Vector2(GO.Size.X, input);
                }
                if (backup == "Texture:")
                    GO.Renderer.SetSprite(properties.text[properties.selected]);
                if (backup == "Components:")
                {
                    components.Add(properties.text[properties.selected]);    
                } 
            }
        }

        public static bool StaticGrabbed { get { return staticGrabbed; } }
    }
}

