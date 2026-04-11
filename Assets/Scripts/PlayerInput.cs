using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance { get; private set; }
    public List<string> keysHeld = new();

    [SerializeField] float maxHealth = 100;
    float currentHealth; 

    [SerializeField] Transform gunPivot;

    string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    bool freeze;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        Instance = this;
    }

    void OnEnable()
    {
        Actions.OnPlayerDie += FreezePlayer;
    }

    void OnDisable()
    {
        Actions.OnPlayerDie -= FreezePlayer;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (freeze) return;

        GetKeysHeld();

        // Turn gun towards closest enemy
        if (EnemyManager.targetedEnemy) TurnGunTowardsEnemy();
    }

    void GetKeysHeld()
    {
        foreach (char c in _alphabet)
        {
            string letter = c.ToString();
            KeyCode currentKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), letter);

            // Add key
            if (Input.GetKeyDown(currentKeyCode) && !keysHeld.Contains(letter))
            {
                keysHeld.Add(letter);
            }

            // Remove key
            if (Input.GetKeyUp(currentKeyCode) && keysHeld.Contains(letter))
            {
                keysHeld.Remove(letter);
            }
        }
    }

    void FreezePlayer()
    {
        freeze = true;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("hit player, health is now " + currentHealth);
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Actions.OnPlayerHit(currentHealth / maxHealth);

        // Gameover if health reaches 0
        if (currentHealth <= 0)
        {
            Debug.Log("game over");
            Actions.OnPlayerDie();
        }
    }

    void TurnGunTowardsEnemy()
    {
        Vector3 target = EnemyManager.targetedEnemy.transform.position;
        Vector3 dir = (target - transform.position).normalized;

        float angle = Vector3.Angle(Vector3.right, dir);
        angle = target.y < transform.position.y ? -angle : angle;

        gunPivot.localEulerAngles = new Vector3(0, 0, angle);
    }
}
