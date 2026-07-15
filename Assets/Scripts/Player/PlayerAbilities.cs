using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public List<PlayerAbility> playerAbilities;
    public KeyCode primaryKey, secondaryKey, utilityKey, specialKey;

    public PlayerAbility primary, secondary, utility, special;

    void Start()
    {
        SetAbilities();
    }

    void Update()
    {
        if (Input.GetKey(primaryKey)) { primary.TriggerAbility(); }
        if (Input.GetKey(secondaryKey)) { secondary.TriggerAbility(); }
        if (Input.GetKey(utilityKey)) { utility.TriggerAbility(); }
        if (Input.GetKey(specialKey)) { special.TriggerAbility(); }
    }

    public void SetAbilities()
    {
        primary = FindAbilityByType(AbilityType.Primary);
        secondary = FindAbilityByType(AbilityType.Secondary);
        utility = FindAbilityByType(AbilityType.Utility);
        special = FindAbilityByType(AbilityType.Special);
    }

    private PlayerAbility FindAbilityByType(AbilityType type)
    {
        PlayerAbility playerAbility = playerAbilities.Find(x => x.type == type);
        if (playerAbility == null) return null;
        else return playerAbility;
    }
}

[System.Serializable]
public class PlayerAbility : Ability { }
