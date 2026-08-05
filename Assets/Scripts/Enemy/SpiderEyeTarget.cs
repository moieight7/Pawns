using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEyeTarget : MonoBehaviour
{
    [SerializeField] private float cameraTargetDivider;

    private Vector3 startPos;

    private Entity entity;

    void Start()
    {
        entity = GetComponentInParent<Entity>();
        startPos = transform.localPosition;
    }

    void Update()
    {
        if (entity.type == EntityType.Player)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var eyePosition = (mousePosition + (cameraTargetDivider - 1) * entity.transform.position) / cameraTargetDivider;
            transform.position = eyePosition;
        }
        else if (entity.type == EntityType.Enemy)
        {
            var targetPosition = entity.target.position;
            var eyePosition = (targetPosition + (cameraTargetDivider - 1) * entity.transform.position) / cameraTargetDivider;
            transform.position = eyePosition;
        }

        Vector2 clampedPosition = new Vector2(
                        Mathf.Clamp(transform.localPosition.x, -0.2f, 0.2f),
                        Mathf.Clamp(transform.localPosition.y, -0.2f, 0.2f));
        transform.localPosition = clampedPosition;
    }
}
