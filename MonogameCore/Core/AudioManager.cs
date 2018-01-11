using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Core;

namespace MonogameCore
{
    public static class AudioManager
    {
        private static Dictionary<string, SoundEffect> effects;
        private static Dictionary<string, Song> songs;

        static AudioManager()
        {
            effects = new Dictionary<string, SoundEffect>();
            songs = new Dictionary<string, Song>();
        }

        public static void LoadEffect(string name, string file)
        {
            if (effects.ContainsKey(name))
            {
                Debug.PrintError("SoundEffect is already loaded:", name);
                return;
            }
            SoundEffect sf = AssetManager.GetResource<SoundEffect>(file);
            if(sf == null)
            {
                Debug.PrintError("SoundEffect file could not be found: ", file);
                return;
            }
            effects.Add(name, sf);
        }

        public static void PlayEffect(string name, float volume = 1f, float pitch = 0f, float pan = 0f)
        {
            if (!effects.ContainsKey(name))
            {
                Debug.PrintError("SoundEffect could not be played: ", name);
                return;
            }
            effects[name].Play(volume, pitch, pan);
        }


    }
}