using System;
using System.Collections.Generic;
using Core;

namespace Core
{
    public class Component
    {
        internal GameObject gameObject;
        public bool active;

        public Component()
        {
            active = true;
        }

        public virtual void Init()
        {

        }

        public virtual void Update(float time)
        {
            if (!active)
                return;
        }

        public virtual void GOReferenceFunction(GameObject g)
        {
        }

        public GameObject GameObject { get { return gameObject; } }
    }
}