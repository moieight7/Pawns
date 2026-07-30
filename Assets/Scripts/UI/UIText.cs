using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIText : MonoBehaviour
{
    public TextMeshProUGUI textObject, textShadow;

    public float shadowX, shadowY;

    private List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();

    void Awake()
    {
        texts = GetComponentsInChildren<TextMeshProUGUI>().ToList();

        textShadow.transform.localPosition = new Vector3(textShadow.transform.localPosition.x + shadowX, textShadow.transform.localPosition.y + shadowY, textShadow.transform.localPosition.z);
    }

    public void SetText(string value)
    {
        foreach (TextMeshProUGUI text in texts) { text.SetText(value); }
    }
}
