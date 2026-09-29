using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Waves")]
    [SerializeField] private List<WaveData> waves = new List<WaveData>();
    [SerializeField] private float timeBetweenWaves = 3f;
    [SerializeField] private bool autoStart = true;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    private EnemySpawner spawner;
    private int currentWaveIndex = -1;
    private bool allWavesComplete = false;

    public event Action<int> OnWaveStart;
    public event Action<int> OnWaveComplete;
    public event Action OnAllWavesComplete;

    public int CurrentWaveIndex => currentWaveIndex;
    public int TotalWaves => waves.Count;
    public bool AllWavesComplete => allWavesComplete;

    void Awake()
    {
        spawner = GetComponent<EnemySpawner>();
    }

    void Start()
    {
        if (autoStart) StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        yield return new WaitForSeconds(2f);  // пауза перед первой волной

        for (int i = 0; i < waves.Count; i++)
        {
            currentWaveIndex = i;
            yield return RunSingleWave(waves[i]);
            OnWaveComplete?.Invoke(i);
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        allWavesComplete = true;
        OnAllWavesComplete?.Invoke();
    }

    IEnumerator RunSingleWave(WaveData wave)
    {
        OnWaveStart?.Invoke(currentWaveIndex);

        // Спавним врагов
        for (int i = 0; i < wave.enemyCount; i++)
        {
            Vector3 pos = spawnPoints[i % spawnPoints.Length].position;
            spawner.SpawnEnemy(pos);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        // Ждём, пока все умрут
        while (spawner.AliveCount > 0)
        {
            yield return null;
        }
    }
}

[System.Serializable]
public class WaveData
{
    public int enemyCount = 3;
    public float spawnInterval = 0.5f;
}