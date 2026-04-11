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
    [SerializeField] Transform gunPivot;
    [SerializeField] Transform gunTip;

    [Header("Graphics")]
    [SerializeField] Sprite[] taskManagerSprites = new Sprite[8];
    [SerializeField] Sprite[] gunSprites = new Sprite[8]; // Directions begin from right and go counter-clockwise
    [SerializeField] SpriteRenderer taskManagerSpriteRenderer;
    [SerializeField] SpriteRenderer gunSpriteRenderer;


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
        Vector3 dir = (target - gunTip.position).normalized;

        // Get exact angle towards enemy
        float angle = Vector3.Angle(Vector3.right, dir);
        angle = target.y < gunTip.position.y ? -angle : angle;

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
        // Show corresponding sprites
        int dirIndex = Array.IndexOf(Enum.GetValues(typeof(Directions)), gunDir);
        taskManagerSpriteRenderer.sprite = taskManagerSprites[dirIndex];
        gunSpriteRenderer.sprite = gunSprites[dirIndex];

        // Offset gun
        Vector2 gunOffsetPos = gunDir == Directions.Right ? new Vector2(.7f, 0) : gunDir == Directions.Left ? new Vector2(-.7f, 0) : 
                               gunDir == Directions.Up ? new Vector2(.2f, .8f) : gunDir == Directions.Down ? new Vector2(-.2f, -.4f) : 
                               gunDir == Directions.RightUp ? new Vector2(.5f, .5f) : gunDir == Directions.UpLeft ? new Vector2(-.3f, .3f) :
                               gunDir == Directions.LeftDown ? new Vector2(-.5f, -.2f) : new Vector2(-.15f, -.3f); 
        gunSpriteRenderer.sortingOrder = gunDir == Directions.Down || gunDir == Directions.LeftDown || gunDir == Directions.DownRight ? 2 : 1;
        taskManagerSpriteRenderer.sortingOrder = 3 - gunSpriteRenderer.sortingOrder;
        gunSpriteRenderer.transform.localPosition = gunOffsetPos;

        // TODO: rotate gun
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
