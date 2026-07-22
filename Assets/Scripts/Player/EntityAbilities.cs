using IngameDebugConsole;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityAbilities : MonoBehaviour
{
    public List<Ability> entityAbilities;
    public KeyCode primaryKey, secondaryKey, utilityKey, specialKey, switchKey;

    [SerializeField] private List<PlayerAbility> playerAbilities = new List<PlayerAbility>();

    private Entity entity;

    private bool canUseAbilites = true;

    void Start()
    {
        entity = GetComponent<Entity>();

        Ability.OnAbilityTriggered += OnAbilityTriggered;
        Ability.OnAbilityChargeCooldown += OnAbilityChargeCooldown;
        Entity.OnPlayerKilled += OnPlayerKilled;

        SetAbilities();

        DebugLogConsole.AddCommand("ability_refresh", "Resets cooldowns for every player ability.", ResetAllCooldowns);
    }

    void Update()
    {
        if (!canUseAbilites) return;

        if (entity.type == EntityType.Player) {
            foreach (PlayerAbility playerAbility in playerAbilities)
            {
                if (Input.GetKey(playerAbility.keyCode) && playerAbility.ability.Type != AbilityType.Switch) playerAbility.ability.TriggerAbility();
                else if (Input.GetKeyDown(playerAbility.keyCode) && playerAbility.ability.Type == AbilityType.Switch) playerAbility.ability.TriggerAbility();
            }
        }
    }

    public void SetAbilities()
    {
        if (playerAbilities.Count > 0) playerAbilities.Clear();
        if (FindAbilityByType(AbilityType.Primary) != null) playerAbilities.Add(new PlayerAbility(FindAbilityByType(AbilityType.Primary), primaryKey, AbilityType.Primary));
        if (FindAbilityByType(AbilityType.Secondary) != null) playerAbilities.Add(new PlayerAbility(FindAbilityByType(AbilityType.Secondary), secondaryKey, AbilityType.Secondary));
        if (FindAbilityByType(AbilityType.Utility) != null) playerAbilities.Add(new PlayerAbility(FindAbilityByType(AbilityType.Utility), utilityKey, AbilityType.Utility));
        if (FindAbilityByType(AbilityType.Special) != null) playerAbilities.Add(new PlayerAbility(FindAbilityByType(AbilityType.Special), specialKey, AbilityType.Special));
        if (FindAbilityByType(AbilityType.Switch) != null) playerAbilities.Add(new PlayerAbility(FindAbilityByType(AbilityType.Switch), switchKey, AbilityType.Switch));

        foreach (PlayerAbility playerAbility in playerAbilities)
        {
            playerAbility.ability.caster = gameObject.GetComponent<Entity>();
            
            if (playerAbility.ability.StartUsable) playerAbility.ability.numberOfCharges = playerAbility.ability.MaxCharges;
            else { playerAbility.ability.numberOfCharges = 0; AbilityCooldownManager.instance.QueueCooldown(playerAbility.ability); }

            playerAbility.ability.SetEvents();
        }

        if (entity.type == EntityType.Player) AbilityUI.instance.PopulateContainerList(entityAbilities);
        if (entity.type == EntityType.Player) AbilityUI.instance.SetAbilityUI();
    }

    public void TriggerAbilityByIndex(int index)
    {
        playerAbilities[index].ability.TriggerAbility();
    }

    private Ability FindAbilityByType(AbilityType type)
    {
        Ability playerAbility = entityAbilities.Find(x => x.Type == type);
        if (playerAbility == null) return null;
        else return playerAbility;
    }

    public void AddAbility(Ability ability)
    {
        entityAbilities.Add(ability);
        if (!ability.StartUsable) ability.numberOfCharges = 0;
    }

    public void RemoveAbility(AbilityType type)
    {
        Ability abilityToRemove = FindAbilityByType(type);
        abilityToRemove.OnRemove();
        entityAbilities.Remove(abilityToRemove);
    }

    private void OnAbilityTriggered(Entity entity)
    {
        Debug.Log("OnAbilityTriggered");
        if (entity.type == EntityType.Player) AbilityUI.instance.SetAbilityUI();
    }

    private void OnAbilityChargeCooldown(Entity entity)
    {
        Debug.Log("OnAbilityChargeCooldown");
        if (entity.type == EntityType.Player) AbilityUI.instance.SetAbilityUI();
    }

    private void OnPlayerKilled()
    {
        canUseAbilites = false;
    }

    private void ResetAllCooldowns()
    {
        foreach (PlayerAbility playerAbility in playerAbilities) playerAbility.ability.ResetCooldown();
    }
}

[System.Serializable]
public class PlayerAbility 
{
    public Ability ability;
    public KeyCode keyCode;
    public AbilityType type;

    public PlayerAbility(Ability ability, KeyCode keyCode, AbilityType type)
    {
        this.ability = ability;
        this.keyCode = keyCode;
        this.type = type;
    }
}
