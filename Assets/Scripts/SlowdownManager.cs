using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownManager : MonoBehaviour
{
    private float desiredTimeScale = 1f, multiplier = 1;
    private float lastTimeScale;

    public static SlowdownManager instance { get; private set; }

    public delegate void GamePausedAction();
    public static event GamePausedAction OnGamePaused;

    public delegate void GameUnpausedAction();
    public static event GameUnpausedAction OnGameUnpaused;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one SlowdownManager object! Destroying the newest one.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        desiredTimeScale = Time.timeScale;
    }

    public void Slowdown(float startAmount, float endAmount, float duration, Ease easeType)
    {
        Time.timeScale = startAmount;
        multiplier = duration;
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, endAmount, multiplier).SetEase(easeType).SetUpdate(true);
    }

    public void Slowdown(float amount, float duration, Ease easeType)
    {
        Time.timeScale = amount;
        multiplier = duration;
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, multiplier).SetEase(easeType).SetUpdate(true);
    }

    public void Pause()
    {
        lastTimeScale = Time.timeScale;
        Time.timeScale = 0;
        if (OnGamePaused != null) OnGamePaused.Invoke();
    }

    public void UnPause()
    {
        Time.timeScale = lastTimeScale;
        if (OnGameUnpaused != null) OnGameUnpaused.Invoke();
    }
}
