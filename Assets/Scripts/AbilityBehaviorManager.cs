using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityBehaviorManager
{
    public static string AbilityTriggerLog(Ability ability) { return ability.name + "triggered. " + NumberOfChargesLog(ability); }
    public static string CooldownFinishLog(Ability ability) { return ability.name + "cooldown finished. " + NumberOfChargesLog(ability); }
    public static string NumberOfChargesLog(Ability ability) { return "Number of charges: " + ability.numberOfCharges + " / " + ability.maxCharges; }

    public static void TestPrimary_UseEffect()
    {
        
    }
}
