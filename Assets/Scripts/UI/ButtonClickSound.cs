using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UltEvents;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
    public UltEvent OnClickEvent;

    void Start()
    {
        buttons = gameObject.GetComponentsInChildren<Button>(true).ToList();
        foreach (var button in buttons) button.onClick.AddListener(OnClick); 
    }

    private void OnClick()
    {
        OnClickEvent.Invoke();
    }
}
