using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float _moveSpeed;
    [SerializeField] string _keysRequirements;
    [SerializeField] bool[] _keysPressed;

    bool isActive = true;

    void Awake()
    {
        EnemyManager.enemies.Add(this);
    }

    void Start()
    {
        _keysPressed = new bool[_keysRequirements.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        // Destroy enemy when keys requirements are met
        if (CheckRequirements()) Die();

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
        Destroy(gameObject);
    }
}
