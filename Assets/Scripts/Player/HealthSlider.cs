using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    public Entity entity;
    public TextMeshProUGUI healthText;

    private Slider slider;
    private float velocity;

    private void Awake()
    {
        Entity.OnPlayerDamaged += OnPlayerDamaged;
    }

    void Start()
    {
        slider = GetComponent<Slider>();
        SetEntity();
        OnPlayerDamaged();
    }

    void SetEntity()
    {
        entity = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        slider.maxValue = entity.maxHealth;
    }

    void Update()
    {
        slider.value = Mathf.SmoothDamp(slider.value, entity.health, ref velocity, 0.2f);
    }

    private void OnPlayerDamaged()
    {
        healthText.text = entity.health.ToString() + " / " + entity.maxHealth.ToString();
    }
}
