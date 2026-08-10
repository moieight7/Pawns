using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSoundBankMasterData", menuName = "Data/Audio/Sound Bank Master Data")]
public class SoundBankMaster : ScriptableObject
{
    public List<SoundBank> soundBanks;
    public Sound errorAudio;

    public Sound GetClip(string clipName)
    {
        Sound s = null;
        foreach (SoundBank soundBank in soundBanks)
        {
            foreach (Sound sound in soundBank.sounds)
            {
                s = Array.Find(soundBank.sounds.ToArray(), sound => sound.name == clipName);
                if (s != null) return s;
            }
        }

        Debug.LogError("Could not find " + clipName + " in the sound database.");
        return errorAudio;
    }
}
