using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int subdivisions = 10;
    public float radius = 0.1f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        SetPositions();
    }

    private void SetPositions()
    {
        float angleStep = 2f * Mathf.PI / subdivisions;
        lineRenderer.positionCount = subdivisions + 1;

        for (int i = 0; i <= subdivisions; i++)
        {
            float xPosition = radius * Mathf.Cos(angleStep * i);
            float yPosition = radius * Mathf.Sin(angleStep * i);

            Vector3 pointInCircle = new Vector3(xPosition, yPosition, 0);

            lineRenderer.SetPosition(i, pointInCircle);
        }
    }

    public void SetCircleRadius(float radius, float duration, Ease ease = Ease.OutSine)
    {
        DOTween.To(() => this.radius, x => this.radius = x, radius, duration).SetEase(ease);
    }

    public void SetColor(Color2 startColor, Color2 endColor, float duration, Ease ease = Ease.OutSine)
    {
        lineRenderer.DOColor(startColor, endColor, duration).SetEase(ease);
    }
}
