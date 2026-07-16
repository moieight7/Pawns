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

    private float timer;
    [HideInInspector] public bool invisible = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (invisible)
        {
            timer += Time.deltaTime;
            if (timer > timeUntilDelete) Destroy(gameObject);
        }
        else timer = 0;
    }

    public virtual void OnBecameInvisible()
    {
        invisible = true;
    }

    public virtual void OnBecameVisible()
    {
        invisible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    public void SetDirection(Vector2 direction, float rotationZ)
    {
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
}
