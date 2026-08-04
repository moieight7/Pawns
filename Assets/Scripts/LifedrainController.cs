using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LifedrainController
{
    public static void EnablePlayerLifedrain(bool set)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().SetLifedrainFlag(set);
    }
}
