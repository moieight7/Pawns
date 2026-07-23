using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Base Stats")]
    public float damage;
    public float timeUntilDelete = 1;
    public bool destroyOnHit = true;
    //public GameObject hitEffect;

    [HideInInspector] public Transform sender;

    private float timer;
    [HideInInspector] public bool invisible = false;

    public UltEvent OnHitEvent;

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

    public virtual void SetDirection(Vector2 direction, float rotationZ)
    {
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
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

        if (destroyOnHit) Destroy(gameObject);
    }

    protected bool SetOnHitPersistentCallArguments(PersistentCall call, Collider2D collision)
    {
        if (call.PersistentArguments.Length > 0)
        {
            Entity senderEntity, targetEntity;

            if (!(senderEntity = sender.GetComponent<Entity>()))
                if (!(senderEntity = sender.GetComponentInParent<Entity>())) { return false; }
            if (!(targetEntity = collision.gameObject.GetComponent<Entity>()))
                if (!(targetEntity = collision.gameObject.GetComponentInParent<Entity>())) { return false; }

            Debug.Log("senderEntity: " + senderEntity.name + " " + "targetEntity: " + targetEntity.name);

            call.SetArguments(senderEntity, targetEntity);
            return true;
        }
        else
        {
            Debug.LogError("SetOnHitPersistentCallArguments tried to set PersistentArguments for a call with 0 parameters");
            return false;
        }
    }
}
