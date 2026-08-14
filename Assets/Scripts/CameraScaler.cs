using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraScaler : MonoBehaviour
{
    public int scale = 6, PPU = 16;
    public CinemachineVirtualCamera cam;

    void Update()
    {
        cam.m_Lens.OrthographicSize = ((Screen.currentResolution.height) / (scale * PPU)) * 0.5f;
    }
}
