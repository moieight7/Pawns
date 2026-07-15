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

    public void TriggerAbilityCooldown(Ability ability)
    {
        StartCoroutine(ability.Cooldown());
    }

    public void TriggerAbilityInBetweenChargesCooldown(Ability ability)
    {
        StartCoroutine(ability.InBetweenChargesCooldown());
    }
}
