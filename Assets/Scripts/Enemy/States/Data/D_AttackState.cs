using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAttackStateData", menuName = "Data/Enemy State Data/Attack State")]
public class D_AttackState : ScriptableObject, IEnemyDataContainer
{
    public float damage;
}
