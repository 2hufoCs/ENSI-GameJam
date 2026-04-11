using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float moveSpeed;
    [SerializeField] List<string> keysRequirements = new();
    [SerializeField] bool[] keysPressed;

    bool isActive = true;

    void Start()
    {
        keysPressed = new bool[keysRequirements.Count];
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        // Destroy enemy when keys requirements are met
        if (CheckRequirements()) Die();

        // Move enemy towards center
        transform.Translate(moveSpeed * Time.deltaTime * (target.position - transform.position).normalized);
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
        for (int i = 0; i < keysPressed.Length; i++)
        {
            keysPressed[i] = false;
        }

        // Assign true for held keys
        foreach (string keys in PlayerInput.Instance.keysHeld)
        {
            if (keysRequirements.Contains(keys))
            {
                int keyIndex = keysRequirements.IndexOf(keys);
                keysPressed[keyIndex] = true;
            }
        }

        // If all keys are held, return true
        return !keysPressed.Contains(false);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
