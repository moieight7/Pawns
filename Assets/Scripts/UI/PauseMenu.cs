using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI, content, resetConfirm, quitConfirm;

    private bool paused = false;
    private Tween moveTween;

    public bool Paused
    {
        get => paused;
        private set => paused = value;
    }

    public static PauseMenu instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("An instance of PauseMenu already exists. Deleting the newest one...");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        pauseUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !StartMenu.instance.StartMenuOpen)
        {
            if (!paused) Pause();
            else Unpause();
        }
    }

    public void MoveMenu(Vector3 vector, float duration, Ease ease = Ease.Linear)
    {
        moveTween = content.transform.DOBlendableLocalMoveBy(vector, duration).SetEase(ease).SetUpdate(true);
    }

    public void Pause()
    {
        paused = true;
        pauseUI.SetActive(true);
        SlowdownManager.instance.Pause();
    }

    public void Unpause()
    {
        paused = false;
        pauseUI.SetActive(false);
        SlowdownManager.instance.UnPause();

        moveTween.Kill(true);
        content.transform.localPosition = new Vector3(0, 65, 0);
    }

    public void ResetScene()
    {
        LevelReset.Reset();
        Unpause();
    }

    public void Quit()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
