using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSceneLoadedEvent : MonoBehaviour
{
    public string sceneName;
    public UltEvent OnSceneLoaded;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == sceneName) { Debug.Log("OnSceneLoad " + scene.name + " " + sceneName); OnSceneLoaded.Invoke(); }
    }
}
