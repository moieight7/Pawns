using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/Enemy State Data/Idle State")]
public class D_TeleportState : ScriptableObject, IEnemyDataContainer
{
    public float teleportRadius;
}
