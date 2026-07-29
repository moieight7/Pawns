using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UltEvents;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
        AttackSpawner.SpawnAttack(caster, bullet);
    }

    public static void TestSecondary_UseEffect(Entity caster, GameObject bullet, int numberOfBullets = 3, float bulletSpread = 30)
    {
        Projectile projectileComponent = null;
        Vector2 direction = Vector2.zero;

        for (int i = 0; i < numberOfBullets; i++)
        {
            projectileComponent = (Projectile)AttackSpawner.SpawnAttack(caster, bullet);

            Quaternion rot = Quaternion.AngleAxis(0f - bulletSpread + (bulletSpread * i), Vector3.forward);
            projectileComponent.RotateDirection(rot);
        }
    }

    public static void TestUtility_UseEffect(Entity caster, float dashSpeed, float dashSmoothTime)
    {
        
    }

    public static void TestSpecial_UseEffect()
    {

    }

    public static void Sword_UseEffect(Entity caster, GameObject prefab)
    {
        Swing swing = (Swing)AttackSpawner.SpawnAttack(caster, prefab);
        swing.transform.parent = caster.firePoint.transform;
    }

    public static void TestSwitch_UseEffect(Entity caster, GameObject bullet)
    {
        Projectile projectile = (Projectile)AttackSpawner.SpawnAttack(caster, bullet);
        projectile.sender = caster.firePoint;
    }

    public static void Dash_UseEffect(Entity caster, float force, float duration)
    {
        if (caster.type == EntityType.Player)
        {
            PlayerMovement playerMovement = caster.GetComponent<PlayerMovement>();
            caster.Dash(playerMovement.motion.normalized * force, duration);
        }
    }

    public static void Teleport_UseEffect(Entity caster)
    {
        if (caster.type == EntityType.Player)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            caster.transform.position = mousePosition;
        }
        else if (caster.type == EntityType.Enemy)
        {
            if (caster is Wizard)
            {
                Wizard wizardObject = (Wizard)caster;
                float teleportRadius = wizardObject.GetTeleportRadius();

                Vector3 circlePos = Vector3.zero;
                bool isValidPosition;
                NavMeshHit hit;
                do
                {
                    circlePos = Random.insideUnitSphere * teleportRadius;
                    isValidPosition = NavMesh.SamplePosition(circlePos, out hit, 0.1f, 1 << NavMesh.GetAreaFromName("Walkable"));
                } while (!isValidPosition);

                caster.transform.position = hit.position;
            }
        }
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

    private static class AttackSpawner
    {
        public static Attack SpawnAttack(Entity caster, GameObject prefab)
        {
            GameObject attack = null;
            Attack attackComponent = null;

            attack = Instantiate(caster, prefab);
            attackComponent = attack.GetComponent<Attack>();

            SetDirectionAndRotation(caster, attackComponent);

            attack.GetComponent<Collider2D>().enabled = true;

            return attackComponent;
        }

        private static GameObject Instantiate(Entity caster, GameObject prefab)
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

        private static void SetDirectionAndRotation(Entity caster, Attack attackComponent)
        {
            Vector3 diff = Vector3.zero;
            float rotationZ, distance;
            Vector2 direction = Vector2.zero;

            if (caster.type == EntityType.Player) diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - caster.firePoint.position;
            else if (caster.type == EntityType.Enemy) diff = caster.target.transform.position - caster.firePoint.position;
            diff.Normalize();
            rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            distance = diff.magnitude;
            direction = diff / distance;
            direction.Normalize();

            attackComponent.SetDirection(direction, rotationZ);
        }
    }
}