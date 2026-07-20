using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newChargeStateData", menuName = "Data/Enemy State Data/Charge State")]
public class D_ChargeState : ScriptableObject, IEnemyDataContainer
{
    public float collisionDamage;
    public float chargeSpeedMultiplier = 2.5f;
    public float windupTime = 2f;
    public float onPlayerHitStaggerTime = 2f;
    public float onWallHitStaggerTime = 4f;
    public float onLedgeHitStaggerTime = 2f;
    public float closeAttackHitboxLifetime = 0.5f;
}
