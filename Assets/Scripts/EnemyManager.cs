using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static List<EnemyBehaviour> enemies;

    [SerializeField] float _spawnInterval;
    float _spawnIntervalTimer;

    [Header("References")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform enemyList;
    [SerializeField] List<Transform> _spawnPoints;

    void Start()
    {
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        _spawnIntervalTimer += Time.deltaTime;
        if (_spawnIntervalTimer > _spawnInterval)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Transform randomSpawnpoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
        GameObject newEnemy = Instantiate(enemyPrefab, randomSpawnpoint.position, Quaternion.identity, enemyList);

        _spawnIntervalTimer = 0;
    }
}
