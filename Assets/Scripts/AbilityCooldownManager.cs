using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCooldownManager : MonoBehaviour
{
    public static AbilityCooldownManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("An instance of AbilityCooldownManager already exists. Deleting the newest one...");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void QueueCooldown(Ability ability)
    {
        ability.cooldownCoroutines.Add(ability.Cooldown());
        if (!ability.cooldownLoopRunning) StartCoroutine(ability.CooldownLoop());
    }

    public void TriggerAbilityCooldown(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void TriggerAbilityInBetweenChargesCooldown(Ability ability)
    {
        StartCoroutine(ability.InBetweenChargesCooldown());
    }

    public void CancelAbilityCooldown(Ability ability)
    {
        if (ability.cooldownLoopRunning) StopCoroutine(ability.CooldownLoop());
        StopCoroutine(ability.Cooldown());
    }

    public void ResetAbilityCooldown(Ability ability)
    {
        ability.ResetCooldown();
    }
}
