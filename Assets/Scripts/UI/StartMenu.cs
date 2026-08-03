using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject startUI, quitConfirm;

    void Start()
    {
        SnapToDropped();
        SlowdownManager.instance.Pause();
    }

    public void Play()
    {
        startUI.transform.DOLocalMove(new Vector3(0, 336, 0), 1.5f).SetEase(Ease.InSine).SetUpdate(true).OnComplete(() =>
        {
            SlowdownManager.instance.UnPause();
        });
    }

    public void SnapToDropped()
    {
        DOTweenAnimationManager.LocalMove(startUI, new Vector3(0, -54.4f, 0), 0.01f, Ease.Linear, true);
    }
}
