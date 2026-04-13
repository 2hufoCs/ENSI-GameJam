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
    [SerializeField] private Ease _easing;
    [SerializeField] private float _fadeOutDuration = 0.5f;
    [SerializeField] private float _loadingWait = 1f;
    [SerializeField] private float _fadeInDuration = 0.5f;
    
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _background;
    [SerializeField] private List<SceneTransitions> _sceneTransitions;
    [Serializable]
    public class SceneTransitions
    {
        [SerializeField] public List<Color> colors = new List<Color>();
        [SerializeField] public List<UIAnimator> animations = new List<UIAnimator>();
        
    }
        public void FadeOut(string sceneName)
    {
        SceneTransitions sceneTransitions = _sceneTransitions[Random.Range(0, _sceneTransitions.Count)];
        _background.color = sceneTransitions.colors[Random.Range(0, sceneTransitions.colors.Count)];
        sceneTransitions.animations[Random.Range(0,sceneTransitions.animations.Count)].gameObject.SetActive(true);

        Sequence fadeOut = DOTween.Sequence();
        fadeOut.Append(_canvasGroup.DOFade(1, _fadeOutDuration));
        fadeOut.AppendInterval(_loadingWait);
        fadeOut.AppendCallback(() => SceneManager.LoadSceneAsync(sceneName));
        print("start load scene");
        //StartCoroutine(FadeCoroutine(color, sceneName));
    }
}
