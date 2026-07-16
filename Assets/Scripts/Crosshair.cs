using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = target;
    }
}
