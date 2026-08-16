using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FinalRoomFireAnimator : MonoBehaviour
{
    public List<SpriteRenderer> fireSprites = new List<SpriteRenderer>();

    void Start()
    {
        fireSprites = GetComponentsInChildren<SpriteRenderer>().ToList();
    }

    public Sequence FireFade(float value, float duration, Ease ease = Ease.Linear, bool ignoreTimescale = false)
    {
        Sequence sequence = DOTween.Sequence();

        foreach (SpriteRenderer sprite in fireSprites) sequence.Join(sprite.DOFade(value, duration).SetEase(ease).SetUpdate(ignoreTimescale));

        sequence.Play();
        return sequence;
    }
}
