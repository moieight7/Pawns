using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UltEvents;
using UnityEngine;

public static class AbilityBehaviorManager
{
    private static List<CastAbility> castAbilities = new List<CastAbility>();

    public static string AbilityTriggerLog(Ability ability) { return ability.name + "triggered. " + NumberOfChargesLog(ability); }
    public static string CooldownFinishLog(Ability ability) { return ability.name + "cooldown finished. " + NumberOfChargesLog(ability); }
    public static string NumberOfChargesLog(Ability ability) { return "Number of charges: " + ability.numberOfCharges + " / " + ability.maxCharges; }

    public static void CreateCastAbility(Entity caster, Ability ability, PersistentCall persistentCall)
    {
        CastAbility castAbility = new CastAbility(caster, ability, persistentCall);
        //castAbilities.Add(castAbility);

        if (castAbility.persistentCall.PersistentArguments.Length > 0) castAbility.persistentCall.SetArguments(caster);
        castAbility.persistentCall.Invoke();
    }

    public static void TestPrimary_UseEffect(Entity caster, GameObject bullet)
    {
        Debug.Log("TestPrimary_UseEffect");
        Debug.Log(caster.name);

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
        Debug.Log("TestPrimary_UseEffect");
        Debug.Log(caster.name);

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