using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityBehaviorManager
{
    public static string AbilityTriggerLog(string abilityName) { return abilityName + "triggered."; }
    public static string CooldownFinishLog(string abilityName) { return abilityName + "cooldown finished."; }

    public static void TestPrimary_UseEffect()
    {
        
    }
}
