using UltEvents;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSceneLoadedEvent : MonoBehaviour
{
    public string sceneName;
    public UltEvent OnSceneLoaded;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == sceneName) 
        {
            #if UNITY_EDITOR
            Debug.Log("OnSceneLoadEvent " + scene.name + " " + sceneName);
            #endif
            OnSceneLoaded.Invoke(); 
        }
    }
}
