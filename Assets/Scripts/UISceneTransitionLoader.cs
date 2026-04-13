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

public class UISceneTransitionLoader : MonoBehaviour
{
    [SerializeField] private float _fadeInDuration = 1f;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _fadeOutDuration = 1f;
    
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _image;
    [SerializeField] private Volume _globalVolume;
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
        StartCoroutine(FadeCoroutine(color, sceneName));
    }

    private IEnumerator FadeCoroutine(Color color, string sceneName = null)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        float time = 0;
        _image.color = color;
        while (time < _fadeInDuration)
        {
            _canvasGroup.alpha = time / _fadeInDuration;
            time += Time.deltaTime;
            yield return new WaitForNextFrameUnit();
        }
        _canvasGroup.alpha = 1;
        yield return new WaitForSeconds(_fadeDuration);

        

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;



        bool isReady = true;
        while (isReady)
        {
            yield return null;
            if (asyncOperation.progress >= 0.9f)
            {
                isReady = false;   
            }
        }

        foreach (GameObject a in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            bool isUsed = false;
            foreach (Transform child in this.transform)
            {
                if (a == child.gameObject)
                {
                    isUsed = true;
                }
            }
            if (a == gameObject) isUsed = true;
            if (a == transform.parent.gameObject) isUsed = true;
            if(_globalVolume != null) if (a == _globalVolume.gameObject) isUsed = true;

            if (!isUsed) Destroy(a);
        }

        
        
        asyncOperation.allowSceneActivation = true;
        while(!asyncOperation.isDone) yield return new WaitForNextFrameUnit();

        if(_globalVolume != null)
        {
            _globalVolume.enabled = false;
            _globalVolume.enabled = true;
        }

        transform.parent.gameObject.GetComponent<Canvas>().worldCamera = Camera.main ; 
        
        yield return new WaitForNextFrameUnit();
        
  
        time = 0;
        while (time < _fadeOutDuration)
        {
            _canvasGroup.alpha = 1 - (time / _fadeOutDuration);
            time += Time.deltaTime;
            yield return new WaitForNextFrameUnit();
        }

        SceneManager.UnloadSceneAsync(currentScene);
    }
}
