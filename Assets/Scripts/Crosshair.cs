using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private Image image;

    private bool isPlayerAlive = true;

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

    public void OnPlayerKilled()
    {
        image.enabled = false;
        Cursor.visible = true;
        isPlayerAlive = false;
    }

    public void OnPlayerRevived()
    {
        image.enabled = true;
        Cursor.visible = false;
        isPlayerAlive = true;
    }
}
