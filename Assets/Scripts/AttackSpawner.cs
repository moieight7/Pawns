using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpawner : MonoBehaviour
{
    public static AttackSpawner instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("An instance of AttackSpawner already exists. Deleting the newest one...");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public Attack SpawnAttack(Entity caster, GameObject prefab)
    {
        GameObject attack = null;
        Attack attackComponent = null;

        attack = Instantiate(caster, prefab);
        attackComponent = attack.GetComponent<Attack>();

        SetDirectionAndRotation(caster, attackComponent);

        attack.GetComponent<Collider2D>().enabled = true;

        return attackComponent;
    }

    public void SpawnAttack(Entity caster, GameObject prefab, float delay)
    {
        StartCoroutine(SpawnAttackWithDelay(caster, prefab, delay));
    }

    private IEnumerator SpawnAttackWithDelay(Entity caster, GameObject prefab, float delay)
    {
        EntityType type = caster.type;

        yield return new WaitForSeconds(delay);

        if (type != caster.type) yield break;

        GameObject attack = null;
        Attack attackComponent = null;

        attack = Instantiate(caster, prefab);
        attackComponent = attack.GetComponent<Attack>();

        SetDirectionAndRotation(caster, attackComponent);

        attack.GetComponent<Collider2D>().enabled = true;
    }

    private GameObject Instantiate(Entity caster, GameObject prefab)
    {
        GameObject attack = null;
        Attack attackComponent = null;

        attack = GameObject.Instantiate(prefab, caster.firePoint.transform.position, Quaternion.Euler(0f, 0f, 0f));
        attackComponent = attack.GetComponent<Attack>();
        attackComponent.sender = caster.firePoint;

        if (caster.type == EntityType.Player) attack.layer = LayerMask.NameToLayer("PlayerBullets");
        else if (caster.type == EntityType.Enemy) attack.layer = LayerMask.NameToLayer("EnemyBullets");
        else Debug.LogError("AbilityBehaviorManager has a defined caster with an invalid EntityType");

        return attack;
    }

    private void SetDirectionAndRotation(Entity caster, Attack attackComponent)
    {
        Vector3 diff = Vector3.zero;
        float rotationZ, distance;
        Vector2 direction = Vector2.zero;

        if (caster.type == EntityType.Player) diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - caster.firePoint.position;
        else if (caster.type == EntityType.Enemy) diff = caster.target.transform.position - caster.firePoint.position;
        diff.Normalize();
        if (attackComponent is not Swing) rotationZ = Mathf.Atan2(-diff.x, diff.y) * Mathf.Rad2Deg;
        else rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        distance = diff.magnitude;
        direction = diff / distance;
        direction.Normalize();

        attackComponent.SetDirection(direction, rotationZ);
    }
}
