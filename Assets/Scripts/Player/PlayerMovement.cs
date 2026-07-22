using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;

    private Rigidbody2D rb;
    private Vector2 motion;

    private Crosshair crosshair;
    private bool canMove = true;

    private void Awake()
    {
        Entity.OnPlayerKilled += OnPlayerKilled;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        crosshair = FindAnyObjectByType<Crosshair>();
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        motion = new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed * Time.deltaTime, Input.GetAxisRaw("Vertical") * movementSpeed * Time.deltaTime);

        rb.velocity = motion;

        if (gameObject.transform.position.x > crosshair.gameObject.transform.position.x) GetComponent<SpriteRenderer>().flipX = true;
        else if (gameObject.transform.position.x < crosshair.gameObject.transform.position.x) GetComponent<SpriteRenderer>().flipX = false;

        /*if (rb.velocity.x < 0) GetComponent<SpriteRenderer>().flipX = true;
        else if (rb.velocity.x > 0) GetComponent<SpriteRenderer>().flipX = false;*/
    }

    public void OnPlayerKilled()
    {
        rb.velocity = Vector2.zero;
        canMove = false;
    }
}
