using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;
using UnityEngine.Tilemaps;
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

    public static Tween CanvasGroupFade(CanvasGroup canvasGroup, float endValue, float duration = 1, Ease ease = Ease.Linear, bool ignoreTimescale = false)
    {
        return canvasGroup.DOFade(endValue, duration).SetEase(ease).SetUpdate(ignoreTimescale);
    }

    public static Tween TilemapFade(Tilemap tilemap, float endAlpha, float duration, Ease ease = Ease.Linear, bool ignoreTimescale = false)
    {
        float alpha = tilemap.color.a;
        return DOTween.To(() => alpha, x => alpha = x, endAlpha, duration).SetEase(ease).SetUpdate(ignoreTimescale)
            .OnUpdate(() =>
            {
                tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, alpha);
            });
    }
}
