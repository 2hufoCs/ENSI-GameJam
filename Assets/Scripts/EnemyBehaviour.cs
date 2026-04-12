using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyBehaviour : MonoBehaviour
{
    Transform target;
    public float _moveSpeed;
    [SerializeField] float _speedReductionMult = .15f;
    [SerializeField] float _damage;
    public string keysRequirements;
    [SerializeField] TextMeshProUGUI _keysRequirementsTxt;
    bool[] _keysPressed;

    [HideInInspector] public bool isActive = true;
    [HideInInspector] public bool freeze;

    void Awake()
    {
        EnemyManager.enemies.Add(this);
    }

    void OnDestroy()
    {
        EnemyManager.enemies.Remove(this);
    }

    void OnEnable()
    {
        Actions.OnPlayerDie += WaitForPlayerToDie;
        Actions.OnWin += HideEnemy;
    }

    void OnDisable()
    {
        Actions.OnPlayerDie -= WaitForPlayerToDie;
        Actions.OnWin -= HideEnemy;
    }

    void Start()
    {
        _keysPressed = new bool[keysRequirements.Length];
        target = PlayerInput.Instance.gameObject.transform;

        _keysRequirementsTxt.text = keysRequirements;
        _moveSpeed -= _speedReductionMult * (keysRequirements.Length - 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (freeze) return;

        // Destroy enemy when keys requirements are met
        // if (EnemyManager.targetedEnemy == this && CheckRequirements()) Die();

        if (!isActive) return;

        // Move enemy towards center
        transform.Translate(_moveSpeed * Time.deltaTime * (target.position - transform.position).normalized);
    }

    void OnTriggerEnter(Collider col)
    {
        // Inflict damage to player
        if (col.CompareTag("Player"))
        {
            PlayerInput.Instance.TakeDamage(_damage);

            isActive = false;
            Destroy(gameObject);
        }
    }

    public bool CheckRequirements(string heldKeys)
    {
        // Null error? (must debug)
        if (_keysPressed == null)
        {
            Debug.Log("keys pressed bool list is null, don't check requirements");
            return false;
        }

        // Reset pressed keys
        for (int i = 0; i < _keysPressed.Length; i++)
        {
            _keysPressed[i] = false;
        }

        // Assign true for held keys
        foreach (char _key in keysRequirements)
        {
            if (!heldKeys.Contains(_key.ToString()))
            {
                return false;
            }
        }
        return true;


        // // Assign true for held keys
        // foreach (string keys in PlayerInput.Instance.keysHeld)
        // {
        //     if (_keysRequirements.Contains(keys))
        //     {
        //         int keyIndex = _keysRequirements.IndexOf(keys);
        //         _keysPressed[keyIndex] = true;
        //     }
        // }

        // // If all keys are held, return true
        // return !_keysPressed.Contains(false);
    }

    public void Hit(string heldKeys)
    {
        //Debug.Log($"enemy requirements: {keysRequirements}, keys held: {heldKeys}");
        if (CheckRequirements(heldKeys)) Die();
    }

    public void Die()
    {
        PlayerInput.Instance.keysHeld = new();
        Destroy(gameObject);
    }

    void WaitForPlayerToDie()
    {
        // Wait for end of player die anim, then hide sprites

        freeze = false;
        HideEnemy();
    }

    void HideEnemy()
    {
        isActive = false;
        freeze = true;
        GetComponent<SpriteRenderer>().enabled = false;
        _keysRequirementsTxt.enabled = false;
    }
}
