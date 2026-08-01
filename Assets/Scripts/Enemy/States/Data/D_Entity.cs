using System.Collections.Generic;
using UnityEngine;

//Enemy data
[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Enemy Data")]
public class D_Entity : ScriptableObject, IEnemyDataContainer
{
    public float health = 100;
    public float iFrameTime = 1.3f;
    public float playerMovementSpeed = 6;
    public float navMeshAgentMovementSpeed = 6;
    public bool seeThroughObstacles = false;

    public float playerMinCheckDist = 3f;
    public float playerMaxCheckDist = 5f;

    public float closeRangeDist = 1f;
    public float circleCastCheckRadius = 1f;

    public float rangeCheckOffset = 0.2f;
    [Range(0.001f, 0.1f)] public float knockbackStillThreshold = 0.05f;

    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsDanger;
    public LayerMask whatIsFriendly;

    public AudioClip spawnSound, hurtSound, deathSound;

    public List<Ability> entityAbilities;
}
