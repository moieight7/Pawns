using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Pre-Spawn Alert")]
    public GameObject enemySilhouette;

    [Header("Lists")]
    public List<Wave> waves;

    [Header("Specifics")]
    [Tooltip("Delay before spawning enemies. (Default: 1)")]
    public float spawnTimer = 1;
    public float randomSpawnDelayMin = 0.05f;
    public float randomSpawnDelayMax = 0.15f;

    public int currentWave = -1;
    public List<GameObject> aliveEnemies;

    private bool isWaitingForWave = false, waveActive = false, isSpawningEnemies = false;
    public float totalEnemyCountInWave;

    public delegate void WaveChangedAction();
    public static event WaveChangedAction OnWaveChanged;

    public delegate void SpawningOverAction();
    public static event SpawningOverAction OnSpawningOver;

    private void Awake()
    {
        Entity.OnSwitch += OnSwitch;
        Entity.OnEnemyKilled += CheckWaveStatus;
    }

    public void StartSpawning()
    {
        if (!waveActive)
        {
            waveActive = true;
            StartCoroutine(NextWave());
        }
    }

    private void Update()
    {
        if (!waveActive) return;

        foreach (GameObject enemy in aliveEnemies)
        {
            if (enemy == null)
            {
                aliveEnemies.Remove(enemy);
                CheckMidTriggers();
            }
        }

        CheckWaveStatus();
    }

    public void CheckWaveStatus()
    {
        if (!waveActive) return;

        if (aliveEnemies.Count == 0 && currentWave + 1 < waves.Count && !isWaitingForWave) { waves[currentWave].OnWaveEnd.Invoke(); waves[currentWave + 1].aliveEnemiesPercentage = 1; StartCoroutine(NextWave()); }
        else if (aliveEnemies.Count == 0 && currentWave + 1 == waves.Count && !isWaitingForWave) EndSpawning();
    }

    public void CheckMidTriggers()
    {
        if (totalEnemyCountInWave > 0) waves[currentWave].aliveEnemiesPercentage = aliveEnemies.Count / totalEnemyCountInWave;

        Debug.Log("MidWaveTrigger: " + aliveEnemies.Count + " / " + totalEnemyCountInWave + " = " + waves[currentWave].aliveEnemiesPercentage);

        if (waves[currentWave].midWaveTriggers.Count != 0 && waves[currentWave].aliveEnemiesPercentage != 0)
        {
            MidWaveTrigger midWaveTrigger = waves[currentWave].midWaveTriggers[waves[currentWave].currentMidTrigger];
            if (midWaveTrigger.killedEnemiesThreshold >= waves[currentWave].aliveEnemiesPercentage && midWaveTrigger.fired == false)
            {
                Debug.Log("Invoke MidWaveTrigger " + midWaveTrigger.killedEnemiesThreshold + " >= " + waves[currentWave].aliveEnemiesPercentage);
                midWaveTrigger.OnMidWaveTrigger.Invoke();
                midWaveTrigger.fired = true;
            }
        }
    }

    void EndSpawning()
    {
        waveActive = false;
        OnSpawningOver.Invoke();
    }

    public void ResetSpawner()
    {
        currentWave = -1;
        isWaitingForWave = false;
        waveActive = false;

        for (int i = 0; i < aliveEnemies.Count; i++)
        {
            Entity entity = aliveEnemies[i].GetComponent<Entity>();
            Destroy(entity.gameObject);
        }
        aliveEnemies.Clear();

        foreach (Wave wave in waves) if (wave.midWaveTriggers.Count != 0) foreach (MidWaveTrigger midWaveTrigger in wave.midWaveTriggers) midWaveTrigger.fired = false;
    }

    IEnumerator NextWave()
    {
        currentWave++;

        if (OnWaveChanged != null)
            OnWaveChanged();

        isWaitingForWave = true;
        yield return new WaitForSeconds(waves[currentWave].spawnDelay);
        isWaitingForWave = false;

        waves[currentWave].OnWaveStart.Invoke();

        isSpawningEnemies = true;
        List<SpawnLocation> spawn = waves[currentWave].enemies;
        foreach (SpawnLocation point in spawn)
        {
            StartCoroutine(SpawnEnemyCoroutine(point.enemy, point.location));
            yield return new WaitForSeconds(Random.Range(randomSpawnDelayMin, randomSpawnDelayMax));
        }

        totalEnemyCountInWave = aliveEnemies.Count;
        spawnTimer = waves[currentWave].spawnTime;

        isSpawningEnemies = false;
    }

    public void SpawnEnemy(GameObject enemy, Transform location)
    {
        StartCoroutine(SpawnEnemyCoroutine(enemy, location));
    }

    IEnumerator SpawnEnemyCoroutine(GameObject enemy, Transform location)
    {
        GameObject enemyObj = Instantiate(enemy, location.position, Quaternion.identity);
        aliveEnemies.Add(enemyObj);
        enemyObj.SetActive(false);

        #region SilhouetteGFX
        Animator silhouetteAnim;
        GameObject silhouette = Instantiate(enemySilhouette, location.position, Quaternion.identity);
        silhouetteAnim = silhouette.GetComponent<Animator>();
        silhouetteAnim.speed = 1;
        silhouetteAnim.speed /= spawnTimer;

        silhouette.GetComponent<SpriteRenderer>().sprite = enemyObj.transform.GetComponent<SpriteRenderer>().sprite;
        silhouette.transform.localScale = enemyObj.transform.localScale;
        silhouette.transform.localRotation = enemyObj.transform.localRotation;
        #endregion

        yield return new WaitForSeconds(spawnTimer);

        silhouetteAnim.Rebind();
        silhouetteAnim.Update(0f);
        silhouette.SetActive(false);

        enemyObj.SetActive(true);
    }

    private void OnSwitch(Entity to, Entity from)
    {
        aliveEnemies.Remove(to.gameObject);
        aliveEnemies.Add(from.gameObject);
    }

    [System.Serializable]
    public class Wave
    {
        public List<SpawnLocation> enemies;
        public float spawnTime;
        public float spawnDelay = 0;
        public List<MidWaveTrigger> midWaveTriggers;
        public UltEvent OnWaveStart, OnWaveEnd;

        [HideInInspector] public float aliveEnemiesPercentage;
        [HideInInspector] public int currentMidTrigger = 0;
    }

    [System.Serializable]
    public class SpawnLocation
    {
        public GameObject enemy;
        public Transform location;
    }

    [System.Serializable]
    public class MidWaveTrigger
    {
        public float killedEnemiesThreshold;
        public UltEvent OnMidWaveTrigger;

        [HideInInspector] public bool fired = false;
    }
}
