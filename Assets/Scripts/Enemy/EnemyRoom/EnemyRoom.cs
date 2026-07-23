using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class EnemyRoom : MonoBehaviour
{
    private bool roomPassed = false;
    public bool enterTrigger = false;

    #region Events
    [Header("Events")]
    [Space]
    public UltEvent OnRoomEnterEvent, OnWaveChangeEvent, OnRoomLeaveEvent, OnPlayerDeathEvent, OnRoomResetEvent;
    #endregion

    private void OnEnable()
    {
        EnemySpawner.OnWaveChanged += OnWaveChanged;
        EnemySpawner.OnSpawningOver += RoomPassed;

        //Entity.OnPlayerKilled += OnPlayerDeath;
        //Entity.OnPlayerRespawned += ResetRoom;
    }

    private void OnDisable()
    {
        EnemySpawner.OnWaveChanged -= OnWaveChanged;
        EnemySpawner.OnSpawningOver -= RoomPassed;

        //Entity.OnPlayerKilled -= OnPlayerDeath;
        //Entity.OnPlayerRespawned -= ResetRoom;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enterTrigger && collision.GetComponent<Entity>().type == EntityType.Player && !roomPassed)
        {
            enterTrigger = true;
            Debug.Log("OnRoomEnterEvent invoked by " + collision.name);
            OnRoomEnterEvent.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!roomPassed) return;
        Debug.Log("EnemyRoom OnTriggerExit invoked");
    }

    public void OnPlayerDeath()
    {
        if (!enterTrigger) return;
        Debug.Log("OnPlayerDeath invoked");
        OnPlayerDeathEvent.Invoke();
    }

    public void ResetRoom()
    {
        if (!enterTrigger) return;
        enterTrigger = false;

        Debug.Log("OnRoomResetEvent invoked, gameobject name: " + gameObject.name);
        OnRoomResetEvent.Invoke();
    }

    void OnWaveChanged()
    {
        if (!enterTrigger) return;
        Debug.Log("OnWaveChangedEvent invoked");
        OnWaveChangeEvent.Invoke();
    }

    void RoomPassed()
    {
        if (!enterTrigger) return;
        Debug.Log("OnRoomLeaveEvent invoked");
        OnRoomLeaveEvent.Invoke();
        //roomPassed = true;
        //enterTrigger = false;
    }

    public void PassEnemyRoom()
    {
        Debug.Log("PassEnemyRoom");
        OnRoomLeaveEvent.Invoke();
        roomPassed = true;
    }

    public void FinishEnemyRoom()
    {
        Debug.Log("FinishEnemyRoom");
        roomPassed = true;
    }

    public void DisableRoomEnterTriggers()
    {
        foreach (Collider collider in gameObject.GetComponents<Collider>()) collider.enabled = false;
    }

    public void ReEnableRoomEnterTriggers(float delay)
    {
        StartCoroutine(ReEnableRoomEnterTriggersCoroutine(delay));
    }

    IEnumerator ReEnableRoomEnterTriggersCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (Collider collider in gameObject.GetComponents<Collider>()) collider.enabled = true;
    }
}
