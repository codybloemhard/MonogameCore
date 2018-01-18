using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonogameCore.Test
{
    public class CPlayerMovement : Component
    {
        private float speed;
        private Vector2 dir;

        public CPlayerMovement(float speed) : base()
        {
            this.speed = speed;
            dir = new Vector2(1, 0);
        }

        public override void Init()
        {
            CRender render = GO.Renderer as CRender;
            if (render != null) render.colour = Color.White;
        }

        public override void Update(float time)
        {
            Vector2 velocity = Vector2.Zero;
            if (Input.GetKey(PressAction.DOWN, Keys.A))
                velocity = new Vector2(-1, 0);
            if (Input.GetKey(PressAction.DOWN, Keys.D))
                velocity = new Vector2(+1, 0);
            if (Input.GetKey(PressAction.DOWN, Keys.W))
                velocity = new Vector2(0, -1);
            if (Input.GetKey(PressAction.DOWN, Keys.S))
                velocity = new Vector2(0, +1);

            Camera.SetCameraTopLeft(GO.Pos - GO.Size/2f - new Vector2(16,9)/2f);
            GO.Pos += velocity * time * speed;
            //shoot
            if (Input.GetKey(PressAction.PRESSED, Keys.Space))
                GO.GetComponent<CShoot>().Shoot(dir, new Vector2(0.2f, 0.2f));
        }

        public override void OnCollision(GameObject other)
        {
            if (other.tag == "killer")
                GO.Pos = new Vector2(1, 1);
        }
    }

    public class Example : Component
    {
        public Example() : base() { }

        public override void Init()
        {
        }

        public override void Update(float time)
        {
            base.Update(time);
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
        }
    }
}