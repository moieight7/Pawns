using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSlider : MonoBehaviour
{
    public Entity entity;
    public TextMeshProUGUI healthText;

    private Slider slider;
    private float velocity;

    public static PlayerHealthSlider instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("An instance of HealthSlider already exists. Deleting the newest one...");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        Entity.OnSwitch += OnSwitch;
        Entity.OnPlayerDamaged += OnPlayerDamaged;
    }

    void Start()
    {
        slider = GetComponent<Slider>();
        SetEntity();
        SetText();
    }

    void SetEntity()
    {
        entity = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        slider.maxValue = entity.maxHealth;
        SetText();
    }

    void Update()
    {
        slider.value = Mathf.SmoothDamp(slider.value, entity.health, ref velocity, 0.2f);
    }

    private void SetText()
    {
        healthText.text = entity.health.ToString() + " / " + entity.maxHealth.ToString();
    }

    private void OnPlayerDamaged()
    {
        SetText();
    }
    private void OnSwitch()
    {
        SetEntity();
    }
}
