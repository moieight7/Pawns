using UnityEngine;

[CreateAssetMenu(fileName = "newMoveStateData", menuName = "Data/Enemy State Data/Move State")]
public class D_MoveState : ScriptableObject, IEnemyDataContainer
{
    public float moveSpeed = 6f;
    public float acceleration = 80f;
    public float stoppingDistance = 1.9f;
}
