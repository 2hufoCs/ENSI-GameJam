using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class CelyansUnityEventListener : MonoBehaviour
{
    [SerializeField] private UnityEvent deathEvent;
    [SerializeField] private UnityEvent playerDamageEvent;
    [SerializeField] private UnityEvent winEvent;
    
    void Awake()
    {
        Actions.OnPlayerDie += () =>  { deathEvent?.Invoke(); };
        Actions.OnPlayerHit += (float a) =>  { playerDamageEvent?.Invoke(); };
        Actions.OnWin += () =>  { winEvent?.Invoke(); };
        
    }
    
    void OnDisable()
    {
        Actions.OnPlayerDie -= () =>  { deathEvent?.Invoke(); };
        Actions.OnPlayerHit -= (float a) =>  { playerDamageEvent?.Invoke(); };
        Actions.OnWin -= () =>  { winEvent?.Invoke(); };
    }
}
