using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    Transform target;
    [SerializeField] float _moveSpeed;
    [SerializeField] string _keysRequirements;
    [SerializeField] TextMeshProUGUI _keysRequirementsTxt;
    bool[] _keysPressed;

    bool isActive = true;
    bool isTargeted;

    void Awake()
    {
        EnemyManager.enemies.Add(this);
    }

    void OnDestroy()
    {
        EnemyManager.enemies.Remove(this);
    }

    void Start()
    {
        _keysPressed = new bool[_keysRequirements.Length];
        target = PlayerInput.Instance.gameObject.transform;

        _keysRequirementsTxt.text = _keysRequirements;
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy enemy when keys requirements are met
        if (EnemyManager.targetedEnemy == this && CheckRequirements()) Die();

        if (!isActive) return;

        // Move enemy towards center
        transform.Translate(_moveSpeed * Time.deltaTime * (target.position - transform.position).normalized);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("hit player");
            isActive = false;
        }
    }

    bool CheckRequirements()
    {
        // Reset pressed keys
        for (int i = 0; i < _keysPressed.Length; i++)
        {
            _keysPressed[i] = false;
        }

        // Assign true for held keys
        foreach (char _key in _keysRequirements)
        {
            if (!PlayerInput.Instance.keysHeld.Contains(_key.ToString()))
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

    void Die()
    {
        PlayerInput.Instance.keysHeld = new();
        Destroy(gameObject);
    }
}
