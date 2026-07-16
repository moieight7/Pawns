using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Base Stats")]
    public float damage;
    public float speed;
    public float timeUntilDelete = 1;
    //public GameObject hitEffect;

    [HideInInspector] public Transform sender;
    [HideInInspector] public Vector3 target;
    protected Vector2 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDirection()
    {
        Vector3 diff = target - sender.position;
        diff.Normalize();
        float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        float distance = diff.magnitude;
        Vector2 direction = diff / distance;
        direction.Normalize();

        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
}
