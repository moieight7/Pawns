using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTelegraphAnimation : MonoBehaviour
{
    public SpriteRenderer telegraphSpriteRenderer;
    private SpriteRenderer enemySpriteRenderer;

    private float timer, time;
    private bool doAnim = false;

    private Tween TelegraphAnim = null;

    private Color inactive = new Color(255, 255, 255, 0);

    private void Start()
    {
        enemySpriteRenderer = GetComponentInParent<SpriteRenderer>();
        telegraphSpriteRenderer.color = inactive;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) DoTelegraphAnim(new Color(255, 0, 0), 2f);

        if (doAnim)
        {
            telegraphSpriteRenderer.flipX = enemySpriteRenderer.flipX;
            timer += Time.deltaTime;
        }

        if (timer > time && doAnim) StopTelegraphAnim();
    }

    public void DoTelegraphAnim(Color color, float duration, Ease ease = Ease.InOutSine)
    {
        telegraphSpriteRenderer.DOColor(new Color(color.r, color.g, color.b, 0), 0.01f);

        TelegraphAnim = telegraphSpriteRenderer.DOFade(0.5f, 0.2f).SetAutoKill(false).SetEase(ease)
        .OnComplete(() => { TelegraphAnim.PlayBackwards(); })
        .OnRewind(() => { TelegraphAnim.Restart(); })
        .OnPause(() => { telegraphSpriteRenderer.color = inactive; });

        TelegraphAnim.Restart();

        doAnim = true;
        time = duration;
        timer = 0;
    }

    public void StopTelegraphAnim()
    {
        Debug.Log("EnemyAttackTelegraphAnimation stop");
        timer = 0;

        TelegraphAnim.Pause();

        doAnim = false;

        telegraphSpriteRenderer.color = inactive;
    }
}
