using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public abstract class GameState
    {
        public LineRenderer lineRenderer;
        public GameObjectManager objects;
        public UIObjectManager ui;
        internal Collision collision;
        internal LayeredRenderer renderer;
        internal LineRenderer lines;
        internal bool loaded = false;

        public GameState()
        {
            objects = new GameObjectManager();
            ui = new UIObjectManager();
            renderer = new LayeredRenderer();
            lines = new LineRenderer();
            lineRenderer = new LineRenderer();
            collision = new Collision(lines);
        }
        
        public abstract void Load(SpriteBatch batch);
        public abstract void Unload();

        public virtual void Update(float time)
        {
            Timers.Update(time);
            collision.Check();
            objects.Update(time);
            ui.Update();
            objects.UpdateDebugInfo();
        }

        public virtual void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            device.Clear(Color.Black);
            batch.Begin(SpriteSortMode.Deferred, GameStateManager.blendstate, GameStateManager.samplerstate, null, null, null, Camera.TranslationMatrix);
            renderer.Render();
            batch.End();
            batch.Begin();
            lineRenderer.Render(batch);
            if(Debug.drawLines) lines.Render(batch);
            if(Debug.showAtlas) batch.Draw(TextureManager.atlas, new Rectangle(0, 0, (int)Camera.ScreenSize.Y, (int)Camera.ScreenSize.Y), Color.White);
            ui.Draw(batch);
            batch.End();
        }

        public bool Loaded { get { return loaded; } }
    }
    
    public enum CHANGETYPE
    {
        LOAD,
        SWITCH
    }

    public class GameStateManager
    {
        private Dictionary<string, GameState> states;
        private GameState currentstate;
        private static GameStateManager instance;
        private SpriteBatch batch;
        internal static BlendState blendstate = BlendState.NonPremultiplied;
        internal static SamplerState samplerstate = SamplerState.PointWrap;

        internal GameStateManager(SpriteBatch batch)
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
            {
                currentstate = states[name];
                Debug.PrintNotification("GameState loaded: ", "\"" + name + "\"");
            }
            else Debug.PrintError("Could not find GameState " + name + "!");
        }

        public static void RequestChange(string state, CHANGETYPE type)
        {
            if (type == CHANGETYPE.LOAD && instance.currentstate != null)
            {
                instance.currentstate.Unload();
                instance.currentstate.ui.Clear();
                instance.currentstate.objects.Clear();
                instance.currentstate.collision.Clear();
                instance.currentstate.renderer.Clear();
                instance.currentstate.loaded = false;
                Time.timeScale = 1.0f;
            }
            instance.SetState(state);
            instance.currentstate.loaded = true;
            if (type == CHANGETYPE.LOAD) instance.currentstate.Load(instance.batch);
            GC.Collect();
        }

        internal void Update(float time)
        {
            Input.Update();
            if (currentstate == null) return;
            currentstate.Update(time);
        }

        internal void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            if (currentstate == null) return;
            currentstate.Draw(time, batch, device);
        }

        public void AddState(string name, GameState state)
        {
            if (state == null) return;
            if (states.ContainsKey(name)) return;
            states.Add(name, state);
            Debug.PrintNotification("GameState added: ", "\"" + name + "\"");
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

        internal static void LoadStartingState()
        {
            instance.currentstate.Load(instance.batch);
            instance.currentstate.loaded = true;
        }

        public static void SetRenderingMode(BlendState bs, SamplerState ss)
        {
            blendstate = bs;
            samplerstate = ss;
        }

        public static GameState CurrentState { get { return instance.currentstate; } }
    }
}