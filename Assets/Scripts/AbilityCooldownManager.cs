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
        if (ability.cooldownLoop == null) { ability.cooldownLoop = StartCoroutine(ability.CooldownLoop());  }
    }

    public Coroutine TriggerAbilityCooldown(Ability ability, IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    public void TriggerAbilityInBetweenChargesCooldown(Ability ability)
    {
        StartCoroutine(ability.InBetweenChargesCooldown());
    }

    public void CancelAbilityCooldown(Ability ability)
    {
        ability.cooldownCoroutines.Clear();
        if (ability.cooldownLoop != null) StopCoroutine(ability.cooldownLoop); ability.cooldownLoop = null;
        if (ability.cooldown != null) StopCoroutine(ability.cooldown);
    }

    public void StopAbilityCooldown(Ability ability)
    {
        ability.cooldownLoop = null;
    }

    public void ResetAbilityCooldown(Ability ability)
    {
        ability.ResetCooldown();
    }
}
