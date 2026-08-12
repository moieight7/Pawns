using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UltEvents;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenBackgroundAnim : MonoBehaviour
{
    private List<RawImage> images = new List<RawImage>();

    private void Start()
    {
        images = GetComponentsInChildren<RawImage>().ToList();
    }

    public void DoAnimation()
    {
        if (gameObject.GetComponent<StartMenu>() == null) FindObjectOfType<StartMenu>().GetComponent<DeathScreenBackgroundAnim>().DoAnimation();
        foreach (RawImage image in images)
        {
            image.uvRect = new Rect(0, 0, image.uvRect.width, image.uvRect.height);

            int numberOfRepetitions = 2, num = 0;
            float moveY = -374.5f;
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
}
