using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : Attack
{
    private Collider2D trigger;

    private void Start()
    {
        trigger = GetComponent<Collider2D>();
        trigger.enabled = false;
    }

    public void EnableTrigger()
    {
        trigger.enabled = true;
    }

    public void DisableTrigger()
    {
        trigger.enabled = false;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
