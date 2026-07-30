using DG.Tweening;
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
    public UIText abilityChargeNum;

    private Tween cooldownAnimation = null;

    public void SetAbilityNumText()
    {
        if (ability != null) abilityChargeNum.SetText(ability.numberOfCharges.ToString());
        else abilityChargeNum.SetText("");
    }

    public void SetAbilityIcons()
    {
        if (ability != null) { abilityIcon.enabled = true; abilityBorder.enabled = true; abilityIcon.sprite = ability.Icon; }
        else { abilityIcon.enabled = false; abilityBorder.enabled = false; }
    }

    public void CooldownAnimation()
    {
        abilityIcon.fillAmount = 0;
        cooldownAnimation = abilityIcon.DOFillAmount(1, ability.CooldownTime).SetEase(Ease.Linear);
    }

    public void CancelCooldownAnimation()
    {
        abilityIcon.fillAmount = 1;
        cooldownAnimation.Complete();
    }

    private void OnDestroy()
    {
        CancelCooldownAnimation();
    }
}
