using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonogameCore.Test
{
    public class CPlayerMovement : Component
    {
        protected float speed;

        public CPlayerMovement(float speed) : base()
        {
            this.speed = speed;
        }

        public override void Update(float time)
        {
            Vector2 velocity = Vector2.Zero;
            if (Input.GetKey(PressAction.DOWN, Keys.A))
                velocity = new Vector2(-speed, 0);
            else if (Input.GetKey(PressAction.DOWN, Keys.D))
                velocity = new Vector2(speed, 0);
            else if (Input.GetKey(PressAction.DOWN, Keys.W))
                velocity = new Vector2(0, -speed);
            else if (Input.GetKey(PressAction.DOWN, Keys.S))
                velocity = new Vector2(0, speed);
            //speed is in Units/Second
            GameObject.Pos += velocity * time;
        }

        public override void OnCollision(GameObject other)
        {
            if (other.tag == "stone")
            {
                GameObject.Pos = Vector2.Zero;
                Console.WriteLine("ouch!");
            }
        }
    }
}