using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerTeleporter
{
    public static void TeleportTo(Transform point)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().transform.position = point.position;
    }
}
