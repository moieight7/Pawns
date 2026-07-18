using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UltEvents;
using Unity.VisualScripting;
using UnityEngine;

public static class AbilityBehaviorManager
{
    public static string AbilityTriggerLog(Ability ability) { return ability.Name + " triggered. " + NumberOfChargesLog(ability); }
    public static string CooldownFinishLog(Ability ability) { return ability.Name + " cooldown finished. " + NumberOfChargesLog(ability); }
    public static string NumberOfChargesLog(Ability ability) { return "Number of charges: " + ability.numberOfCharges + " / " + ability.MaxCharges; }

    public static void CreateCastAbility(Entity caster, Ability ability, PersistentCall persistentCall)
    {
        CastAbility castAbility = new CastAbility(caster, ability, persistentCall);

        if (castAbility.persistentCall.PersistentArguments.Length > 0) castAbility.persistentCall.SetArguments(caster);
        castAbility.persistentCall.Invoke();
    }

    public static void TestPrimary_UseEffect(Entity caster, GameObject bullet)
    {
        if (caster.type == EntityType.Player)
        {
            GameObject projectile = GameObject.Instantiate(bullet, caster.firePoint.transform.position, Quaternion.Euler(0f, 0f, 0f));

            Projectile projectileComponent = projectile.GetComponent<Projectile>();

            projectileComponent.sender = caster.firePoint;
            projectileComponent.target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - caster.firePoint.position;
            diff.Normalize();
            float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            float distance = diff.magnitude;
            Vector2 direction = diff / distance;
            direction.Normalize();

            projectileComponent.SetDirection(direction, rotationZ);
        }
        else if (caster.type == EntityType.Enemy)
        {

        }
        else Debug.LogError("AbilityBehaviorManager has a defined caster with an invalid EntityType");
    }

    public static void TestSecondary_UseEffect(Entity caster, GameObject bullet, int numberOfBullets = 3, float bulletSpread = 30)
    {
        if (caster.type == EntityType.Player)
        {
            for (int i = 0; i < numberOfBullets; i++)
            {
                GameObject projectile = GameObject.Instantiate(bullet, caster.firePoint.transform.position, Quaternion.Euler(0f, 0f, 0f));

                Projectile projectileComponent = projectile.GetComponent<Projectile>();

                projectileComponent.sender = caster.firePoint;
                projectileComponent.target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - caster.firePoint.position;
                diff.Normalize();
                float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

                float distance = diff.magnitude;
                Vector2 direction = diff / distance;
                direction.Normalize();

                Quaternion rot = Quaternion.AngleAxis(0f - bulletSpread + (bulletSpread * i), Vector3.forward);

                direction = rot * direction;

                projectileComponent.SetDirection(direction, rotationZ);
            }
        }
        else if (caster.type == EntityType.Enemy)
        {

        }
        else Debug.LogError("AbilityBehaviorManager has a defined caster with an invalid EntityType");
    }

    public static void TestUtility_UseEffect()
    {

    }

    public static void TestSpecial_UseEffect()
    {

    }

    public static void TestSwitch_UseEffect(Entity caster, GameObject bullet)
    {
        GameObject projectile = GameObject.Instantiate(bullet, caster.firePoint.transform.position, Quaternion.Euler(0f, 0f, 0f));

        Projectile projectileComponent = projectile.GetComponent<Projectile>();

        projectileComponent.sender = caster.firePoint;
        projectileComponent.target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - caster.firePoint.position;
        diff.Normalize();
        float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        float distance = diff.magnitude;
        Vector2 direction = diff / distance;
        direction.Normalize();

        projectileComponent.SetDirection(direction, rotationZ);
    }

    public static void SwitchCharacters(Entity caster, Entity target)
    {
        PlayerMovement oldPlayerMovement = caster.GetComponent<PlayerMovement>();
        EntityAbilities oldPlayerAbilities = caster.GetComponent<EntityAbilities>();

        PlayerMovement newPlayerMovement = target.AddComponent<PlayerMovement>();
        EntityAbilities newPlayerAbilities = target.GetComponent<EntityAbilities>();

        target.type = EntityType.Player;
        caster.type = EntityType.Enemy;

        newPlayerAbilities.SetAbilities();

        foreach (Ability ability in oldPlayerAbilities.entityAbilities) AbilityCooldownManager.instance.CancelAbilityCooldown(ability);

        CameraTarget.instance.SetTarget(target.transform);

        target.gameObject.layer = LayerMask.NameToLayer("Player");
        caster.gameObject.layer = LayerMask.NameToLayer("Enemy");

        float movementSpeed = oldPlayerMovement.movementSpeed;
        newPlayerMovement.movementSpeed = movementSpeed;
        GameObject.Destroy(oldPlayerMovement);

        caster.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    private class CastAbility
    {
        public Entity caster;
        public Ability ability;
        public PersistentCall persistentCall;

        public CastAbility(Entity caster, Ability ability, PersistentCall persistentCall)
        {
            this.caster = caster;
            this.ability = ability;
            this.persistentCall = persistentCall;
        }
    }
}