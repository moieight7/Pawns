using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UltEvents;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenBackgroundAnim : MonoBehaviour
{
    private List<RawImage> images = new List<RawImage>();

    private void Start()
    {
        images = GetComponentsInChildren<RawImage>().ToList();

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            Debug.Log("DeathScreen OnSceneLoaded"); foreach (RawImage image in images) { StartCoroutine(EnableDelay(image, 0, false)); }
        }
    }

    public void DoAnimation()
    {
        foreach (RawImage image in images) StartCoroutine(EnableDelay(image, 0, true));
        if (gameObject.GetComponent<StartMenu>() == null) FindObjectOfType<StartMenu>().GetComponent<DeathScreenBackgroundAnim>().DoAnimation();
        foreach (RawImage image in images)
        {
            image.uvRect = new Rect(0, 0, image.uvRect.width, image.uvRect.height);

            int numberOfRepetitions = 2, num = 0;
            float moveY = -359.9467f;
            float upDuration = 1, downDuration = 0.8f;

            Up();

            void Up()
            {
                image.gameObject.transform.DOBlendableLocalMoveBy(new Vector3(0, moveY, 0), upDuration).SetEase(Ease.InCubic).SetUpdate(UpdateType.Normal, true).OnComplete(() => { moveY /= 2; num++; if (num < numberOfRepetitions) Down(); });
            }

            void Down()
            {
                image.gameObject.transform.DOBlendableLocalMoveBy(new Vector3(0, -moveY, 0), downDuration).SetEase(Ease.OutCubic).SetUpdate(UpdateType.Normal, true).OnComplete(
                    () => {
                        upDuration = 0.9f; 
                        Up(); 
                    });
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Gameplay")
        {
            Debug.Log("DeathScreen OnSceneLoaded"); foreach (RawImage image in images) { StartCoroutine(EnableDelay(image, 2, false)); }
        }
    }

    private IEnumerator EnableDelay(RawImage image, float delay, bool enable)
    {
        yield return new WaitForSeconds(delay);
        image.enabled = enable;
    }
}
