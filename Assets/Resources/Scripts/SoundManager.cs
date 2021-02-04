using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    List<AudioSource> clips = new List<AudioSource>();
    Dictionary<AudioClip, AudioSource> dicSounds = new Dictionary<AudioClip, AudioSource>();

    public void Play(AudioClip sound, bool loop = false, float volume = 1, int position = 0)
    {
        if (!dicSounds.ContainsKey(sound))
        {
            var test = gameObject.AddComponent<AudioSource>();
            test.clip = sound;
            dicSounds.Add(test.clip, test);
        }
        if (dicSounds[sound])
        {
            if (dicSounds[sound].isPlaying) return;
            dicSounds[sound].Play();
            dicSounds[sound].loop = loop;
            dicSounds[sound].volume = volume;
            dicSounds[sound].time = position;
        }
    }
}
