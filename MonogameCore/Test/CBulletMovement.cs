using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace MonogameCore.Test
{
    public class CBulletMovement : Component
    {
        private float speed;
        private Vector2 dir;

        public CBulletMovement(float speed, Vector2 dir) : base()
        {
            this.speed = speed;
            this.dir = dir;
        }

        public override void Init()
        {
            CRender render = GO.Renderer as CRender;
            if (render != null) render.colour = Color.Red;
        }

        public override void Update(float time)
        {
            base.Update(time);
            GO.Pos += dir * speed * time;
            if (GO.Pos.X < 0 || GO.Pos.X > 16
                || GO.Pos.Y < 0 || GO.Pos.Y > 9)
                GO.Destroy();
        }

        public override void OnCollision(GameObject other)
        {
            if (other.tag == "stone")
                GO.Destroy();
        }
    }
}