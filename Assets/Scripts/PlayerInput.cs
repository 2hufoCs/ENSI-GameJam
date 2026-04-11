using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Directions { Right = 0, RightUp = 45, Up = 90, UpLeft = 135, Left = 180, LeftDown = 225, Down = 270, DownRight = 315 }

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance { get; private set; }
    public List<string> keysHeld = new();

    [SerializeField] float maxHealth = 100;
    float currentHealth; 

    [Header("Gun")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Sprite[] gunSprites = new Sprite[8]; // Directions begin from right and go counter-clockwise
    [SerializeField] Transform gunPivot;
    [SerializeField] Transform gunTip;

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

        // Only fire when requirements are met
        if (EnemyManager.targetedEnemy.CheckRequirements(ListToString(keysHeld)))
        {
            Fire();
        }

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

    public string ListToString(List<string> stringList)
    {
        string finalString = "";
        foreach (string str in stringList)
        {
            finalString += str;
        }
        return finalString;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("hit player, health is now " + currentHealth);

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

        // Get exact angle towards enemy
        float angle = Vector3.Angle(Vector3.right, dir);
        angle = target.y < transform.position.y ? -angle : angle;

        // Finding approximated angle
        Directions gunDir;
        if (angle > 0)
        {
            gunDir = angle < 22.5f ? Directions.Right : angle < 67.5f ? Directions.RightUp : angle < 112.5f ? Directions.Up : 
                     angle < 157.5f ? Directions.UpLeft : Directions.Left;
        }
        else
        {
            gunDir = angle > -22.5f ? Directions.Right : angle > -67.5f ? Directions.DownRight : angle > -112.5f ? Directions.Down : 
                     angle > -157.5f ? Directions.LeftDown : Directions.Left;
        }
        Debug.Log(gunDir.ToString());

        gunPivot.localEulerAngles = new Vector3(0, 0, angle);
    }

    public void Fire()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, gunTip.position, Quaternion.identity);

        // Set initial direction
        Vector3 target = EnemyManager.targetedEnemy.transform.position;
        Vector3 dir = (target - transform.position).normalized;

        newProjectile.GetComponent<Projectile>().initialDir = dir;
        newProjectile.GetComponent<Projectile>().initialHeldKeys = ListToString(keysHeld);

        // Reset held keys
        keysHeld = new();
    }
}
