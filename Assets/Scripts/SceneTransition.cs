using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Ease _easeType;
    [SerializeField] private float _fadeOutDuration = 0.5f;
    [SerializeField] private float _loadingWait = 1f;
    [SerializeField] private float _fadeInDuration = 0.5f;
    [SerializeField] private bool _forceFadeIn;
    [SerializeField] private bool _forceBlockFadeIn;

    [Required]
    [SerializeField] private CanvasGroup _canvasGroup;
    [Required]
    [SerializeField] private Image _background;
    [SerializeField] private List<SceneTransitions> _sceneTransitions;
    [Serializable]
    public class SceneTransitions
    {
        [SerializeField] public List<Color> colors = new List<Color>();
        [SerializeField] public List<UIAnimator> animations = new List<UIAnimator>();
        
    }
    public void OnEnable()
    {
        if (Resources.Load<SceneTransitionsData>("SceneTransitionsData").sceneTransitionIndex >= 0)
        {
            if (_forceBlockFadeIn)
            {
                ResetSceneTransitionsData();
            }
            else
            {
                ApplySceneTransitionsData();
                CreateAnimation(0);
                ResetSceneTransitionsData();
            }
        }
        else if (_forceFadeIn && !_forceBlockFadeIn)
        {
            GenerateRandomSceneTransitionsData();
            ApplySceneTransitionsData();
            CreateAnimation(0);
            ResetSceneTransitionsData();
        }
    }

    public void FadeOut(string sceneName)
    {
        GenerateRandomSceneTransitionsData();
        ApplySceneTransitionsData();
        CreateAnimation(1,sceneName);
    }

    public void CreateAnimation(int finalCanvaAlpha, string sceneName = null)
    {
        _canvasGroup.alpha = 1 - finalCanvaAlpha;
        Sequence fadeOut = DOTween.Sequence();
        fadeOut.Append(_canvasGroup.DOFade(finalCanvaAlpha, _fadeOutDuration)).SetEase(_easeType);
        fadeOut.AppendInterval(_loadingWait);
        if(sceneName != null) fadeOut.AppendCallback(() => SceneManager.LoadSceneAsync(sceneName));
    }

    private void GenerateRandomSceneTransitionsData()
    {
        int sceneTransitionIndex = Random.Range(0, _sceneTransitions.Count);
        SceneTransitions sceneTransitions = _sceneTransitions[sceneTransitionIndex];
        Resources.Load<SceneTransitionsData>("SceneTransitionsData").sceneTransitionIndex = sceneTransitionIndex;
        Resources.Load<SceneTransitionsData>("SceneTransitionsData").uIAnimatorIndex = Random.Range(0, sceneTransitions.colors.Count);
        Resources.Load<SceneTransitionsData>("SceneTransitionsData").colorIndex = Random.Range(0,sceneTransitions.animations.Count);
    }

    private void ApplySceneTransitionsData()
    {
        int sceneTransitionIndex = Resources.Load<SceneTransitionsData>("SceneTransitionsData").sceneTransitionIndex;
        if (sceneTransitionIndex >= 0)
        {
            SceneTransitions sceneTransitions = _sceneTransitions[sceneTransitionIndex];
            _background.color = sceneTransitions.colors[Resources.Load<SceneTransitionsData>("SceneTransitionsData").uIAnimatorIndex];
            sceneTransitions.animations[Resources.Load<SceneTransitionsData>("SceneTransitionsData").colorIndex].gameObject.SetActive(true);
        }
    }


    private void ResetSceneTransitionsData()
    {
        Resources.Load<SceneTransitionsData>("SceneTransitionsData").sceneTransitionIndex = -1;
        Resources.Load<SceneTransitionsData>("SceneTransitionsData").uIAnimatorIndex = -1;
        Resources.Load<SceneTransitionsData>("SceneTransitionsData").colorIndex = -1;
    }
}
