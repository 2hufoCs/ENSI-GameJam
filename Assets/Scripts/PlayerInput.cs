using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance { get; private set; }
    public List<string> keysHeld = new();

    string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        Instance = this;
    }

    void Update()
    {
        GetKeysHeld();
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

/*
    public void OnKeyPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
    }
*/
}
