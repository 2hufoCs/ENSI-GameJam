using UnityEngine;
using DG.Tweening;

public class ChromaticAberrationTween : MonoBehaviour
{
    [SerializeField] float baseBlur = .0015f;

    [Header("Hit")]
    [SerializeField] float blurHit = .01f;
    [SerializeField] float durationHit;

    [Header("Die")]
    [SerializeField] AnimationCurve blurAnimDie;
    [SerializeField] float durationDie;

    [Header("Next Wave")]
    [SerializeField] AnimationCurve blurAnimWave;
    [SerializeField] float durationWave;

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
