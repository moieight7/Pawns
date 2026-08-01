using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float x, y;

    private RawImage image;

    private void Start()
    {
        image = GetComponent<RawImage>();

        //StartCoroutine(Scroll());
    }

    void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(x, y) * Time.unscaledDeltaTime, image.uvRect.size);
    }

    IEnumerator Scroll()
    {
        while (true)
        {
            image.uvRect = new Rect(image.uvRect.position + new Vector2(x, y) * Time.unscaledDeltaTime, image.uvRect.size);
        }
    }
}
