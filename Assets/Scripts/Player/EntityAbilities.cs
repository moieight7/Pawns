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

    void Start()
    {
        entity = GetComponent<Entity>();

        Ability.OnAbilityTriggered += OnAbilityTriggered;
        Ability.OnAbilityChargeCooldown += OnAbilityChargeCooldown;

        SetAbilities();

        DebugLogConsole.AddCommand("ability_refresh", "Resets cooldowns for every player ability.", ResetAllCooldowns);
    }

    void Update()
    {
        if (entity.type == EntityType.Player) {
            foreach (PlayerAbility playerAbility in playerAbilities)
            {
                if (Input.GetKey(playerAbility.keyCode)) playerAbility.ability.TriggerAbility();
            }
        }
    }

    public void SetAbilities()
    {
        if (FindAbilityByType(AbilityType.Primary) != null) playerAbilities.Add(new PlayerAbility(FindAbilityByType(AbilityType.Primary), primaryKey));
        if (FindAbilityByType(AbilityType.Secondary) != null) playerAbilities.Add(new PlayerAbility(FindAbilityByType(AbilityType.Secondary), secondaryKey));
        if (FindAbilityByType(AbilityType.Utility) != null) playerAbilities.Add(new PlayerAbility(FindAbilityByType(AbilityType.Utility), utilityKey));
        if (FindAbilityByType(AbilityType.Special) != null) playerAbilities.Add(new PlayerAbility(FindAbilityByType(AbilityType.Special), specialKey));
        if (FindAbilityByType(AbilityType.Switch) != null) playerAbilities.Add(new PlayerAbility(FindAbilityByType(AbilityType.Switch), switchKey));

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

    public PlayerAbility(Ability ability, KeyCode keyCode)
    {
        this.ability = ability;
        this.keyCode = keyCode;
    }
}
