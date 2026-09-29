using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    private List<Health> aliveEnemies = new List<Health>();

    public event Action<Health> OnEnemyDied;
    public int AliveCount => aliveEnemies.Count;

    public Health SpawnEnemy(Vector3 position)
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemySpawner: не задан enemyPrefab!");
            return null;
        }

        GameObject enemyObj = Instantiate(enemyPrefab, position, Quaternion.identity);
        Health health = enemyObj.GetComponent<Health>();

        if (health == null)
        {
            Debug.LogError("EnemySpawner: у префаба нет компонента Health!");
            return null;
        }

        aliveEnemies.Add(health);
        health.OnDeath += () => HandleEnemyDeath(health);

        return health;
    }

    void HandleEnemyDeath(Health health)
    {
        aliveEnemies.Remove(health);
        OnEnemyDied?.Invoke(health);
    }
}