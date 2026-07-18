using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    public List<PlayerAbility> playerAbilities;
    public KeyCode primaryKey, secondaryKey, utilityKey, specialKey;

    private PlayerAbility primary, secondary, utility, special;

    void Start()
    {
        Ability.OnAbilityTriggered += OnAbilityTriggered;
        Ability.OnAbilityChargeCooldown += OnAbilityChargeCooldown;

        SetAbilities();
    }

    void Update()
    {
        if (Input.GetKey(primaryKey)) { primary.TriggerAbility(); }
        if (Input.GetKey(secondaryKey)) { secondary.TriggerAbility(); }
        if (Input.GetKey(utilityKey)) { utility.TriggerAbility(); }
        if (Input.GetKey(specialKey)) { special.TriggerAbility(); }
    }

    public void SetAbilities()
    {
        primary = FindAbilityByType(AbilityType.Primary);
        secondary = FindAbilityByType(AbilityType.Secondary);
        utility = FindAbilityByType(AbilityType.Utility);
        special = FindAbilityByType(AbilityType.Special);

        if (primary != null) primary.caster = gameObject.GetComponent<Entity>();
        if (secondary != null) secondary.caster = gameObject.GetComponent<Entity>();
        if (utility != null) utility.caster = gameObject.GetComponent<Entity>();
        if (special != null) special.caster = gameObject.GetComponent<Entity>();

        if (primary != null) primary.numberOfCharges = primary.maxCharges;
        if (secondary != null) secondary.numberOfCharges = secondary.maxCharges;
        if (utility != null) utility.numberOfCharges = utility.maxCharges;
        if (special != null) special.numberOfCharges = special.maxCharges;

        AbilityUI.instance.PopulateContainerList(playerAbilities);
        AbilityUI.instance.SetAbilityUI();
    }

    private PlayerAbility FindAbilityByType(AbilityType type)
    {
        PlayerAbility playerAbility = playerAbilities.Find(x => x.type == type);
        if (playerAbility == null) return null;
        else return playerAbility;
    }

    private void OnAbilityTriggered()
    {
        Debug.Log("OnAbilityTriggered");
        AbilityUI.instance.SetAbilityUI();
    }

    private void OnAbilityChargeCooldown()
    {
        Debug.Log("OnAbilityChargeCooldown");
        AbilityUI.instance.SetAbilityUI();
    }
}

[System.Serializable]
public class PlayerAbility : Ability 
{
    
}
