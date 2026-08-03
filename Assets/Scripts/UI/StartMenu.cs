using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject startUI, quitConfirm;

    private bool startMenuOpen = false;

    public bool StartMenuOpen
    {
        get => startMenuOpen;
        private set => startMenuOpen = value;
    }

    public static StartMenu instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("An instance of StartMenu already exists. Deleting the newest one...");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        startMenuOpen = true;
        //SlowdownManager.instance.Pause();
    }

    public void Play()
    {
        SceneManager.LoadScene("Gameplay");
        startUI.transform.DOLocalMove(new Vector3(0, 390.2f, 0), 1.5f).SetEase(Ease.InSine).SetUpdate(true).OnComplete(() =>
        {
            startMenuOpen = false;
            //SlowdownManager.instance.UnPause();
        });
    }

    public void SnapTo()
    {
        startMenuOpen = true;
        DOTweenAnimationManager.Move(startUI, new Vector3(0, 0, 0), 0.01f, Ease.Linear, true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
