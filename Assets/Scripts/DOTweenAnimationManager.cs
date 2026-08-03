using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DOTweenAnimationManager
{
    public static void BlendableLocalMoveBy(GameObject gameObj, Vector3 moveBy, float duration = 1, Ease ease = Ease.Linear, bool ignoreTimescale = false)
    {
        gameObj.transform.DOBlendableLocalMoveBy(moveBy, duration).SetEase(ease).SetUpdate(ignoreTimescale);
    }
}
