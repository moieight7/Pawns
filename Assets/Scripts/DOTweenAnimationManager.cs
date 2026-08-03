using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;
using UnityEngine.UI;

public static class DOTweenAnimationManager
{
    public static void Move(GameObject gameObj, Vector3 moveTo, float duration = 1, Ease ease = Ease.Linear, bool ignoreTimescale = false)
    {
        gameObj.transform.DOMove(moveTo, duration).SetEase(ease).SetUpdate(ignoreTimescale);
    }

    public static void LocalMove(GameObject gameObj, Vector3 moveTo, float duration = 1, Ease ease = Ease.Linear, bool ignoreTimescale = false)
    {
        gameObj.transform.DOLocalMove(moveTo, duration).SetEase(ease).SetUpdate(ignoreTimescale);
    }

    public static void BlendableLocalMoveBy(GameObject gameObj, Vector3 moveBy, float duration = 1, Ease ease = Ease.Linear, bool ignoreTimescale = false)
    {
        gameObj.transform.DOBlendableLocalMoveBy(moveBy, duration).SetEase(ease).SetUpdate(ignoreTimescale);
    }

    public static void RawImageFade(RawImage image, float endValue, float duration = 1, Ease ease = Ease.Linear, bool ignoreTimescale = false)
    {
        image.DOFade(endValue, duration).SetEase(ease).SetUpdate(ignoreTimescale);
    }

    public static void CanvasGroupFade(CanvasGroup canvasGroup, float endValue, float duration = 1, Ease ease = Ease.Linear, bool ignoreTimescale = false)
    {
        canvasGroup.DOFade(endValue, duration).SetEase(ease).SetUpdate(ignoreTimescale);
    }
}
