using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonogameCore.Test
{
    public class CPlayerMovement : Component
    {
        private float speed;
        private Vector2 dir;
        private float jumpPower = 15f;
        private float acceleration = 0.8f, vertVelo = 0f;
        private bool grounded = false;

        public CPlayerMovement(float speed) : base()
        {
            this.speed = speed;
        }

        public override void Init()
        {
            CRender render = GO.Renderer;
            if (render != null) render.colour = Color.Blue;
        }

        public override void Update(float time)
        {
            if (GO.Pos.Y > 9) GO.Pos = new Vector2(1, -1);
            Vector2 velocity = Vector2.Zero;
            if (Input.GetKey(PressAction.DOWN, Keys.A))
                velocity = new Vector2(-1, 0);
            else if (Input.GetKey(PressAction.DOWN, Keys.D))
                velocity = new Vector2(1, 0);       
            if (velocity != Vector2.Zero)
            {
                dir = velocity;
                dir.Normalize();
            }

            //gravity and jump
            Vector2 feetLeft = GO.Pos + new Vector2(0, GO.Size.Y + 0.01f);
            Vector2 feetRight = GO.Pos + new Vector2(GO.Size.X, GO.Size.Y + 0.01f);
            RaycastResult hitLeft = GO.Raycast(feetLeft, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hitRight = GO.Raycast(feetRight, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hit;
            if (hitLeft.distance > hitRight.distance) hit = hitRight;
            else hit = hitLeft;
            if (hit.hit)
            {
                if (hit.distance < 0.05f) grounded = true;
                else grounded = false;
            }
            else grounded = false;
            if (grounded && Input.GetKey(PressAction.PRESSED, Keys.W))
                vertVelo = -jumpPower;
            if (!grounded) vertVelo += acceleration;
            //speed is in Units/Second
            GO.Pos += velocity * speed * time;
            GO.Pos += new Vector2(0, Math.Min(hit.distance, vertVelo * time));
            //shoot
            if (Input.GetKey(PressAction.PRESSED, Keys.Space))
                GO.GetComponent<CShoot>().Shoot(dir, new Vector2(0.2f, 0.2f));
        }
    }
}