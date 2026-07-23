using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class AnimationUltEvent : MonoBehaviour
{
    public UltEvent ultEvent;

    public void InvokeUltEvent()
    {
        ultEvent.Invoke();
    }
}
