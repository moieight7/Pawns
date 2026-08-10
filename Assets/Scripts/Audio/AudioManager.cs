using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;
    public SoundBankMaster soundBankMaster;

    public List<Sound> sounds = new List<Sound>();

    public static AudioManager instance { get; private set; }

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one AudioManager object! Destroying the newest one.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        foreach (SoundBank soundBank in soundBankMaster.soundBanks)
        {
            foreach (Sound s in soundBank.sounds)
            {
                if (sounds.Contains(s)) continue;

                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.name = s.source.clip.name;
                Debug.Log("Adding sound " + s.name + " to sound library.");

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.outputAudioMixerGroup = s.mixer;

                sounds.Add(s);
            }
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds.ToArray(), sound => sound.name == name);
        s.source.PlayOneShot(s.clip, s.volume);
    }

    public void Play(string name, float volume)
    {
        Sound s = Array.Find(sounds.ToArray(), sound => sound.name == name);
        s.source.volume = volume;
        s.source.PlayOneShot(s.clip, s.volume);
    }

    public void Play(string name, float volume, float pitch)
    {
        Sound s = Array.Find(sounds.ToArray(), sound => sound.name == name);
        s.source.volume = volume;
        s.source.pitch = pitch;
        s.source.PlayOneShot(s.clip, s.volume);
    }

    public void PlayClipAtPoint(string name, Transform point)
    {
        Sound s = Array.Find(sounds.ToArray(), sound => sound.name == name);
        AudioSource.PlayClipAtPoint(s.clip, point.position, s.volume);
    }

    public AudioClip GetClip(string name)
    {
        Sound s = Array.Find(sounds.ToArray(), sound => sound.name == name);
        return s.clip;
    }

    public Sound GetSound(string name)
    {
        Sound s = Array.Find(sounds.ToArray(), sound => sound.name == name);
        return s;
    }

    public float SetVolume(string name)
    {
        Sound s = Array.Find(sounds.ToArray(), sound => sound.name == name);
        return s.volume;
    }
}
