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
            playerAbility.ability.numberOfCharges = playerAbility.ability.MaxCharges;
        }

        if (entity.type == EntityType.Player) AbilityUI.instance.PopulateContainerList(entityAbilities);
        if (entity.type == EntityType.Player) AbilityUI.instance.SetAbilityUI();
    }

    private Ability FindAbilityByType(AbilityType type)
    {
        Ability playerAbility = entityAbilities.Find(x => x.Type == type);
        if (playerAbility == null) return null;
        else return playerAbility;
    }

    private void OnAbilityTriggered()
    {
        Debug.Log("OnAbilityTriggered");
        if (entity.type == EntityType.Player) AbilityUI.instance.SetAbilityUI();
    }

    private void OnAbilityChargeCooldown()
    {
        Debug.Log("OnAbilityChargeCooldown");
        if (entity.type == EntityType.Player) AbilityUI.instance.SetAbilityUI();
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
