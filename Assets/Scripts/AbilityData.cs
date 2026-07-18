using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

[CreateAssetMenu(fileName = "newAbilityData", menuName = "Data/Abilities/Ability Data")]
public class AbilityData : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite icon;
    public Color color = Color.white;
    public Color offColor = new Color(140, 140, 140, 129);
    public AbilityType type;

    public float inBetweenChargesCooldownTime;
    public float cooldownTime;

    public int maxCharges;

    public bool startUsable = true;

    public UltEvent OnAbilityTriggeredEvent, OnAbilityCoolDownEvent;
}
