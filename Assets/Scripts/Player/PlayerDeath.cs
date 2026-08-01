using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject deathUI;

    public UltEvent OnDeathEvent;

    public static PlayerDeath instance { get; private set; }

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one PlayerDeath object! Destroying the newest one.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void TriggerDeathSequence(Entity entity)
    {
        OnPlayerKilledCalls(entity);
        SlowdownManager.instance.Slowdown(1, 0, 5, Ease.OutCirc);

        OnDeathEvent.Invoke();
    }

    public void EndDeathSequence(Entity entity)
    {
        OnPlayerRevivedCalls(entity);
        SlowdownManager.instance.CancelSlowdown();
    }

    private void OnPlayerKilledCalls(Entity entity)
    {
        entity.GetComponent<PlayerMovement>().OnPlayerKilled();
        entity.GetComponent<EntityAbilities>().OnPlayerKilled();
        FindObjectOfType<Crosshair>().OnPlayerKilled();
        Target.instance.OnPlayerKilled();
    }

    private void OnPlayerRevivedCalls(Entity entity)
    {
        entity.GetComponent<PlayerMovement>().OnPlayerRevived();
        entity.GetComponent<EntityAbilities>().OnPlayerRevived();
        FindObjectOfType<Crosshair>().OnPlayerRevived();
        Target.instance.OnPlayerRevived();
    }
}
