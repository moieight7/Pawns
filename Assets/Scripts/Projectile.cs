using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class Projectile : Attack
{
    [Header("Projectile Stats")]
    public float speed;

    [HideInInspector] public Vector2 moveDir;
    protected Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void SetDirection(Vector2 direction, float rotationZ)
    {
        base.SetDirection(direction, rotationZ);
        rb.velocity = direction * speed;
        moveDir = rb.velocity.normalized;
    }

    public void RotateDirection(Quaternion rotation)
    {
        Vector2 currentDirection = rb.velocity;
        Vector2 newDirection = rotation * currentDirection;
        rb.velocity = newDirection;
        moveDir = rb.velocity.normalized;
    }
}
