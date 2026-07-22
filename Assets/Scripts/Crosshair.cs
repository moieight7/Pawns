using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private Image image;

    private bool isPlayerAlive = true;

    private void Awake()
    {
        Entity.OnPlayerKilled += OnPlayerKilled;
    }

    void Start()
    {
        image = GetComponent<Image>();

        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        if (!isPlayerAlive) return;

        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = target;
    }

    private void OnPlayerKilled()
    {
        image.enabled = false;
        Cursor.visible = true;
        isPlayerAlive = false;
    }
}
