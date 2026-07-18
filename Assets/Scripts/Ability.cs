using System;
using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;
using UnityEngine.UIElements;

public class Ability
{
	public string name;
	public string description;
    public Sprite icon;
    public Color color = Color.white;
    public Color offColor = new Color(140, 140, 140, 129);
    public AbilityType type;

	public float inBetweenChargesCooldownTime;
	public float cooldownTime;

	public int numberOfCharges;
	public int maxCharges;

    [HideInInspector] public Entity caster;

    private bool isCoolingDown = false;

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
		if ((numberOfCharges <= 0 || !usable)) return;

		numberOfCharges--;
        if (maxCharges > 1) AbilityCooldownManager.instance.TriggerAbilityInBetweenChargesCooldown(this);
        AbilityCooldownManager.instance.TriggerAbilityCooldown(this);

        Debug.Log(AbilityBehaviorManager.AbilityTriggerLog(this));
        OnAbilityTriggered.Invoke();

        Debug.Log(OnAbilityTriggeredEvent.ToString());

        List<PersistentCall> persistentCalls = new List<PersistentCall>();
        persistentCalls = OnAbilityTriggeredEvent.PersistentCallsList;

        foreach (PersistentCall call in persistentCalls) { Debug.Log(call.ToString()); AbilityBehaviorManager.CreateCastAbility(caster, this, call); }
    }

    public void FinishAbilityCooldown() 
	{
        Debug.Log(AbilityBehaviorManager.CooldownFinishLog(this));
        OnAbilityChargeCooldown.Invoke();
        OnAbilityCoolDownEvent.Invoke();
    }

	public IEnumerator Cooldown()
	{
        Debug.Log("Ability cooldown start");

        yield return new WaitUntil(() => isCoolingDown == false);

        AbilityUI.instance.CooldownAnimation(this);
        isCoolingDown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCoolingDown = false;

        numberOfCharges++;
        numberOfCharges = Mathf.Clamp(numberOfCharges, 0, maxCharges);
        FinishAbilityCooldown();
        Debug.Log("Ability cooldown end");
    }

    public IEnumerator InBetweenChargesCooldown()
    {
        usable = false;
        yield return new WaitForSeconds(inBetweenChargesCooldownTime);
        usable = true;
    }
}

public enum AbilityType
{
	None,
	Primary,
	Secondary,
	Utility,
	Special,
    Switch
}
