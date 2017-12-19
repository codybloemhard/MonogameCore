using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework.Input;

namespace MonogameCore.Test
{
    public class CFsm : Component
    {
        private FSM fsm = new FSM();

        public CFsm() : base() { }

        public override void Init()
        {
            fsm.Add("sayhello", Hello);
            fsm.Add("saygoodbye", Doei);
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (Input.GetKey(PressAction.PRESSED, Keys.Q))
                fsm.SetCurrentState("sayhello");
            else if (Input.GetKey(PressAction.PRESSED, Keys.E))
                fsm.SetCurrentState("saygoodbye");
            fsm.Update();
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
        }

        private void Hello()
        {
            Console.WriteLine("hello");
        }

        private void Doei()
        {
            Console.WriteLine("doei");
        }
    }
}