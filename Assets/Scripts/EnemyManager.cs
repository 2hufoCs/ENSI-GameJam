using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyManager : MonoBehaviour
{
    public static List<EnemyBehaviour> enemies = new();
    public static EnemyBehaviour targetedEnemy;

    [SerializeField] string baseWords;
    [SerializeField] string finalWord = "HARDER";

    int currentWave = 0;
    float maxWaves;
    List<int> mergelessWavesIndexes = new() { 1, 2, 4, 5, 7, 9};
    [SerializeField] float waveInterval;
    bool waveWait;

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
        // Update curve key times
        maxWaves = wordFusion.InitiateKeys(baseWords, finalWord) + mergelessWavesIndexes.Count;
        _enemiesPerWave.keys[^1].time = maxWaves;
        _spawnIntervalPerWave.keys[^1].time = maxWaves;
        _enemyMovespeedPerWave.keys[^1].time = maxWaves;

        //Debug.Log($"there are {maxWaves} waves with given words: {baseWords}");

        StartWave();   
    }

    void OnEnable()
    {
        Actions.OnWin += StopSpawn;
        Actions.OnPlayerDie += StopSpawn;
    }

    void OnDisable()
    {
        Actions.OnWin -= StopSpawn;
        Actions.OnPlayerDie -= StopSpawn;
    }

    void StartWave()
    {
        // Win if word has been fusioned
        if (wordFusion.finalQueue.Count == 0)
        {
            Win();
            return;
        }
        if (!mergelessWavesIndexes.Contains(currentWave)) possibleKeyRequirements = wordFusion.LimitedDequeue(99);

        currentWave++;

        _enemiesLeft = (int)_enemiesPerWave.Evaluate(currentWave);
        _spawnInterval = _spawnIntervalPerWave.Evaluate(currentWave);
        _enemyMovespeed = _enemyMovespeedPerWave.Evaluate(currentWave);

        //Actions.OnNewWave();

        #if UNITY_EDITOR
            Debug.Log($"starting wave {currentWave}, enemiesLeft: {_enemiesLeft}, spawnInterval: {_spawnInterval}, _enemyMovespeed: {_enemyMovespeed}");
        #endif

        // Blur/change lines to show new wave
        waveWait = true;
        Sequence waveSequence = DOTween.Sequence();
        waveSequence.AppendInterval(waveInterval);
        waveSequence.OnComplete(() => { waveWait = false; });
    }

    // Update is called once per frame
    void Update()
    {
        if (freeze || waveWait) return;

        // Go to next wave once all enemies are dead
        if (enemies.Count == 0 && _enemiesLeft <= 0)
        {
            StartWave();
            return;
        }

        _spawnIntervalTimer += Time.deltaTime;
        if (_spawnIntervalTimer > _spawnInterval && _enemiesLeft > 0)
        {
            SpawnEnemy();
        }

        if (!targetedEnemy) targetedEnemy = GetClosestEnemy();
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
        //if (previousTargetedEnemy && previousTargetedEnemy != closestEnemy) previousTargetedEnemy.GetComponent<SpriteRenderer>().color = Color.black;
        //closestEnemy.GetComponent<SpriteRenderer>().color = Color.blue;
        return closestEnemy;
    }

    void StopSpawn()
    {
        freeze = true;
    }

    void Win()
    {
        Actions.OnWin();
        freeze = true;
    }
}
