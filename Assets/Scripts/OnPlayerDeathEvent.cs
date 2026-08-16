using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class OnPlayerDeathEvent : MonoBehaviour
{
    public UltEvent OnPlayerDeath;

    private void OnEnable()
    {
        Entity.OnPlayerKilled += OnPlayerKilled;
    }

    private void OnDisable()
    {
        Entity.OnPlayerKilled -= OnPlayerKilled;
    }

    private void OnPlayerKilled()
    {
        OnPlayerDeath.Invoke();
    }
}
