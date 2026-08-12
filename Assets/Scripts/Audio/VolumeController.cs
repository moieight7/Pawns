using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    public AudioMixer mixer;
    public string parameter;
    public float volume;

    public static VolumeController instance { get; private set; }

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one VolumeController object! Destroying the newest one.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        mixer.GetFloat(parameter, out volume);
    }
}
