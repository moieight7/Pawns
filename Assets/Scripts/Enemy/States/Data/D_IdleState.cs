using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/Enemy State Data/Idle State")]
public class D_IdleState : ScriptableObject, IEnemyDataContainer
{
    public float minIdleTime = 1f;
    public float maxIdleTime = 2f;
}
