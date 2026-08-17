using DG.Tweening;
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

                #if UNITY_EDITOR 
                Debug.Log("Adding sound " + s.name + " to sound library.");
                #endif

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
        if (!s.source.loop) s.source.PlayOneShot(s.clip, s.volume);
        else s.source.Play();
    }

    public void Play(string name, float volume)
    {
        Sound s = Array.Find(sounds.ToArray(), sound => sound.name == name);
        s.source.volume = volume;
        if (!s.source.loop) s.source.PlayOneShot(s.clip, s.volume);
        else s.source.Play();
    }

    public void Play(string name, float volume, float pitch)
    {
        Sound s = Array.Find(sounds.ToArray(), sound => sound.name == name);
        s.source.volume = volume;
        s.source.pitch = pitch;
        if (!s.source.loop) s.source.PlayOneShot(s.clip, s.volume);
        else s.source.Play();
    }

    public void Play(string name, float volume, float pitch, float delay)
    {
        Sound s = Array.Find(sounds.ToArray(), sound => sound.name == name);
        s.source.volume = volume;
        s.source.pitch = pitch;
        StartCoroutine(PlayWithDelay(s, delay));
    }

    public void StopLoopingAudio(string name)
    {
        Sound s = Array.Find(sounds.ToArray(), sound => sound.name == name);
        s.source.Stop();
    }

    public AudioSource GetSource(string name)
    {
        Sound s = Array.Find(sounds.ToArray(), sound => sound.name == name);
        return s.source;
    }

    public void FadeIn(string name, float startVolume, float endVolume, float time, Ease easeType = Ease.Linear)
    {
        AudioSource audioSource = GetSource(name);
        audioSource.volume = startVolume;
        DOTween.To(() => audioSource.volume, x => audioSource.volume = x, endVolume, time).SetEase(easeType).SetUpdate(true);
    }

    public void FadeOut(string name, float endVolume, float time, Ease easeType = Ease.Linear)
    {
        AudioSource audioSource = GetSource(name);
        DOTween.To(() => audioSource.volume, x => audioSource.volume = x, endVolume, time).SetEase(easeType).SetUpdate(true);
    }

    private IEnumerator PlayWithDelay(Sound s, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!s.source.loop) s.source.PlayOneShot(s.clip, s.volume);
        else s.source.Play();
    }
}
