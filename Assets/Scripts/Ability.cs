using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UltEvents;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Ability
{
    [SerializeField] private AbilityData abilityData;

    [HideInInspector] public Entity caster;
    [HideInInspector] public int numberOfCharges;

    [HideInInspector] public List<IEnumerator> cooldownCoroutines = new List<IEnumerator>();

    public bool cooldownLoopRunning = false;

    private bool isCoolingDown = false;

    public bool usable = true;

    public Coroutine cooldownLoop;

    [SerializeField] public UltEvent OnAbilityTriggeredEvent, OnAbilityCoolDownEvent, OnAbilityCoolDownPlayerEvent;

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
        get => abilityData.UIcolor;
        private set => abilityData.UIcolor = value;
    }

    public Color OffColor
    {
        get => abilityData.UIoffColor;
        private set => abilityData.UIoffColor = value;
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

    public float EnemyCastDelayTime
    {
        get => abilityData.enemyCastDelayTime;
        private set => abilityData.enemyCastDelayTime = value;
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

    public Color TelegraphColor
    {
        get => abilityData.telegraphColor;
        private set => abilityData.telegraphColor = value;
    }

    public delegate void OnAbilityTriggeredAction(Entity entity);
    public static event OnAbilityTriggeredAction OnAbilityTriggered;

    public delegate void OnAbilityChargeCooldownAction(Entity entity);
    public static event OnAbilityChargeCooldownAction OnAbilityChargeCooldown;

    public void SetEvents()
    {
        OnAbilityTriggeredEvent.CopyFrom(abilityData.OnAbilityTriggeredEvent);
        OnAbilityCoolDownEvent.CopyFrom(abilityData.OnAbilityCoolDownEvent);
        OnAbilityCoolDownPlayerEvent.CopyFrom(abilityData.OnAbilityCoolDownPlayerEvent);
    }

    public void TriggerAbility() 
	{
		if (numberOfCharges <= 0 || !usable || PauseMenu.instance.Paused || Time.timeScale == 0) return;

        if (abilityData.maxCharges > 1) AbilityCooldownManager.instance.TriggerAbilityInBetweenChargesCooldown(this);
        AbilityCooldownManager.instance.QueueCooldown(this);

        numberOfCharges--;

        Debug.Log(AbilityBehaviorManager.AbilityTriggerLog(this));
        OnAbilityTriggered.Invoke(caster);

        Debug.Log(OnAbilityTriggeredEvent.ToString());

        List<PersistentCall> persistentCalls = new List<PersistentCall>();
        persistentCalls = OnAbilityTriggeredEvent.PersistentCallsList;

        foreach (PersistentCall call in persistentCalls) AbilityBehaviorManager.CreateCastAbility(caster, this, call); 
    }

    public void FinishAbilityCooldown(bool playCooldownEvents = true) 
	{
        Debug.Log(AbilityBehaviorManager.CooldownFinishLog(this));
        OnAbilityChargeCooldown.Invoke(caster);
        if (playCooldownEvents)
        {
            OnAbilityCoolDownEvent.Invoke();
            if (caster.type == EntityType.Player) { Debug.Log("StaticAudioPlayer OnAbilityCoolDownPlayerEvent " + caster.name + ", " + caster.type + ", " + Name); OnAbilityCoolDownPlayerEvent.Invoke(); }
        }
    }

    public void RefundAbilityCharge()
    {
        isCoolingDown = false;

        numberOfCharges++;
        numberOfCharges = Mathf.Clamp(numberOfCharges, 0, abilityData.maxCharges);

        FinishAbilityCooldown(false);
        AbilityCooldownManager.instance.CancelAbilityCooldown(this);
        cooldownCoroutines.Clear();

        Debug.Log("Ability cooldown end (" + Name + ")");
    }

    public IEnumerator CooldownLoop()
    {
        if (cooldownLoop != null) yield break;

        Debug.Log("Enter CooldownLoop() on " + Name + ", " + caster.name);
        cooldownLoopRunning = true;
        while (cooldownCoroutines.Count > 0)
        {
            Debug.Log("Enter CooldownLoop while on " + Name + ", " + caster.name);
            yield return new WaitUntil(() => cooldownCoroutines.Count > 0);
            Debug.Log("CooldownLoop " + Name + " passed first yield" + ", " + caster.name);
            if (cooldownCoroutines.Count > 0)
            {
                AbilityCooldownManager.instance.TriggerAbilityCooldown(cooldownCoroutines.First());
                cooldownCoroutines.Remove(cooldownCoroutines.First());
            }
            yield return new WaitUntil(() => isCoolingDown == false);
        }
        cooldownLoopRunning = false;
        cooldownLoop = null;
        Debug.Log("Exit CooldownLoop() on " + Name + ", " + caster.name);
    }

	public IEnumerator Cooldown()
	{
        Debug.Log("Ability cooldown start (" + Name + ") on " + caster.name);

        if (caster.type == EntityType.Player) AbilityUI.instance.CooldownAnimation(this);
        isCoolingDown = true;
        yield return new WaitForSeconds(abilityData.cooldownTime);
        if (cooldownLoop == null) yield break;
        isCoolingDown = false;

        numberOfCharges++;
        numberOfCharges = Mathf.Clamp(numberOfCharges, 0, abilityData.maxCharges);
        FinishAbilityCooldown();
        Debug.Log("Ability cooldown end (" + Name + ") on " + caster.name);
    }

    public IEnumerator InBetweenChargesCooldown()
    {
        usable = false;
        yield return new WaitForSeconds(abilityData.inBetweenChargesCooldownTime);
        usable = true;
    }

    public void ResetCooldown()
    {
        isCoolingDown = false;
        numberOfCharges = MaxCharges;

        FinishAbilityCooldown(false);
        AbilityCooldownManager.instance.CancelAbilityCooldown(this);

        if (caster.type == EntityType.Player) AbilityUI.instance.CancelCooldownAnimation(this);

        cooldownCoroutines.Clear();
    }

    public void StopCooldown()
    {
        isCoolingDown = false;
        cooldownLoopRunning = false;
        foreach (IEnumerator coroutine in cooldownCoroutines)
        {
            AbilityCooldownManager.instance.CancelAbilityCooldown(this);
            AbilityCooldownManager.instance.StopAbilityCooldown(this);
        }
        cooldownCoroutines.Clear();
    }

    public void OnRemove()
    {
        AbilityCooldownManager.instance.CancelAbilityCooldown(this);
        cooldownCoroutines.Clear();
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
