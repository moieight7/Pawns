using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UltEvents;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public static class AbilityBehaviorManager
{
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
        AttackSpawner.instance.SpawnAttack(caster, bullet);
    }

    public static void TestSecondary_UseEffect(Entity caster, GameObject bullet, int numberOfBullets = 3, float bulletSpread = 30)
    {
        Projectile projectileComponent = null;
        Vector2 direction = Vector2.zero;

        for (int i = 0; i < numberOfBullets; i++)
        {
            projectileComponent = (Projectile)AttackSpawner.instance.SpawnAttack(caster, bullet);

            Quaternion rot = Quaternion.AngleAxis(0f - bulletSpread + (bulletSpread * i), Vector3.forward);
            projectileComponent.RotateDirection(rot);
        }
    }

    public static void Sword_UseEffect(Entity caster, GameObject prefab)
    {
        Swing swing = (Swing)AttackSpawner.instance.SpawnAttack(caster, prefab);
        swing.transform.parent = caster.firePoint.transform;
    }

    public static void TestSwitch_UseEffect(Entity caster, GameObject bullet)
    {
        Projectile projectile = (Projectile)AttackSpawner.instance.SpawnAttack(caster, bullet);
        projectile.sender = caster.firePoint;
    }

    public static void SpiderPrimary_UseEffect(Entity caster, GameObject bullet, float numberOfBullets, float delay)
    {
        for (int i = 0; i < numberOfBullets; i++) AttackSpawner.instance.SpawnAttack(caster, bullet, delay * i);
    }

    public static void Dash_UseEffect(Entity caster, float force, float duration)
    {
        if (caster.type == EntityType.Player)
        {
            PlayerMovement playerMovement = caster.GetComponent<PlayerMovement>();
            caster.Dash(playerMovement.motion.normalized * force, duration);
        }
        else if (caster.type == EntityType.Enemy)
        {
            Attack attackToAvoid;
            caster.isDashing = true;

            attackToAvoid = caster.GetDangerousObject().GetComponentInParent<Attack>();
            if (attackToAvoid == null) return;

            D_DashState dashStateData = null;
            if (caster is Spider)
            {
                Spider spiderObject = (Spider)caster;
                dashStateData = spiderObject.GetDashStateData();
            }
            else if (caster is LesserKnight)
            {
                LesserKnight lesserKnightObject = (LesserKnight)caster;
                dashStateData = lesserKnightObject.GetDashStateData();
            }

            Vector2 dashDir = Vector2.zero;
            if (attackToAvoid is Projectile)
            {
                Debug.Log("AttackToAvoid projectile");

                Projectile projectile = (Projectile)attackToAvoid;
                dashDir = new Vector2(projectile.moveDir.y, -projectile.moveDir.x);

                dashDir.x *= dashDir.x * dashStateData.dashForce;
                dashDir.y *= dashDir.y * dashStateData.dashForce;
                caster.Dash(dashDir, dashStateData.dashDuration);
            }
            else if (attackToAvoid is Swing)
            {
                Debug.Log("AttackToAvoid swing");

                Swing swing = (Swing)attackToAvoid;

                dashDir = swing.sender.transform.position - caster.transform.position;
                dashDir.Normalize();

                dashDir.x *= dashDir.x * dashStateData.dashForce;
                dashDir.y *= dashDir.y * dashStateData.dashForce;

                if (swing.sender.transform.position.x > caster.transform.position.x) dashDir.x *= -1;
                if (swing.sender.transform.position.y > caster.transform.position.y) dashDir.y *= -1;

                caster.Dash(dashDir, dashStateData.dashDuration);
            }
        }
    }

    public static void Firebolt_UseEffect(Entity caster, GameObject bullet)
    {
        Explosive firebolt = (Explosive)AttackSpawner.instance.SpawnAttack(caster, bullet);

        if (caster.type == EntityType.Enemy)
        {
            Transform target = caster.target;

            float distanceFromTarget = (target.position - caster.transform.position).magnitude;
            firebolt.SetMovementTween((distanceFromTarget / firebolt.speed) * 1.2f);
        }
    }

    public static void Teleport_UseEffect(Entity caster)
    {
        if (caster.type == EntityType.Player)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            NavMeshHit hit;
            bool isValidPosition = NavMesh.SamplePosition(mousePosition, out hit, 1, 1 << NavMesh.GetAreaFromName("Walkable"));

            if (!isValidPosition) return;

            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(caster.transform.position, mousePosition, 1 << NavMesh.GetAreaFromName("Walkable"), path);
            if (path.status != NavMeshPathStatus.PathComplete) return;

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

                int timesChecked = 0;
                do
                {
                    circlePos = Random.insideUnitSphere * teleportRadius;
                    isValidPosition = NavMesh.SamplePosition(circlePos, out hit, 1, 1 << NavMesh.GetAreaFromName("Walkable"));

                    NavMeshPath path = new NavMeshPath();
                    NavMesh.CalculatePath(caster.transform.position, circlePos, 1 << NavMesh.GetAreaFromName("Walkable"), path);
                    if (path.status != NavMeshPathStatus.PathComplete) isValidPosition = false;

                    timesChecked++;
                } while (!isValidPosition || timesChecked < 100);

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
}