using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject startUI, quitConfirm;
    public CanvasGroup contentCanvasGroup;
    public List<RawImage> startMenuBackgrounds = new List<RawImage>();

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
        startUI.transform.localPosition = new Vector3(startUI.transform.localPosition.x, startUI.transform.localPosition.y, 0);
        SlowdownManager.instance.Pause();
    }

    public void Play()
    {
        Debug.Log("StartMenu Play");
        SceneManager.LoadScene("Gameplay");
        startUI.transform.DOBlendableLocalMoveBy(new Vector3(0, 360.06427f, 0), 1.5f).SetEase(Ease.InSine).SetUpdate(true).OnComplete(() =>
        {
            Debug.Log("StartMenu DoLocalMove");
            startMenuOpen = false;
            SlowdownManager.instance.UnPause();
        });
    }

    public void SnapToMenuOpen()
    {
        //startMenuOpen = true;
        //startUI.transform.position = new Vector3(0, 0, 0);
    }

    public void SnapToMenuClosed()
    {
        startMenuOpen = false;

        List<RawImage> images = new List<RawImage>();
        images = GetComponentsInChildren<RawImage>().ToList();
        foreach (RawImage image in images)
        {
            image.transform.localPosition = Vector3.zero;
        }

        SlowdownManager.instance.UnPause();
    }

    public void OpenMenuVictoryScreen(Rect victoryBackgroundImageRect)
    {
        startMenuOpen = true;
        DOTweenAnimationManager.LocalMove(startUI, new Vector3(-1.7f, 0, 0), 0.01f, Ease.Linear, true);
        GetComponentInChildren<RawImage>().uvRect = victoryBackgroundImageRect;
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartMenu") { 
            startMenuOpen = true; 
            contentCanvasGroup.DOFade(1, 1).SetUpdate(true); 
            DOTweenAnimationManager.Move(contentCanvasGroup.gameObject, new Vector3(-0.0027777785435318949f, -0.013888465240597725f, 0), 0.01f, Ease.Linear, true);
            //contentCanvasGroup.gameObject.transform.position = Vector3.zero;
            startUI.transform.localPosition = Vector3.zero;
            foreach (RawImage image in startMenuBackgrounds) image.gameObject.transform.localPosition = Vector3.zero;
            //DOTweenAnimationManager.Move(startUI, new Vector3(-0.04722222313284874f, 0, 92.20125579833985f), 0.01f, Ease.Linear, true);
        }
        else if (scene.name == "Gameplay") contentCanvasGroup.DOFade(0, 1).SetUpdate(true);
        Debug.Log("OnSceneLoaded " + scene.name);
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
