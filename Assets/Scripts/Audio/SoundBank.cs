using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSoundBankData", menuName = "Data/Audio/Sound Bank Data")]
public class SoundBank : ScriptableObject
{
    public List<Sound> sounds;
}
