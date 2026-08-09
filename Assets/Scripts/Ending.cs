using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Ending
{
    public static void FocusOnPlayer()
    {
        Target.instance.OnPlayerKilled();
    }

    public static void BackgroundAnim(RawImage backgroundImage, float fadeAmount, float numberOfFades, float delayBetweenFades)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.PrependInterval(delayBetweenFades);

        float step = fadeAmount / numberOfFades;
        for (int i = 0; i < numberOfFades; i++)
        {
            sequence.Append(backgroundImage.DOFade(step * (i + 1), 0.1f));
            sequence.AppendInterval(delayBetweenFades);
        }

        sequence.Play();
    }
}
