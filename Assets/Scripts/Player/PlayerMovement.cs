using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 1f;

    private Rigidbody2D rb;
    private Vector2 motion;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        motion = new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed * Time.deltaTime, Input.GetAxisRaw("Vertical") * movementSpeed * Time.deltaTime);

        rb.velocity = motion;

        if (rb.velocity.x < 0) GetComponent<SpriteRenderer>().flipX = true;
        else if (rb.velocity.x > 0) GetComponent<SpriteRenderer>().flipX = false;
    }
}
