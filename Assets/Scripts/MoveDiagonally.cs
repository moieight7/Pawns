using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDiagonally : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = new Vector2(1, 1);

    void FixedUpdate()
    {
        gameObject.transform.Translate(new Vector2(direction.x * speed * Time.deltaTime, direction.y * speed * Time.deltaTime));
    }
}
