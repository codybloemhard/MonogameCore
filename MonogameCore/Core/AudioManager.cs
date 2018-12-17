using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Core;

namespace Core
{
    public static class AudioManager
    {
        private static Dictionary<string, SoundEffect> effects;
        private static Dictionary<string, Song> songs;
        private static float masterVolume = 1f, trackVolume = 1f, effectVolume = 1f;

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

        public static void LoadTrack(string name, string file)
        {
            if (songs.ContainsKey(name))
            {
                Debug.PrintError("SoundTrack is already loaded:", name);
                return;
            }
            Song sf = AssetManager.GetResource<Song>(file);
            if (sf == null)
            {
                Debug.PrintError("SoundTrack file could not be found: ", file);
                return;
            }
            songs.Add(name, sf);
        }

        public static void PlayEffect(string name, float volume = 1f, float pitch = 0f, float pan = 0f)
        {
            if (!effects.ContainsKey(name))
            {
                Debug.PrintError("SoundEffect could not be played: ", name);
                return;
            }
            effects[name].Play(volume * effectVolume * masterVolume, pitch, pan);
        }

        public static void PlayTrack(string name)
        {
            return;
            if (!songs.ContainsKey(name))
            {
                Debug.PrintError("SoundTrack could not be played: ", name);
                return;
            }
            MediaPlayer.Stop();
            MediaPlayer.Play(songs[name]);
        }

        public static void StopTrack()
        {
            return;
            MediaPlayer.Stop();
        }

        public static void LoopTrack(bool loop)
        {
            return;
            MediaPlayer.IsRepeating = loop;
        }

        public static void SetTrackVolume(float vol)
        {
            vol = (float)MathH.Clamp(vol, 0f, 1f);
            trackVolume = vol;
            ApplyVolumes();
        }

        public static void SetEffectVolume(float vol)
        {
            effectVolume = vol;
        }

        public static void SetMasterVolume(float v)
        {
            masterVolume = (float)MathH.Clamp(v, 0f, 1f);
            ApplyVolumes();
        }

        private static void ApplyVolumes()
        {
            return;
            MediaPlayer.Volume = trackVolume * masterVolume;
        }
    }
}