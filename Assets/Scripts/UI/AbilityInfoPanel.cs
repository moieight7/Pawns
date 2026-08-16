using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityInfoPanel : MonoBehaviour
{
    public UIText abilityName, abilityDesc;

    private CanvasGroup canvasGroup;
    private Ability ability;

    private Tween showTween, hideTween;

    private bool isVisible = false;

    public bool IsVisible
    {
        get { return isVisible; }
        private set { isVisible = value; }
    }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = new Vector3(target.x, target.y, 0);
    }

    public void SetAbility(Ability ability)
    {
        this.ability = ability;

        abilityName.SetText(this.ability.Name);
        abilityDesc.SetText(this.ability.Description);
    }

    public void Show()
    {
        if (hideTween != null) hideTween.Kill();
        if (showTween == null) showTween = DOTweenAnimationManager.CanvasGroupFade(canvasGroup, 1, 0.5f, Ease.InOutSine, true).OnComplete(() => { showTween = null; }).OnKill(() => { showTween = null; });
        IsVisible = true;
    }

    public void Hide()
    {
        if (showTween != null) showTween.Kill();
        if (hideTween == null) hideTween = DOTweenAnimationManager.CanvasGroupFade(canvasGroup, 0, 0.5f, Ease.InOutSine, true).OnComplete(() => { hideTween = null; }).OnKill(() => { hideTween = null; }); ;
        IsVisible = false;
    }
}
