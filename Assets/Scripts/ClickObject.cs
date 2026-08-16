using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UltEvents;
using System;
using System.Linq;

public class ClickObject : MonoBehaviour
{
    private GameObject lastHovered;

    void Update()
    {
        GameObject hoverTarget = GetHover(out RaycastHit2D hoverHit);
        if (hoverTarget != null && hoverTarget.GetComponent<ClickableObject>() != null && hoverTarget.GetComponent<ClickableObject>().hoverFlag == false)
        {
            hoverTarget.GetComponent<ClickableObject>().OnHoverEvent.Invoke();
            hoverTarget.GetComponent<ClickableObject>().hoverFlag = true;
            if (lastHovered != null) lastHovered.GetComponent<ClickableObject>().hoverFlag = false;
            lastHovered = hoverTarget;
        }
        else if (hoverTarget == null || (hoverTarget != null && hoverTarget.GetComponent<ClickableObject>() == null))
        {
            if (lastHovered != null)
            {
                lastHovered.GetComponent<ClickableObject>().OnHoverStopEvent.Invoke();
                lastHovered.GetComponent<ClickableObject>().hoverFlag = false;
            }
        }
        else if (hoverTarget != null && hoverTarget != lastHovered)
        {
            if (lastHovered != null)
            {
                lastHovered.GetComponent<ClickableObject>().OnHoverStopEvent.Invoke();
                lastHovered.GetComponent<ClickableObject>().hoverFlag = false;
            }
        }
    }

    GameObject GetHover(out RaycastHit2D hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null)
            if (IsPointerOverObject()) target = hit.collider.gameObject;
        return target;
    }

    private bool IsPointerOverObject()
    {
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);

        return results.Count > 0;
    }
}
