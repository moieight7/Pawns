using System;
using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Ability
{
    [SerializeField] private AbilityData abilityData;

    [HideInInspector] public Entity caster;
    [HideInInspector] public int numberOfCharges;

    private bool isCoolingDown = false;

	[SerializeField] private bool usable = true;

    public string Name
    {
        get => abilityData.name;
        private set => abilityData.name = value;
    }

    public string Description
    {
        get => abilityData.description;
        private set => abilityData.description = value;
    }

    public Sprite Icon
    {
        get => abilityData.icon;
        private set => abilityData.icon = value;
    }

    public Color Color
    {
        get => abilityData.color;
        private set => abilityData.color = value;
    }

    public Color OffColor
    {
        get => abilityData.offColor;
        private set => abilityData.offColor = value;
    }

    public AbilityType Type
    {
        get => abilityData.type;
        private set => abilityData.type = value;
    }

    public float CooldownTime
    {
        get => abilityData.cooldownTime;
        private set => abilityData.cooldownTime = value;
    }

    public int MaxCharges
    {
        get => abilityData.maxCharges;
        private set => abilityData.maxCharges = value;
    }

    public bool StartUsable
    {
        get => abilityData.startUsable;
        private set => abilityData.startUsable = value;
    }

    public delegate void OnAbilityTriggeredAction();
    public static event OnAbilityTriggeredAction OnAbilityTriggered;

    public delegate void OnAbilityChargeCooldownAction();
    public static event OnAbilityChargeCooldownAction OnAbilityChargeCooldown;

    public void TriggerAbility() 
	{
		if ((numberOfCharges <= 0 || !usable)) return;

		numberOfCharges--;
        if (abilityData.maxCharges > 1) AbilityCooldownManager.instance.TriggerAbilityInBetweenChargesCooldown(this);
        AbilityCooldownManager.instance.TriggerAbilityCooldown(this);

        Debug.Log(AbilityBehaviorManager.AbilityTriggerLog(this));
        OnAbilityTriggered.Invoke();

        Debug.Log(abilityData.OnAbilityTriggeredEvent.ToString());

        List<PersistentCall> persistentCalls = new List<PersistentCall>();
        persistentCalls = abilityData.OnAbilityTriggeredEvent.PersistentCallsList;

        foreach (PersistentCall call in persistentCalls) { Debug.Log(call.ToString()); AbilityBehaviorManager.CreateCastAbility(caster, this, call); }
    }

    public void FinishAbilityCooldown() 
	{
        Debug.Log(AbilityBehaviorManager.CooldownFinishLog(this));
        OnAbilityChargeCooldown.Invoke();
        abilityData.OnAbilityCoolDownEvent.Invoke();
    }

	public IEnumerator Cooldown()
	{
        Debug.Log("Ability cooldown start");

        yield return new WaitUntil(() => isCoolingDown == false);

        if (caster.type == EntityType.Player) AbilityUI.instance.CooldownAnimation(this);
        isCoolingDown = true;
        yield return new WaitForSeconds(abilityData.cooldownTime);
        isCoolingDown = false;

        numberOfCharges++;
        numberOfCharges = Mathf.Clamp(numberOfCharges, 0, abilityData.maxCharges);
        FinishAbilityCooldown();
        Debug.Log("Ability cooldown end");
    }

    public IEnumerator InBetweenChargesCooldown()
    {
        usable = false;
        yield return new WaitForSeconds(abilityData.inBetweenChargesCooldownTime);
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
