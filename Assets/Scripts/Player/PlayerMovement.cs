using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    [HideInInspector] public Vector2 motion;

    private Rigidbody2D rb;
    private Entity entity;

    private Crosshair crosshair;
    private bool canMove = true;

    void Start()
    {
        entity = GetComponent<Entity>();
        rb = GetComponent<Rigidbody2D>();
        crosshair = FindAnyObjectByType<Crosshair>();
    }

    void FixedUpdate()
    {
        if (!canMove || entity.isDashing) return;

        motion = new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed * Time.deltaTime, Input.GetAxisRaw("Vertical") * movementSpeed * Time.deltaTime);

        rb.velocity = motion;

        if (gameObject.transform.position.x > crosshair.gameObject.transform.position.x) GetComponent<SpriteRenderer>().flipX = true;
        else if (gameObject.transform.position.x < crosshair.gameObject.transform.position.x) GetComponent<SpriteRenderer>().flipX = false;

        /*if (rb.velocity.x < 0) GetComponent<SpriteRenderer>().flipX = true;
        else if (rb.velocity.x > 0) GetComponent<SpriteRenderer>().flipX = false;*/
    }

    public void OnPlayerKilled()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        canMove = false;
    }
}
