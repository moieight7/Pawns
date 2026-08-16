using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetProjectile : Projectile
{
    [Header("Ricochet Projectile Stats")]
    public int numberOfBounces = 3;
    public LayerMask bounceOffMask;
    public TrailRenderer trail;
    public Gradient playerTrailColorGradient, enemyTrailColorGradient;

    private Vector3 previousPosition = Vector3.zero;
    private Vector3 currentPositionRayTo;

    protected override void Start()
    {
        base.Start();
        if (caster.type == EntityType.Player) trail.colorGradient = playerTrailColorGradient;
        else trail.colorGradient = enemyTrailColorGradient;
    }

    private void FixedUpdate()
    {
        if (previousPosition == Vector3.zero) previousPosition = transform.position;
        currentPositionRayTo = (transform.position - previousPosition);

        RaycastHit2D hit = Physics2D.Raycast(previousPosition, currentPositionRayTo, Vector3.Distance(transform.position, previousPosition), bounceOffMask);
        if (hit.collider != null)
        {
            numberOfBounces--;
            if (numberOfBounces <= 0) Destroy(gameObject);

            rb.velocity = Vector2.Reflect(rb.velocity.normalized, hit.normal) * speed;
            float rotationZ = Mathf.Atan2(-rb.velocity.x, rb.velocity.y) * Mathf.Rad2Deg;
            SetDirection(rb.velocity.normalized, rotationZ);
        }
        previousPosition = rb.position;
    }
}
