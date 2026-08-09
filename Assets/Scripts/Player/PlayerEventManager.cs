using IngameDebugConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventManager : MonoBehaviour
{
    private Entity player;

    public static PlayerEventManager instance { get; private set; }

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one PlayerEventManager object! Destroying the newest one.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        Entity.OnSwitch += OnSwitch;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
    }

    public void EnablePlayerMovement()
    {
        player.GetComponent<PlayerMovement>().EnableMovement();
    }

    public void DisablePlayerMovement()
    {
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.GetComponent<PlayerMovement>().DisableMovement();
    }

    public void EnablePlayerAttacking()
    {
        player.GetComponent<EntityAbilities>().EnableAbilities();
    }

    public void DisablePlayerAttacking()
    {
        player.GetComponent<EntityAbilities>().DisableAbilities();
    }

    private void OnSwitch(Entity to, Entity from)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
    }
}
