using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTargetSwitcher : MonoBehaviour
{
    private Transform target;
    private List<Entity> enemies = new List<Entity>();

    public static EnemyTargetSwitcher instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("An instance of EnemyTargetSwitcher already exists. Deleting the newest one...");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        enemies = FindObjectsOfType<Entity>().ToList();
    }

    public void SetNewTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        foreach (Entity enemy in enemies) enemy.target = target;
    }
}
