using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Transform targetTransform, firePointTransform;
    [SerializeField] private Camera mainCamera;
    [Range(2, 100)][SerializeField] private float cameraTargetDivider;
    [Range(2, 100)][SerializeField] private float firePointDivider;

    private bool isPlayerAlive = true;

    public Transform TargetTransform
    {
        get { return targetTransform; }
        private set { targetTransform = value; }
    }

    public delegate void OnTargetSetAction();
    public static event OnTargetSetAction OnTargetSet;

    public static Target instance;

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
    }

    private void Start()
    {
        SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
        mainCamera.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void Update()
    {
        if (isPlayerAlive)
        {
            var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var cameraTargetPosition = (mousePosition + (cameraTargetDivider - 1) * targetTransform.position) / cameraTargetDivider;
            transform.position = cameraTargetPosition;

            if (!PauseMenu.instance.Paused)
            {
                var firePointPosition = (mousePosition + (firePointDivider - 1) * targetTransform.position) / firePointDivider;
                firePointTransform.position = firePointPosition;
            }
        }
        else { transform.position = targetTransform.position; firePointTransform.position = targetTransform.position; }
    }

    public void SetTarget(Transform target)
    {
        this.targetTransform = target;
        firePointTransform = target.Find("FirePoint");
        if (OnTargetSet != null) OnTargetSet.Invoke();
    }

    public void OnPlayerKilled()
    {
        isPlayerAlive = false;
    }

    public void OnPlayerRevived()
    {
        isPlayerAlive = true;
    }
}
