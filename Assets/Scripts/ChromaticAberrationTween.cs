using UnityEngine;

public class ChromaticAberrationTween : MonoBehaviour
{
    void OnEnable()
    {
        Actions.OnPlayerHit += HitTween;
        Actions.OnPlayerDie += DieTween;
        Actions.OnNewWave += WaveTween;
    }

    void OnDisable()
    {
        Actions.OnPlayerHit += HitTween;
        Actions.OnPlayerDie += DieTween;
        Actions.OnNewWave += WaveTween;
    }

    void HitTween(float temp)
    {
        
    }

    void DieTween()
    {
        
    }

    void WaveTween()
    {
        
    }
}
