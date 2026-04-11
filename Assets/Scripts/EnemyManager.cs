using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static List<EnemyBehaviour> enemies = new();
    public static EnemyBehaviour targetedEnemy;

    [SerializeField] string baseWord;

    float currentWave = 0;

    List<string> possibleKeyRequirements;

    [SerializeField] AnimationCurve _enemiesPerWave;
    int _enemiesLeft;

    [SerializeField] AnimationCurve _spawnIntervalPerWave;
    float _spawnInterval;
    float _spawnIntervalTimer;

    [SerializeField] AnimationCurve _enemyMovespeedPerWave;
    float _enemyMovespeed;

    [Header("References")]
    [SerializeField] WordFusion wordFusion;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform enemyList;
    [SerializeField] List<Transform> _spawnPoints;

    bool freeze = false;

    void Start()
    {
        wordFusion.GenerateStack(baseWord);
        StartWave();   
    }

    void OnEnable()
    {
        Actions.OnGameOver += FreezeEnemies;
    }

    void OnDisable()
    {
        Actions.OnGameOver -= FreezeEnemies;
    }

    void StartWave()
    {
        // Win if word has been fusioned
        if (wordFusion.finalStack.Count == 0)
        {
            Win();
            return;
        }
        possibleKeyRequirements = wordFusion.finalStack.Pop();

        currentWave++;

        Debug.Log("starting wave " + currentWave);

        _enemiesLeft = (int)_enemiesPerWave.Evaluate(currentWave);
        _spawnInterval = _spawnIntervalPerWave.Evaluate(currentWave);
        _enemyMovespeed = _enemyMovespeedPerWave.Evaluate(currentWave);

        Debug.Log($"enemies: {_enemiesLeft}, spawn interval: {_spawnInterval}");

        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (freeze) return;

        // Go to next wave once all enemies are dead
        if (enemies.Count == 0 && _enemiesLeft == 0)
        {
            StartWave();
            return;
        }

        _spawnIntervalTimer += Time.deltaTime;
        if (_spawnIntervalTimer > _spawnInterval && _enemiesLeft > 0)
        {
            SpawnEnemy();
        }

        targetedEnemy = GetClosestEnemy();
    }

    void SpawnEnemy()
    {
        Transform randomSpawnpoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
        GameObject newEnemy = Instantiate(enemyPrefab, randomSpawnpoint.position, Quaternion.identity, enemyList);

        EnemyBehaviour enemyBehaviour = newEnemy.GetComponent<EnemyBehaviour>();
        enemyBehaviour.keysRequirements = possibleKeyRequirements[Random.Range(0, possibleKeyRequirements.Count)];
        enemyBehaviour._moveSpeed = _enemyMovespeed;

        _spawnIntervalTimer = 0;
        _enemiesLeft--;
        //Debug.Log(_enemiesLeft + " enemies left");
    }

    public EnemyBehaviour GetClosestEnemy()
    {
        if (enemies.Count == 0) return null;

        EnemyBehaviour previousTargetedEnemy = targetedEnemy;
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
        if (previousTargetedEnemy && previousTargetedEnemy != closestEnemy) previousTargetedEnemy.GetComponent<SpriteRenderer>().color = Color.black;
        closestEnemy.GetComponent<SpriteRenderer>().color = Color.blue;
        return closestEnemy;
    }

    void FreezeEnemies()
    {
        foreach (EnemyBehaviour enemy in enemies)
        {
            enemy.isActive = false;
        }
    }

    void Win()
    {
        Debug.Log("you win!!!");
        freeze = true;
    }
}
