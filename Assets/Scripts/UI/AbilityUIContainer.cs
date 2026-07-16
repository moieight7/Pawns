using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIContainer : MonoBehaviour
{
    public Ability ability;

    public Image abilityIcon, abilityOffIcon;
    public Image abilityBorder;
    public TextMeshProUGUI abilityChargeNum;

    public void SetAbilityNumText()
    {
        if (ability != null) abilityChargeNum.text = ability.numberOfCharges.ToString();
        else abilityChargeNum.text = "";
    }

    public void SetAbilityIcons()
    {
        if (ability != null) { abilityIcon.enabled = true; abilityBorder.enabled = true; abilityIcon.sprite = ability.icon; }
        else { abilityIcon.enabled = false; abilityBorder.enabled = false; }
    }
}
