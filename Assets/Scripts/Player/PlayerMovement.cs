using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    [HideInInspector] public Vector2 motion;

    private Rigidbody2D rb;
    private Entity entity;

    private Crosshair crosshair;
    private bool canMove = true;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnLevelReset;
    }

    void Start()
    {
        entity = GetComponent<Entity>();
        rb = GetComponent<Rigidbody2D>();
        crosshair = FindObjectOfType<Crosshair>();

        movementSpeed = entity.entityData.playerMovementSpeed;
    }

    void FixedUpdate()
    {
        if (!canMove || entity.isDashing || entity.type != EntityType.Player) return;

        motion = new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed * Time.deltaTime, Input.GetAxisRaw("Vertical") * movementSpeed * Time.deltaTime);

        rb.velocity = motion;

        if (gameObject.transform.position.x > crosshair.gameObject.transform.position.x && entity.entityData.flipX) GetComponent<SpriteRenderer>().flipX = false;
        else if (gameObject.transform.position.x < crosshair.gameObject.transform.position.x && entity.entityData.flipX) GetComponent<SpriteRenderer>().flipX = true;

        /*if (rb.velocity.x < 0) GetComponent<SpriteRenderer>().flipX = true;
        else if (rb.velocity.x > 0) GetComponent<SpriteRenderer>().flipX = false;*/
    }

    public void EnableMovement() 
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public void OnPlayerKilled()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        canMove = false;
    }

    public void OnPlayerRevived()
    {
        canMove = true;
    }

    private void OnLevelReset(Scene arg0, LoadSceneMode arg1)
    {
        crosshair = FindObjectOfType<Crosshair>();
    }
}
