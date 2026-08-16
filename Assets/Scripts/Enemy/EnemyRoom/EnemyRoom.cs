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
    }

    private void OnDisable()
    {
        EnemySpawner.OnWaveChanged -= OnWaveChanged;
        EnemySpawner.OnSpawningOver -= RoomPassed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enterTrigger && collision.GetComponent<Entity>().type == EntityType.Player && !roomPassed)
        {
            enterTrigger = true;
            OnRoomEnterEvent.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!roomPassed) return;
    }

    public void OnPlayerDeath()
    {
        if (!enterTrigger) return;
        OnPlayerDeathEvent.Invoke();
    }

    public void ResetRoom()
    {
        if (!enterTrigger) return;
        enterTrigger = false;
        OnRoomResetEvent.Invoke();
    }

    void OnWaveChanged()
    {
        if (!enterTrigger) return;
        OnWaveChangeEvent.Invoke();
    }

    void RoomPassed()
    {
        if (!enterTrigger) return;
        OnRoomLeaveEvent.Invoke();
    }

    public void PassEnemyRoom()
    {
        OnRoomLeaveEvent.Invoke();
        roomPassed = true;
    }

    public void FinishEnemyRoom()
    {
        roomPassed = true;
    }
}
