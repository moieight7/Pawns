using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/Enemy State Data/Stagger State")]
public class D_StaggerState : ScriptableObject, IEnemyDataContainer
{
    public float minStaggerTime = 1f;
    public float maxStaggerTime = 2f;
}
