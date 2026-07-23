using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class Projectile : Attack
{
    public float speed;

    protected Vector2 moveDir;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void SetDirection(Vector2 direction, float rotationZ)
    {
        base.SetDirection(direction, rotationZ);
        rb.velocity = direction * speed;
    }

    public void RotateDirection(Quaternion rotation)
    {
        Vector2 currentDirection = rb.velocity;
        Vector2 newDirection = rotation * currentDirection;
        rb.velocity = newDirection;
    }
}
