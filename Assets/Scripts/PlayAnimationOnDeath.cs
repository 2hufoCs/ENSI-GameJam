using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class PlayAnimationOnDeath : MonoBehaviour
{
    [SerializeField] private UnityEvent deathEvent;
    
    void Awake()
    {
        Actions.OnPlayerDie += () =>  { deathEvent?.Invoke(); };
    }
    
    void OnDisable()
    {
        Actions.OnPlayerDie -= () =>  { deathEvent?.Invoke(); };
    }
}
