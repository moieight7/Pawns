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
    public UIText abilityChargeNum, abilityKeyText;

    private AbilityInfoPanel abilityInfoPanel;
    private Tween cooldownAnimation = null;

    private void Start()
    {
        abilityInfoPanel = FindObjectOfType<AbilityInfoPanel>();
    }

    public void OnHover()
    {
        abilityInfoPanel.SetAbility(ability);
        if (!abilityInfoPanel.IsVisible) abilityInfoPanel.Show();
    }

    public void OnHoverStop()
    {
        abilityInfoPanel.Hide();
    }

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

    public void SetAbilityKeyText()
    {
        if (ability != null)
        {
            if (ability.Type == AbilityType.Primary) abilityKeyText.SetText("M1");
            else if (ability.Type == AbilityType.Secondary) abilityKeyText.SetText("M2");
            else if (ability.Type == AbilityType.Utility) abilityKeyText.SetText("Shift");
            else if (ability.Type == AbilityType.Special) abilityKeyText.SetText("R");
            else if (ability.Type == AbilityType.Switch) abilityKeyText.SetText("Space");
        }
        else abilityKeyText.SetText("");
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
