using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//Simpele GameStateManager, spreekt voorzich.
namespace Core
{
    public abstract class GameState
    {
        protected GameObjectManager manager;

        public GameState()
        {
            manager = new GameObjectManager();
        }

        public abstract void Load(SpriteBatch batch);
        public abstract void Unload();
        public virtual void Update(float time)
        {
            manager.Update(time);
        }
        public virtual void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            batch.Begin();
            manager.Draw();
            batch.End();
        }
    }
    
    public enum CHANGETYPE
    {
        LOAD,
        SWITCH
    }

    public class GameStateChange
    {
        public string newstate;
        public CHANGETYPE type;
        public GameStateChange(string s, CHANGETYPE t)
        {
            newstate = s;
            type = t;
        }
    }

    public class GameStateManager
    {
        private Dictionary<string, GameState> states;
        private GameState currentstate;
        private static GameStateManager instance;
        private SpriteBatch batch;

        public GameStateManager(SpriteBatch batch)
        {
            if (instance != null) return;
            states = new Dictionary<string, GameState>();
            currentstate = null;
            this.batch = batch;
            instance = this;
        }

        private void SetState(string name)
        {
            if (states.ContainsKey(name))
                currentstate = states[name];
        }

        public static void RequestChange(GameStateChange change)
        {
            if (change == null) return;
            if (change.type == CHANGETYPE.LOAD) instance.currentstate.Unload();
            instance.SetState(change.newstate);
            if (change.type == CHANGETYPE.LOAD) instance.currentstate.Load(instance.batch);
        }

        public void Update(float time)
        {
            Input.Update();
            if (currentstate == null) return;
            currentstate.Update(time);
        }

        public void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            if (currentstate == null) return;
            currentstate.Draw(time, batch, device);
        }

        public void AddState(string name, GameState state)
        {
            if (state == null) return;
            if (states.ContainsKey(name)) return;
            states.Add(name, state);
            states[name].Load(instance.batch);
        }

        public void RemoveState(string name)
        {
            if (states.ContainsKey(name))
                states.Remove(name);
        }

        public void SetStartingState(string name)
        {
            if (currentstate != null) return;
            SetState(name);
        }
    }
}