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
    public CanvasGroup contentCanvasGroup;

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
        startUI.transform.position = new Vector3(startUI.transform.position.x, startUI.transform.position.y, 0);
        SlowdownManager.instance.Pause();
    }

    public void Play()
    {
        SceneManager.LoadScene("Gameplay");
        startUI.transform.DOLocalMove(new Vector3(0, 390.2f, 0), 1.5f).SetEase(Ease.InSine).SetUpdate(true).OnComplete(() =>
        {
            startMenuOpen = false;
            SlowdownManager.instance.UnPause();
        });
    }

    public void SnapTo()
    {
        startMenuOpen = true;
        DOTweenAnimationManager.LocalMove(startUI, new Vector3(0, startUI.transform.localPosition.y, startUI.transform.position.z), 0.01f, Ease.Linear, true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartMenu") { startMenuOpen = true; contentCanvasGroup.DOFade(1, 1).SetUpdate(true); DOTweenAnimationManager.LocalMove(startUI, new Vector3(0, 14.2f, 0), 0.01f, Ease.Linear, true); }
        else if (scene.name == "Gameplay") contentCanvasGroup.DOFade(0, 1).SetUpdate(true);
        Debug.Log("OnSceneLoaded " + scene.name);
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
