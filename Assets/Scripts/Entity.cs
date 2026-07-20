using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float health;
    public EntityType type;

    public Transform firePoint;

    public delegate void EnemyKilledAction();
    public static event EnemyKilledAction OnEnemyKilled;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0) Die();
    }

    private void Die()
    {
        if (type == EntityType.Enemy && OnEnemyKilled != null) OnEnemyKilled.Invoke();

        Destroy(gameObject);
    }
}

public enum EntityType
{
    None,
    Player,
    Enemy
}
