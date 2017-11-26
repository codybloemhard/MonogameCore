using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class Time
    {
        public static float timeScale = 1.0f;
    }

    public sealed class Timer : _tagged
    {
        private float time;
        private float limit;
        private bool wentOff;
        private Action action;
        internal GameState state;

        public Timer(string tag, float limit, Action action = null)
        {
            time = 0f;
            wentOff = false;
            this.tag = tag;
            this.limit = limit;
            this.action = action;
        }

        public void Update(float elapsed)
        {
            if (!state.Loaded) return;
            time += elapsed;
            if(time >= limit && !wentOff && action != null)
            {
                wentOff = true;
                action();
            }
        }

        public void Reset()
        {
            time = 0f;
            wentOff = false;
        }

        public void ReInit(string tag, float limit, Action action = null)
        {
            time = 0f;
            wentOff = false;
            this.tag = tag;
            this.limit = limit;
            this.action = action;
        }

        public float Time { get { return time; } }
        public float TimeLeft { get { return Math.Max(0, limit - time); } }
    }

    public struct ScopedTimer
    {
        public Timer timer;
        public GameState state;
    }

    public static class Timers
    {
        private static List<Timer> timers;
        private static TagEngine tags;

        static Timers()
        {
            timers = new List<Timer>();
            tags = new TagEngine();
        }

        internal static void Update(float time)
        {
            for (int i = 0; i < timers.Count; i++)
                timers[i].Update(time);
        }

        public static void Add(string tag, float limit, Action action = null)
        {
            Timer timer = new Timer(tag, limit, action);
            timer.state = GameStateManager.CurrentState;
            timers.Add(timer);
        }

        public static void Remove(string timer)
        {
            Timer t = FindWithTag(timer);
            if (t == null) return;
            timers.Remove(t);
        }

        public static Timer FindWithTag(string tag)
        {
            return tags.FindWithTag(tag, timers);
        }

        public static Timer[] FindAllWithTag(string tag)
        {
            return tags.FindAllWithTag(tag, timers);
        }

        public static Timer[] FindAllWithTags(string[] tags)
        {
            return Timers.tags.FindAllWithTags(tags, timers);
        }

        public static int Size { get { return timers.Count; } }
    }
}