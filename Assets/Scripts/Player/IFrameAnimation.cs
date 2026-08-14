using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFrameAnimation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private float timer, time;
    private bool doAnim = false;

    private Tween IFrameAnim = null;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (doAnim) timer += Time.deltaTime;

        if (timer > time && doAnim)
        {
            timer = 0;

            IFrameAnim.Pause();

            doAnim = false;

            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 255);
        }
    }

    public void DoIFrameAnim(float duration)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (IFrameAnim == null)
        {
            IFrameAnim = spriteRenderer.DOFade(spriteRenderer.color.a / 2, 0.2f).SetAutoKill(false)
            .OnComplete(() => { IFrameAnim.PlayBackwards(); })
            .OnRewind(() => { IFrameAnim.Restart(); })
            .OnPause(() => { spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 255); });
        }

        IFrameAnim.Restart();

        doAnim = true;
        time = duration;
        timer = 0;
    }
}
