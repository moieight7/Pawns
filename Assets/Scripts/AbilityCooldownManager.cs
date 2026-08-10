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
        Debug.Log("QueueCooldown on " + ability.Name);
        ability.cooldownCoroutines.Add(ability.Cooldown());
        if (!ability.cooldownLoopRunning) { ability.cooldownLoop = StartCoroutine(ability.CooldownLoop()); }
        else Debug.Log("QueueCooldown on " + ability.Name + " - cooldownLoopRunning");
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
        Debug.Log("CancelAbilityCooldown on " + ability.Name);
        if (ability.cooldownLoopRunning) { Debug.Log("CancelAbilityCooldown stopCooldownLoop"); StopCoroutine(ability.CooldownLoop()); ability.cooldownLoop = null; }
        StopCoroutine(ability.Cooldown());
    }

    public void ResetAbilityCooldown(Ability ability)
    {
        ability.ResetCooldown();
    }
}
