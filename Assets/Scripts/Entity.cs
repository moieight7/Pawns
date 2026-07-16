using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int health;
    public EntityType type;

    public Transform firePoint;
}

public enum EntityType
{
    None,
    Player,
    Enemy
}
