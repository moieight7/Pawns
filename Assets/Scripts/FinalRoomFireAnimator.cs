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

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            FireBlink(6, LoopType.Yoyo);
        }*/
    }

    public Sequence FireFade(float value, float duration, Ease ease = Ease.Linear, bool ignoreTimescale = false)
    {
        Sequence sequence = DOTween.Sequence();

        foreach (SpriteRenderer sprite in fireSprites) sequence.Join(sprite.DOFade(value, duration).SetEase(ease).SetUpdate(ignoreTimescale));

        sequence.Play();
        return sequence;
    }

    public void FireBlink(int numberOfLoops, LoopType loopType = LoopType.Restart)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.PrependInterval(0.5f);
        foreach (SpriteRenderer sprite in fireSprites) sequence.Join(sprite.DOFade(0, 0.1f));
        foreach (SpriteRenderer sprite in fireSprites) sequence.Insert(1, sprite.DOFade(255, 0.1f));
        sequence.AppendInterval(0.5f);

        sequence.SetLoops(numberOfLoops, loopType);

        sequence.Play();
    }
}
