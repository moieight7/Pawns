using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDashStateData", menuName = "Data/Enemy State Data/Dash State")]
public class D_DashState : ScriptableObject, IEnemyDataContainer
{
    public float dashForce = 1f;
    public float dashDuration = 0.4f;
}
