using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSlider : MonoBehaviour
{
    public Entity entity;
    public UIText healthText;

    private Slider slider;
    private float velocity;

    private Vector3 scale;

    private void Awake()
    {
        scale = transform.localScale;
    }

    private void OnEnable()
    {
        Entity.OnEntityDamaged += OnEntityDamaged;
    }

    private void OnDisable()
    {
        Entity.OnEntityDamaged -= OnEntityDamaged;
    }

    void Start()
    {
        entity = GetComponentInParent<Entity>();

        slider = GetComponent<Slider>();
        slider.maxValue = entity.maxHealth;

        SetText();
    }

    void Update()
    {
        slider.value = Mathf.SmoothDamp(slider.value, entity.health, ref velocity, 0.2f);
    }

    public void Show()
    {
        transform.localScale = scale;
    }

    public void Hide()
    {
        transform.localScale = Vector3.zero;
    }

    private void SetText()
    {
        healthText.SetText(Mathf.RoundToInt(entity.health).ToString() + " / " + entity.maxHealth.ToString());
    }

    private void OnEntityDamaged(Entity entity)
    {
        if (entity != this.entity) return;

        SetText();
    }
}
