using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Camera mainCamera;
    [Range(2, 100)][SerializeField] private float cameraTargetDivider;

    private bool isPlayerAlive = true;

    public static CameraTarget instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("An instance of CameraTarget already exists. Deleting the newest one...");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        Entity.OnPlayerKilled += OnPlayerKilled;
    }

    private void Update()
    {
        if (isPlayerAlive)
        {
            var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var cameraTargetPosition = (mousePosition + (cameraTargetDivider - 1) * playerTransform.position) / cameraTargetDivider;
            transform.position = cameraTargetPosition;
        }
        else
        {
            transform.position = playerTransform.position;
        }
    }

    public void SetTarget(Transform target)
    {
        playerTransform = target;
    }

    private void OnPlayerKilled()
    {
        isPlayerAlive = false;
    }
}
