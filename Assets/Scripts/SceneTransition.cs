using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private float _fadeInDuration = 0.5f;
    [SerializeField] private float _minimunLoadingWait = 1f;
    [SerializeField] private float _fadeOutDuration = 0.5f;
    
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
        Color color = sceneTransitions.colors[Random.Range(0, sceneTransitions.colors.Count)];
        sceneTransitions.animations[Random.Range(0,sceneTransitions.animations.Count)].gameObject.SetActive(true);

        //StartCoroutine(FadeCoroutine(color, sceneName));
    }
}
