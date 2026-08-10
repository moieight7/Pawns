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
}
