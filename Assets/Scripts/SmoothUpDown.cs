using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothUpDown : MonoBehaviour
{
    public float amp;
    public float freq;
    private Vector3 initPos;

    void Start()
    {
        initPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = new Vector3(initPos.x, initPos.y + Mathf.Sin(Time.time * freq) * amp, initPos.z);
    }
}
