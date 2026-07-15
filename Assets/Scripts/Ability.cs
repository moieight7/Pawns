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

	public float cooldownTime;

	public int maxCharges;

	public UltEvent OnAbilityTriggered, OnAbilityCoolDown;

	private bool usable = true;
    public bool GetUsable() { return usable; }

    /*public string DisplayName { get { SetName(true); return name; } private set { } }
    public string TooltipName { get { SetName(false); return name; } private set { } }
    public string Description { get { SetDescription(); return description; } private set { } }*/

    public Ability() { }

	public void TriggerAbility() 
	{
		Debug.Log(AbilityBehaviorManager.AbilityTriggerLog(name));
        OnAbilityTriggered.Invoke();
        AbilityCooldownManager.instance.TriggerAbilityCooldown(this);
    }

    public void FinishAbilityCooldown() 
	{
        Debug.Log(AbilityBehaviorManager.CooldownFinishLog(name));
        OnAbilityCoolDown.Invoke();
    }

	public IEnumerator Cooldown()
	{
		usable = false;
		yield return new WaitForSeconds(cooldownTime);
		usable = true;
		FinishAbilityCooldown();
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
