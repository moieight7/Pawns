using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public UltEvent ultEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ultEvent.Invoke();
    }
}
