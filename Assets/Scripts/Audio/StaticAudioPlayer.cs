using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticAudioPlayer
{
    public static void Play(string name)
    {
        AudioManager.instance.Play(name);
    }

    public static void Play(string name, float volume)
    {
        AudioManager.instance.Play(name, volume);
    }

    public static void Play(string name, float volume, float pitch)
    {
        Debug.Log("StaticAudioPlayer Play " + name);
        AudioManager.instance.Play(name, volume, pitch);
    }

    public static void StopLoopingAudio(string name)
    {
        AudioManager.instance.StopLoopingAudio(name);
    }

    public static void FadeIn(string name, float startVolume, float endVolume, float time, Ease easeType = Ease.Linear)
    {
        AudioManager.instance.FadeIn(name, startVolume, endVolume, time, easeType);
    }

    public static void FadeOut(string name, float endVolume, float time, Ease easeType = Ease.Linear)
    {
        AudioManager.instance.FadeOut(name, endVolume, time, easeType);
    }
}
