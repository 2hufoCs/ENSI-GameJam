using UnityEngine;
using System;

public class Actions : MonoBehaviour
{
    public static Action OnEnemyHit;
    public static Action OnEnemyKilled;

    public static Action<float> OnPlayerHit;
    public static Action OnPlayerDie;
    public static Action OnWin;
    public static Action OnNewWave;
}
