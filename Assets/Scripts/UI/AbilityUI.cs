using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUI : MonoBehaviour
{
    public List<AbilityUIContainer> abilityUIContainers = new List<AbilityUIContainer>();

    public static AbilityUI instance;

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

    public void PopulateContainerList(List<PlayerAbility> abilities)
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            if (abilities[i].type == AbilityType.None) Debug.LogError("AbilityUI.cs attempted to populate a UI container with an invalid ability.");
            abilityUIContainers[i].ability = abilities[i];
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

        abilityUIContainer.abilityIcon.fillAmount = 0;
        cooldownAnimation = abilityUIContainer.abilityIcon.DOFillAmount(1, ability.cooldownTime).SetEase(Ease.Linear).OnComplete(() => { cooldownAnimation = null; });
    }
}
