using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeathScreenBackgroundAnim : MonoBehaviour
{
    public void DoAnimation()
    {
        int numberOfRepetitions = 3, num = 0;
        float moveY = -363;
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
