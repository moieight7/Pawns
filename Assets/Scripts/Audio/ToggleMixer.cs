using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ToggleMixer : MonoBehaviour
{
    public AudioMixer mixer;
    public string parameter;

    public Image buttonImage, disabledImage;
    private float startVolume = 0;

    private void Start()
    {
        float volume = 0f;
        mixer.GetFloat(parameter, out volume);

        if (volume != -80)
        {
            disabledImage.gameObject.SetActive(false);
            buttonImage.color = new Color(255, 255, 255);
        }
        else
        {
            disabledImage.gameObject.SetActive(true);
            buttonImage.color = new Color(170, 170, 170);
        }
    }

    public void Toggle()
    {
        Debug.Log("ToggleMixer toggle ");

        float volume = 0f;
        mixer.GetFloat(parameter, out volume);

        if (volume != -80)
        {
            mixer.SetFloat(parameter, -80);
            disabledImage.gameObject.SetActive(true);
            buttonImage.color = new Color(170, 170, 170);
            Debug.Log("ToggleMixer toggle turn off");
        }
        else if (volume == -80)
        { 
            mixer.SetFloat(parameter, startVolume);
            disabledImage.gameObject.SetActive(false);
            buttonImage.color = new Color(255, 255, 255);
            Debug.Log("ToggleMixer toggle turn on");
        }
    }
}
