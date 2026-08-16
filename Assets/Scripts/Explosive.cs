using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class Explosive : Projectile
{
    [Header("Explosive Stats")]
    public float explodeDamageMin;
    public float explodeDamageMax;
    public float explodeRadius;
    public float explodeTime;

    public Color explosionOutlineStartColor, explosionOutlineEndColor;
    public UltEvent OnExplodeEvent;

    private DrawCircle explosionOutline;
    private Tween movementTween;

    protected override void Start()
    {
        base.Start();

        movementTween = DOTween.To(() => rb.velocity, x => rb.velocity = x, Vector2.zero, 2).SetEase(Ease.OutSine);

        explosionOutline = GetComponent<DrawCircle>();
        explosionOutline.SetCircleRadius(0, 0);
        explosionOutline.SetCircleRadius(explodeRadius, explodeTime * 0.75f);

        Color2 start = new Color2(new Color(explosionOutlineStartColor.r, explosionOutlineStartColor.g, explosionOutlineStartColor.b, explosionOutlineStartColor.a), new Color(explosionOutlineStartColor.r, explosionOutlineStartColor.g, explosionOutlineStartColor.b, explosionOutlineStartColor.a));
        Color2 end = new Color2(new Color(explosionOutlineEndColor.r, explosionOutlineEndColor.g, explosionOutlineEndColor.b, explosionOutlineEndColor.a), new Color(explosionOutlineEndColor.r, explosionOutlineEndColor.g, explosionOutlineEndColor.b, explosionOutlineEndColor.a));
        explosionOutline.SetColor(start, end, explodeTime * 0.85f, Ease.InOutSine);
    }

    protected override void Update()
    {
        base.Update();
        if (timer > explodeTime) { Destroy(gameObject); }
    }

    public void SetMovementTween(float mult)
    {
        if (movementTween != null) movementTween.Kill();
        movementTween = DOTween.To(() => rb.velocity * mult, x => rb.velocity = x, Vector2.zero, 2).SetEase(Ease.OutSine);
    }

    private void OnDestroy()
    {
        Explode();
        OnExplodeEvent.Invoke();
    }

    private void Explode()
    {
        LayerMask hitLayer = new LayerMask();
        if (sender.GetComponentInParent<Entity>().type == EntityType.Player) hitLayer = LayerMask.GetMask("Enemy");
        else if (sender.GetComponentInParent<Entity>().type == EntityType.Enemy) hitLayer = LayerMask.GetMask("Player");
        Collider2D[] hitObjectsArray = Physics2D.OverlapCircleAll(transform.position, explodeRadius, hitLayer);

        foreach (Collider2D hit in hitObjectsArray)
        {
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            hit.GetComponent<Entity>().TakeDamage(Mathf.FloorToInt(Mathf.Lerp(explodeDamageMax, explodeDamageMin, distance / explodeRadius)));
        }
    }
}
