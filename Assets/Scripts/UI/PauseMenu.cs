using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI, resetConfirm, quitConfirm;

    private bool paused = false;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused) Pause();
            else Unpause();
        }
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
    }
}
