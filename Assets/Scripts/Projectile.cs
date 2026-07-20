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

    private float timer;
    [HideInInspector] public bool invisible = false;

    public UltEvent OnHitEvent;

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
        Debug.Log("Collided with " + collision.name);

        Entity entity = collision.GetComponent<Entity>();

        if (entity != null && entity.type == EntityType.Enemy) entity.TakeDamage(damage);

        if (OnHitEvent != null) foreach (PersistentCall call in OnHitEvent.PersistentCallsList) SetOnHitPersistentCallArguments(call, collision);
        OnHitEvent.Invoke();

        Destroy(gameObject);
    }

    private void SetOnHitPersistentCallArguments(PersistentCall call, Collider2D collision)
    {
        if (call.PersistentArguments.Length > 0)
        {
            Entity senderEntity, targetEntity;
        
            if (!(senderEntity = sender.GetComponent<Entity>()))
                if (!(senderEntity = sender.GetComponentInParent<Entity>())) return;
            if (!(targetEntity = collision.gameObject.GetComponent<Entity>()))
                if (!(targetEntity = collision.gameObject.GetComponentInParent<Entity>())) return;

            if (senderEntity == null) Debug.LogError("Projectile " + gameObject.name + " has no defined sender entity.");
            if (targetEntity == null) Debug.LogError("Projectile " + gameObject.name + " has no defined target entity.");
            call.SetArguments(senderEntity, targetEntity);
        }
    }

    public void SetDirection(Vector2 direction, float rotationZ)
    {
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
}
