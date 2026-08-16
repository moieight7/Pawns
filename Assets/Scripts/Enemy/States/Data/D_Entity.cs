using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Enemy Data")]
public class D_Entity : ScriptableObject, IEnemyDataContainer
{
    public float health = 100;
    public float lifedrainDelay = 1.5f;
    public float lifedrainDuration = 30;
    public float iFrameTime = 1.3f;
    public float playerMovementSpeed = 6;
    public float navMeshAgentMovementSpeed = 6;
    public bool seeThroughObstacles = false;
    public bool flipX = true;

    public float playerMinCheckDist = 3f;
    public float playerMaxCheckDist = 5f;

    public float closeRangeDist = 1f;
    public float circleCastCheckRadius = 1f;

    public float rangeCheckOffset = 0.2f;

    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsDanger;
    public LayerMask whatIsFriendly;
}
