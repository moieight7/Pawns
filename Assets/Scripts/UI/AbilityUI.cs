using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUI : MonoBehaviour
{
    public static AbilityUI instance;

    public GameObject containerPrefab;

    [SerializeField] private List<AbilityUIContainer> abilityUIContainers = new List<AbilityUIContainer>();
    private Tween cooldownAnimation = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("An instance of AbilityUI already exists. Deleting the newest one...");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void PopulateContainerList(List<Ability> abilities)
    {
        if (abilityUIContainers.Count > 0)
        {
            foreach (var container in abilityUIContainers) Destroy(container.gameObject);
            abilityUIContainers.Clear();
        }

        for (int i = 0; i < abilities.Count; i++)
        {
            AbilityUIContainer abilityUIContainer = GameObject.Instantiate(containerPrefab).GetComponent<AbilityUIContainer>();
            abilityUIContainer.transform.SetParent(gameObject.transform, false);
            abilityUIContainers.Add(abilityUIContainer);

            abilityUIContainer.abilityIcon.sprite = abilities[i].Icon;
            abilityUIContainer.abilityOffIcon.sprite = abilities[i].Icon;

            abilityUIContainer.abilityIcon.color = abilities[i].Color;
            abilityUIContainer.abilityOffIcon.color = abilities[i].OffColor;

            if (abilities[i].Type == AbilityType.None) Debug.LogError("AbilityUI.cs attempted to populate a UI container with an invalid ability.");
            abilityUIContainer.ability = abilities[i];
        }

        SetAbilityUI();
    }

    public void SetAbilityUI()
    {
        foreach (var ability in abilityUIContainers)
        {
            ability.SetAbilityNumText();
            ability.SetAbilityIcons();
        }
    }

    public void CooldownAnimation(Ability ability)
    {
        AbilityUIContainer abilityUIContainer = abilityUIContainers.Find(x => x.ability == ability);
        abilityUIContainer.CooldownAnimation();
    }

    public void CancelCooldownAnimation(Ability ability)
    {
        AbilityUIContainer abilityUIContainer = abilityUIContainers.Find(x => x.ability == ability);
        abilityUIContainer.CancelCooldownAnimation();
    }
}
