using System;
using System.Collections;
using UltEvents;
using UnityEngine;
using UnityEngine.UIElements;

public class Ability
{
	public string name;
	public string description;
	public AbilityType type;

	public float inBetweenChargesCooldownTime;
	public float cooldownTime;

	public int numberOfCharges;
	public int maxCharges;

	public UltEvent OnAbilityTriggeredEvent, OnAbilityCoolDownEvent;

	[SerializeField] private bool usable = true;
    public bool GetUsable() { return usable; }

    /*public string DisplayName { get { SetName(true); return name; } private set { } }
    public string TooltipName { get { SetName(false); return name; } private set { } }
    public string Description { get { SetDescription(); return description; } private set { } }*/

    public Ability() { }

    public delegate void OnAbilityTriggeredAction();
    public static event OnAbilityTriggeredAction OnAbilityTriggered;

    public delegate void OnAbilityChargeCooldownAction();
    public static event OnAbilityChargeCooldownAction OnAbilityChargeCooldown;

    public void TriggerAbility() 
	{
		if (numberOfCharges <= 0 || !usable) return;

		numberOfCharges--;
        if (maxCharges > 1) AbilityCooldownManager.instance.TriggerAbilityInBetweenChargesCooldown(this);
        AbilityCooldownManager.instance.TriggerAbilityCooldown(this);

        Debug.Log(AbilityBehaviorManager.AbilityTriggerLog(this));
        OnAbilityTriggered.Invoke();
        OnAbilityTriggeredEvent.Invoke();
    }

    public void FinishAbilityCooldown() 
	{
        Debug.Log(AbilityBehaviorManager.CooldownFinishLog(this));
        OnAbilityChargeCooldown.Invoke();
        OnAbilityCoolDownEvent.Invoke();
    }

	public IEnumerator Cooldown()
	{
		yield return new WaitForSeconds(cooldownTime);
        numberOfCharges++;
        numberOfCharges = Mathf.Clamp(numberOfCharges, 0, maxCharges);
        FinishAbilityCooldown();
    }

    public IEnumerator InBetweenChargesCooldown()
    {
        usable = false;
        yield return new WaitForSeconds(inBetweenChargesCooldownTime);
        if (numberOfCharges > 0) usable = true;
    }
}

public enum AbilityType
{
	None,
	Primary,
	Secondary,
	Utility,
	Special
}
