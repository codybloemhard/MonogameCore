using System;
using System.Collections.Generic;

namespace Core
{
    public class FSM
    {
        private Dictionary<string, Action> states;
        private string current;

        public FSM()
        {
            states = new Dictionary<string, Action>();
        }

        public void Add(string name, Action a)
        {
            if (states.ContainsKey(name)) return;
            states.Add(name, a);
        }

        public void Remove(string name)
        {
            if (!states.ContainsKey(name)) return;
            states.Remove(name);
        }

        public void Update()
        {
            if (!states.ContainsKey(current)) return;
            if (states[current] == null) return;
            states[current]();
        }

        public void SetCurrentState(string name)
        {
            if (!states.ContainsKey(name)) return;
            current = name;
        }

        public string CurrentState { get { return current; } }
    }
}