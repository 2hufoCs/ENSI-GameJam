using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static List<EnemyBehaviour> enemies = new();
    public static EnemyBehaviour targetedEnemy;

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

        targetedEnemy = GetClosestEnemy();
    }

    void SpawnEnemy()
    {
        Transform randomSpawnpoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
        GameObject newEnemy = Instantiate(enemyPrefab, randomSpawnpoint.position, Quaternion.identity, enemyList);

        _spawnIntervalTimer = 0;
    }

    public EnemyBehaviour GetClosestEnemy()
    {
        if (enemies.Count == 0) return null;

        EnemyBehaviour closestEnemy = enemies[0];
        float minDist = Mathf.Infinity;
        foreach (EnemyBehaviour enemy in enemies)
        {
            float newDist = (PlayerInput.Instance.transform.position - enemy.transform.position).magnitude;
            if (newDist < minDist)
            {
                minDist = newDist;
                closestEnemy = enemy;
            }
        }

        // Change enemy color for debug
        closestEnemy.GetComponent<SpriteRenderer>().color = Color.blue;
        return closestEnemy;
    }
}
