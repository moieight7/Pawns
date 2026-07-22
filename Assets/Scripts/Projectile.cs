using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Base Stats")]
    public float damage;
    public float speed;
    public float timeUntilDelete = 1;
    //public GameObject hitEffect;

    [HideInInspector] public Transform sender;
    public Vector3 target;
    protected Vector2 moveDir;

    private Rigidbody2D rb;

    private float timer;
    [HideInInspector] public bool invisible = false;

    public UltEvent OnHitEvent;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        Debug.Log("Collided with " + collision.name);

        Entity entity = collision.GetComponent<Entity>();
        if (entity != null) entity.TakeDamage(damage);

        if (OnHitEvent != null)
        {
            bool persistentCallsValid = true;
            foreach (PersistentCall call in OnHitEvent.PersistentCallsList)
            {
                persistentCallsValid = SetOnHitPersistentCallArguments(call, collision);
                if (!persistentCallsValid) break;
            }
            if (persistentCallsValid) OnHitEvent.Invoke();
        }

        Destroy(gameObject);
    }

    private bool SetOnHitPersistentCallArguments(PersistentCall call, Collider2D collision)
    {
        if (call.PersistentArguments.Length > 0)
        {
            Entity senderEntity, targetEntity;

            if (!(senderEntity = sender.GetComponent<Entity>()))
                if (!(senderEntity = sender.GetComponentInParent<Entity>())) { return false; }
            if (!(targetEntity = collision.gameObject.GetComponent<Entity>()))
                if (!(targetEntity = collision.gameObject.GetComponentInParent<Entity>())) { return false; }

            call.SetArguments(senderEntity, targetEntity);
            return true;
        }
        else
        {
            Debug.LogError("SetOnHitPersistentCallArguments tried to set PersistentArguments for a call with 0 parameters");
            return false;
        }
    }

    public void SetDirection(Vector2 direction, float rotationZ)
    {
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        rb.velocity = direction * speed;
    }

    public void RotateDirection(Quaternion rotation)
    {
        Vector2 currentDirection = rb.velocity;
        Vector2 newDirection = rotation * currentDirection;
        rb.velocity = newDirection;
    }
}
