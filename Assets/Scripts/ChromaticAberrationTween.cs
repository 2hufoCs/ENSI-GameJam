using UnityEngine;
using DG.Tweening;

public class ChromaticAberrationTween : MonoBehaviour
{
    [SerializeField] Material CrtMat;
    [SerializeField] float baseBlur = .0015f;

    [Header("Hit")]
    [SerializeField] float blurHit = .01f;
    [SerializeField] float durationHit = .5f;

    [Header("Die")]
    [SerializeField] float blurDie;
    [SerializeField] float durationDie;

    [Header("Next Wave")]
    [SerializeField] float blurWave;
    [SerializeField] float durationWave;

    Sequence chromaticSequence;

    void OnEnable()
    {
        Actions.OnPlayerHit += HitTween;
        Actions.OnPlayerDie += DieTween;
        Actions.OnNewWave += WaveTween;
    }

    void OnDisable()
    {
        Actions.OnPlayerHit -= HitTween;
        Actions.OnPlayerDie -= DieTween;
        Actions.OnNewWave -= WaveTween;
    }

    void HitTween(float health)
    {
        if (health <= 0) return;

        CrtMat.SetInt("_UseChromaticAberration", 1);
        CrtMat.SetFloat("_BlurOffset", blurHit);

        chromaticSequence = DOTween.Sequence();
        chromaticSequence.AppendInterval(durationHit);
        chromaticSequence.OnComplete(() => 
        { 
            CrtMat.SetInt("_UseChromaticAberration", 0);
            CrtMat.SetFloat("_BlurOffset", baseBlur);
        });
    }

    void DieTween()
    {
        CrtMat.SetInt("_UseChromaticAberration", 1);
        CrtMat.SetFloat("_BlurOffset", blurDie);

        chromaticSequence = DOTween.Sequence();
        chromaticSequence.AppendInterval(durationDie);
        chromaticSequence.OnComplete(() => 
        { 
            CrtMat.SetInt("_UseChromaticAberration", 0);
            CrtMat.SetFloat("_BlurOffset", baseBlur);
        });
    }

    void WaveTween()
    {
        CrtMat.SetInt("_UseChromaticAberration", 1);
        CrtMat.SetFloat("_BlurOffset", blurWave);

        chromaticSequence = DOTween.Sequence();
        chromaticSequence.AppendInterval(durationWave);
        chromaticSequence.OnComplete(() => 
        { 
            CrtMat.SetInt("_UseChromaticAberration", 0);
            CrtMat.SetFloat("_BlurOffset", baseBlur);
        });
    }
}
