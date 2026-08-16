using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private Entity player;
    private SpriteRenderer spriteRenderer;
    private bool facingRight = false;

    private void OnEnable()
    {
        Entity.OnSwitch += OnSwitch;
    }

    private void OnDisable()
    {
        Entity.OnSwitch += OnSwitch;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (transform.position.x < player.transform.position.x && !facingRight)
            Flip();
        else if (transform.position.x > player.transform.position.x && facingRight)
            Flip();
    }

    public void Flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        facingRight = !facingRight;
    }

    private void OnSwitch(Entity to, Entity from)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
    }
}
