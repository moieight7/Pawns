using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthSlider : MonoBehaviour
{
    public Entity entity;
    public UIText healthText;
    public Color lifedrainAlertColor;

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
        Entity.OnLifedrainEnabled += OnLifedrainEnabled;

        SceneManager.sceneLoaded += OnLevelReset;
    }

    void Start()
    {
        slider = GetComponent<Slider>();
        SetEntity(GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>());
        SetText();
    }

    void SetEntity(Entity to)
    {
        entity = to;
        slider.maxValue = entity.maxHealth;
        SetText();
    }

    void Update()
    {
        slider.value = Mathf.SmoothDamp(slider.value, entity.health, ref velocity, 0.2f);
    }

    private void SetText()
    {
        healthText.SetText(Mathf.RoundToInt(entity.health).ToString() + " / " + entity.maxHealth.ToString());
    }

    private void OnPlayerDamaged()
    {
        SetText();
    }

    private void OnSwitch(Entity to, Entity from)
    {
        SetEntity(to);
    }

    private void OnLevelReset(Scene scene, LoadSceneMode arg1)
    {
        if (scene.name != "Gameplay") { return; }
        slider = GetComponent<Slider>();
        SetEntity(GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>());
        SetText();
    }

    private void OnLifedrainEnabled()
    {
        Image image = slider.fillRect.gameObject.GetComponent<Image>();
        Color startingColor = slider.fillRect.gameObject.GetComponent<Image>().color;

        image.DOColor(lifedrainAlertColor, 0.75f).SetUpdate(false).SetLoops(4, LoopType.Yoyo).SetEase(Ease.InOutSine); // 217 68 78

        AudioManager.instance.Play("snd_heartbeat", 0.7f, 1);
    }
}
