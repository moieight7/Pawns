using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityNameText : MonoBehaviour
{
    public UIText nameText;

    private void Awake()
    {
        Entity.OnSwitch += OnSwitch;
    }

    private void Start()
    {
        //nameText = GetComponent<UIText>();
    }

    private void OnSwitch(Entity to, Entity from)
    {
        nameText.SetText("You are controlling: <color=yellow>" + to.name + "</color>");
    }
}
