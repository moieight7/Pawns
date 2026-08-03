using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyUIOnLoad : MonoBehaviour
{
    public static DontDestroyUIOnLoad instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("An instance of DontDestroyUIOnLoad already exists. Deleting the newest one...");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnLevelReset;
    }

    private void OnLevelReset(Scene arg0, LoadSceneMode arg1)
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
