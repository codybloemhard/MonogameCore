using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
/*Kleine input wrapper. Om te zien of geklikt is dan hoeven we 
dat niet steeds te checken in de gamecode op basis van het vorige frame etc.*/
namespace Core
{
    public enum MouseButton : byte
    {
        LEFT,
        RIGHT,
        MIDDLE,
        BUTTON0,
        BUTTON1
    }

    public enum PressAction : byte
    {
        DOWN,
        UP,
        PRESSED,
        RELEASED
    }

    public static class Input
    {
        private enum Mstate : byte
        {
            PREV,
            CURRENT
        }
        private static MouseState mprevState, mcurrentState;
        private static Keys[] kPrev, kCurrent;

        static Input()
        {
            mprevState = Mouse.GetState();
            mcurrentState = Mouse.GetState();
            kPrev = Keyboard.GetState().GetPressedKeys();
            kCurrent = Keyboard.GetState().GetPressedKeys();
        }

        internal static void Update()
        {
            mprevState = mcurrentState;
            mcurrentState = Mouse.GetState();
            kPrev = kCurrent;
            kCurrent = Keyboard.GetState().GetPressedKeys();
        }

        public static string Type(string s)
        {
            string toAdd = "";
            foreach (Keys k in kCurrent)
            {
                if (!Array.Exists<Keys>(kPrev, element => element == k))
                {
                    if (k.ToString().Length == 1)
                        toAdd = k.ToString();
                    else if (k == Keys.Space)
                        toAdd = " ";
                    else if (k == Keys.Back && s.Length >= 1)
                        s = s.Substring(0, s.Length - 1);
                    else if (k == Keys.OemSemicolon)
                        toAdd = ":";
                    else if (k == Keys.OemPeriod)
                        toAdd = ".";
                    else if (k == Keys.OemComma)
                        toAdd = ",";
                    else if (k.ToString()[0] == 'D' && k.ToString().Length == 2)
                        toAdd = k.ToString()[1].ToString();
                }
            }

            if (!Array.Exists<Keys>(kCurrent, element => element == Keys.LeftShift || element == Keys.RightShift))
            {
                toAdd = toAdd.ToLower();
            }

            return s + toAdd;
        }

        private static bool GetButton(Mstate state, MouseButton button)
        {
            MouseState s;
            if (state == Mstate.CURRENT) s = mcurrentState;
            else s = mprevState;
            switch (button)
            {
                case MouseButton.LEFT:
                    return s.LeftButton == ButtonState.Pressed;
                case MouseButton.RIGHT:
                    return s.RightButton == ButtonState.Pressed;
                case MouseButton.MIDDLE:
                    return s.MiddleButton == ButtonState.Pressed;
                case MouseButton.BUTTON0:
                    return s.XButton1 == ButtonState.Pressed;
                case MouseButton.BUTTON1:
                    return s.XButton2 == ButtonState.Pressed;
                default:
                    return false;
            }
        }

        public static bool GetMouseButton(PressAction action, MouseButton button)
        {
            if (action == PressAction.DOWN)
                return GetButton(Mstate.CURRENT, button);
            else if (action == PressAction.UP)
                return !GetButton(Mstate.CURRENT, button);
            else if (action == PressAction.PRESSED)
                return GetButton(Mstate.CURRENT, button) && !GetButton(Mstate.PREV, button);
            else if (action == PressAction.RELEASED)
                return !GetButton(Mstate.CURRENT, button) && GetButton(Mstate.PREV, button);
            return false;
        }

        private static bool KeyIsIn(Mstate state, Keys key)
        {
            Keys[] s = kCurrent;
            if (state == Mstate.PREV) s = kPrev;
            for(int i = 0; i < s.Length; i++)
                if (s[i] == key) return true;
            return false;
        }

        public static bool GetKey(PressAction action, Keys key)
        {
            bool isInPrev = KeyIsIn(Mstate.PREV, key);
            bool isInCurr = KeyIsIn(Mstate.CURRENT, key);
            if (action == PressAction.DOWN) return isInCurr;
            else if (action == PressAction.UP) return !isInCurr;
            else if (action == PressAction.PRESSED) return !isInPrev && isInCurr;
            else if (action == PressAction.RELEASED) return isInPrev && !isInCurr;
            return false;
        }

        public static Vector2 GetMousePosition()
        {
            return Grid.ToGridSpace(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
        }

        public static Vector2 GetMouseWorldPosition()
        {
            return GetMousePosition() + Camera.TopLeft;
        }
    }
}