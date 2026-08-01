using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetProjectile : Projectile
{
    [Header("Ricochet Projectile Stats")]
    public int numberOfBounces = 3;
    public LayerMask bounceOffMask;

    private Vector3 previousPosition;
    private Vector3 currentPositionRayTo;

    private void FixedUpdate()
    {
        currentPositionRayTo = (transform.position - previousPosition);

        RaycastHit2D hit = Physics2D.Raycast(previousPosition, currentPositionRayTo, Vector3.Distance(transform.position, previousPosition), bounceOffMask);
        if (hit.collider != null)
        {
            numberOfBounces--;
            if (numberOfBounces <= 0) Destroy(gameObject);

            rb.velocity = Vector2.Reflect(rb.velocity.normalized, hit.normal) * speed;
            float rotationZ = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.AngleAxis(rotationZ, Vector3.forward);
            SetDirection(rb.velocity.normalized, rotationZ);
        }
        previousPosition = rb.position;
    }

    protected new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        /*RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 10, bounceOffMask);
        if (hit.collider != null)
        {
            numberOfBounces--;
            if (numberOfBounces <= 0) Destroy(gameObject);

            rb.velocity = Vector2.Reflect(rb.velocity.normalized, hit.normal) * speed;
            float rotationZ = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.AngleAxis(rotationZ, Vector3.forward);
            SetDirection(rb.velocity.normalized, rotationZ);
        }*/
    }
}
