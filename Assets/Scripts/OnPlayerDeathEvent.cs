using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class OnPlayerDeathEvent : MonoBehaviour
{
    public UltEvent OnPlayerDeath;

    private void Awake()
    {
        Entity.OnPlayerKilled += OnPlayerKilled;
    }

    private void OnPlayerKilled()
    {
        OnPlayerDeath.Invoke();
    }
}
