using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public List<PlayerAbility> playerAbilities;
    public KeyCode primaryKey, secondaryKey, utilityKey, specialKey;

    public PlayerAbility primary, secondary, utility, special;

    public TextMeshProUGUI primaryAbilityChargeNum, secondaryAbilityChargeNum, utilityAbilityChargeNum, specialAbilityChargeNum;

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

        primary.numberOfCharges = primary.maxCharges;
        secondary.numberOfCharges = secondary.maxCharges;
        utility.numberOfCharges = utility.maxCharges;
        special.numberOfCharges = special.maxCharges;

        SetAbilityNumText();
    }

    private void SetAbilityNumText()
    {
        primaryAbilityChargeNum.text = primary.numberOfCharges.ToString();
        secondaryAbilityChargeNum.text = secondary.numberOfCharges.ToString();
        utilityAbilityChargeNum.text = utility.numberOfCharges.ToString();
        specialAbilityChargeNum.text = special.numberOfCharges.ToString();
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
        SetAbilityNumText();
    }

    private void OnAbilityChargeCooldown()
    {
        Debug.Log("OnAbilityChargeCooldown");
        SetAbilityNumText();
    }
}

[System.Serializable]
public class PlayerAbility : Ability { }
