using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    [HideInInspector] public bool hoverFlag = false;
    public UltEvents.UltEvent OnClickEvent, OnHoverEvent, OnHoverStopEvent;
}
