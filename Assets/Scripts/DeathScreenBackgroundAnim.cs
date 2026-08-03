using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UltEvents;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenBackgroundAnim : MonoBehaviour
{
    public void DoAnimation()
    {
        GetComponent<RawImage>().uvRect = new Rect(0, 0, GetComponent<RawImage>().uvRect.width, GetComponent<RawImage>().uvRect.height);
        if (gameObject.GetComponentInParent<StartMenu>() == null) FindObjectOfType<StartMenu>().GetComponentInChildren<DeathScreenBackgroundAnim>().DoAnimation();

        int numberOfRepetitions = 2, num = 0;
        float moveY = -376;
        float upDuration = 1, downDuration = 0.8f;

        Up();

        void Up()
        {
            gameObject.transform.DOBlendableLocalMoveBy(new Vector3(0, moveY, 0), upDuration).SetEase(Ease.InCubic).SetUpdate(UpdateType.Normal, true).OnComplete(() => { moveY /= 2; num++; if (num < numberOfRepetitions) Down(); });
        }

        void Down()
        {
            gameObject.transform.DOBlendableLocalMoveBy(new Vector3(0, -moveY, 0), downDuration).SetEase(Ease.OutCubic).SetUpdate(UpdateType.Normal, true).OnComplete(() => { upDuration = 0.9f; Up(); });
        }
    }
}
