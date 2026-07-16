using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    public List<PlayerAbility> playerAbilities;
    public KeyCode primaryKey, secondaryKey, utilityKey, specialKey;

    public PlayerAbility primary, secondary, utility, special;

    [Header("UI")]
    public Image primaryAbilityIcon;
    public Image secondaryAbilityIcon, utilityAbilityIcon, specialAbilityIcon;
    public Image primaryAbilityBorder, secondaryAbilityBorder, utilityAbilityBorder, specialAbilityBorder;
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

        if (primary != null) primary.numberOfCharges = primary.maxCharges;
        if (secondary != null) secondary.numberOfCharges = secondary.maxCharges;
        if (utility != null) utility.numberOfCharges = utility.maxCharges;
        if (special != null) special.numberOfCharges = special.maxCharges;

        SetAbilityNumText();
        SetAbilityIcons();
    }

    private void SetAbilityNumText()
    {
        if (primary != null) primaryAbilityChargeNum.text = primary.numberOfCharges.ToString();
        else primaryAbilityChargeNum.text = "";
        if (secondary != null) secondaryAbilityChargeNum.text = secondary.numberOfCharges.ToString();
        else secondaryAbilityChargeNum.text = "";
        if (utility != null) utilityAbilityChargeNum.text = utility.numberOfCharges.ToString();
        else utilityAbilityChargeNum.text = "";
        if (special != null) specialAbilityChargeNum.text = special.numberOfCharges.ToString();
        else specialAbilityChargeNum.text = "";
    }

    private void SetAbilityIcons()
    {
        if (primary != null) { primaryAbilityIcon.enabled = true; primaryAbilityBorder.enabled = true; primaryAbilityIcon.sprite = primary.icon; }
        else { primaryAbilityIcon.enabled = false; primaryAbilityBorder.enabled = false; }
        if (secondary != null) { secondaryAbilityIcon.enabled = true; secondaryAbilityIcon.sprite = secondary.icon; }
        else { secondaryAbilityIcon.enabled = false; secondaryAbilityBorder.enabled = false; }
        if (utility != null) { utilityAbilityIcon.enabled = true; utilityAbilityIcon.sprite = utility.icon; }
        else { utilityAbilityIcon.enabled = false; utilityAbilityBorder.enabled = false; }
        if (special != null) { specialAbilityIcon.enabled = true; specialAbilityIcon.sprite = special.icon; }
        else { specialAbilityIcon.enabled = false; specialAbilityBorder.enabled = false; }
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
        SetAbilityIcons();
    }

    private void OnAbilityChargeCooldown()
    {
        Debug.Log("OnAbilityChargeCooldown");
        SetAbilityNumText();
        SetAbilityIcons();
    }
}

[System.Serializable]
public class PlayerAbility : Ability { }
