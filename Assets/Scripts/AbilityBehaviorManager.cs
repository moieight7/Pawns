using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UltEvents;
using Unity.VisualScripting;
using UnityEngine;

public static class AbilityBehaviorManager
{
    private static GameObject player = GameObject.FindGameObjectWithTag("Player");

    public static string AbilityTriggerLog(Ability ability) { return ability.Name + " triggered by " + ability.caster + ". " + NumberOfChargesLog(ability); }
    public static string CooldownFinishLog(Ability ability) { return ability.Name + " cooldown finished by " + ability.caster + ". " + NumberOfChargesLog(ability); }
    public static string NumberOfChargesLog(Ability ability) { return "Number of charges: " + ability.numberOfCharges + " / " + ability.MaxCharges; }

    public static void CreateCastAbility(Entity caster, Ability ability, PersistentCall persistentCall)
    {
        CastAbility castAbility = new CastAbility(caster, ability, persistentCall);

        if (castAbility.persistentCall.PersistentArguments.Length > 0) castAbility.persistentCall.SetArguments(caster);
        castAbility.persistentCall.Invoke();
        DestroyCastAbility(castAbility);
    }

    private static void DestroyCastAbility(CastAbility castAbility)
    {
        if (castAbility.persistentCall.PersistentArguments.Length > 0) castAbility.persistentCall.SetArguments(null);
    }

    public static void TestPrimary_UseEffect(Entity caster, GameObject bullet)
    {
        BulletSpawner.SpawnBullet(caster, bullet);
    }

    public static void TestSecondary_UseEffect(Entity caster, GameObject bullet, int numberOfBullets = 3, float bulletSpread = 30)
    {
        Projectile projectileComponent = null;
        Vector2 direction = Vector2.zero;

        for (int i = 0; i < numberOfBullets; i++)
        {
            projectileComponent = BulletSpawner.SpawnBullet(caster, bullet);

            Quaternion rot = Quaternion.AngleAxis(0f - bulletSpread + (bulletSpread * i), Vector3.forward);
            projectileComponent.RotateDirection(rot);
        }
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
        EntityAbilities oldEntityAbilities = caster.GetComponent<EntityAbilities>();

        PlayerMovement newPlayerMovement = target.AddComponent<PlayerMovement>();
        EntityAbilities newEntityAbilities = target.GetComponent<EntityAbilities>();

        if (target.stateMachine == null) target.stateMachine = new FiniteStateMachine();
        if (caster.stateMachine == null) caster.stateMachine = new FiniteStateMachine();

        foreach (Ability ability in oldEntityAbilities.entityAbilities) AbilityCooldownManager.instance.CancelAbilityCooldown(ability);
        foreach (Ability ability in oldEntityAbilities.entityAbilities) AbilityCooldownManager.instance.ResetAbilityCooldown(ability);

        foreach (Ability ability in newEntityAbilities.entityAbilities) AbilityCooldownManager.instance.CancelAbilityCooldown(ability);
        foreach (Ability ability in newEntityAbilities.entityAbilities) AbilityCooldownManager.instance.ResetAbilityCooldown(ability);

        target.OnSwitchedTo(caster);
        caster.OnSwitchedFrom(target);

        Ability switchAbility = oldEntityAbilities.entityAbilities.Find(x => x.Type == AbilityType.Switch);
        newEntityAbilities.AddAbility(switchAbility);
        oldEntityAbilities.RemoveAbility(AbilityType.Switch);

        newEntityAbilities.SetAbilities();

        Target.instance.SetTarget(target.transform);

        float movementSpeed = oldPlayerMovement.movementSpeed;
        newPlayerMovement.movementSpeed = movementSpeed;
        GameObject.Destroy(oldPlayerMovement);

        EnemyTargetSwitcher.instance.SetNewTarget();
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

    private static class BulletSpawner
    {
        public static Projectile SpawnBullet(Entity caster, GameObject bullet)
        {
            GameObject projectile = null;
            Projectile projectileComponent = null;

            Vector3 diff = Vector3.zero;
            float rotationZ, distance;
            Vector2 direction = Vector2.zero;

            projectile = GameObject.Instantiate(bullet, caster.firePoint.transform.position, Quaternion.Euler(0f, 0f, 0f));
            projectileComponent = projectile.GetComponent<Projectile>();

            projectileComponent.sender = caster.firePoint;

            if (caster.type == EntityType.Player)
            {
                projectileComponent.target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                projectile.layer = LayerMask.NameToLayer("PlayerBullets");
            }
            else if (caster.type == EntityType.Enemy)
            {
                projectileComponent.target = caster.target.transform.position;
                projectile.layer = LayerMask.NameToLayer("EnemyBullets");
            }
            else Debug.LogError("AbilityBehaviorManager has a defined caster with an invalid EntityType");

            diff = projectileComponent.target - caster.firePoint.position;
            diff.Normalize();
            rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            distance = diff.magnitude;
            direction = diff / distance;
            direction.Normalize();

            projectileComponent.SetDirection(direction, rotationZ);

            projectile.GetComponent<Collider2D>().enabled = true;

            return projectileComponent;
        }
    }
}