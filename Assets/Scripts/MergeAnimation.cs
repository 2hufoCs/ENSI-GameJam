using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class MergeAnimation : MonoBehaviour
{
    [SerializeField] private float _frameDuration = 0.5f;
    [SerializeField] private float _shakeRange = 5f;
    [SerializeField] private List<GlitchAnimations> _clips = new List<GlitchAnimations>();
    [SerializeField] private List<GlitchAnimations> _glitchClips = new List<GlitchAnimations>();
    [SerializeField] private GameObject _panelPrefab;
    [SerializeField] private RectTransform _leftPanel;
    [SerializeField] private RectTransform _rightPanel;
    [SerializeField] private float _startDistance;
    [SerializeField,CurveRange(0,0,1,1)] private AnimationCurve _movementCurve;
    [SerializeField] private float _moveTime;
    [SerializeField] private float _glitchTime;
    [SerializeField] private GameObject _glitchPrefab;

    private float _moveTimer = float.PositiveInfinity;
    private float _glitchTimer = float.PositiveInfinity;
    private GameObject _glitchObject;
    
    public void Update()
    {
        if (_moveTimer <= _moveTime)
        {
            _moveTimer += Time.deltaTime;
            float distance = _startDistance * _movementCurve.Evaluate(_moveTimer / _moveTime);
            _leftPanel.offsetMax = new Vector2(-distance , _leftPanel.offsetMax.y);
            _rightPanel.offsetMin= new Vector2(distance , _leftPanel.offsetMin.y);
            if (_moveTimer >= _moveTime)
            {
                _glitchTimer = 0;
                GameObject instantiate = Instantiate(_glitchPrefab,transform);
                instantiate.name = "Glitch";
                instantiate.GetComponent<UIAnimator>().frameDuration = _frameDuration;
                instantiate.GetComponent<UIAnimator>().frames = _glitchClips[Random.Range(0,_glitchClips.Count)].sprites;
                instantiate.GetComponent<UISimpleShake>().shakeInterval = _frameDuration / 2;
                instantiate.GetComponent<UISimpleShake>().range = _shakeRange;
                _glitchObject = instantiate;
            }
        }

        if (_glitchTimer <= _glitchTime)
        {
            _glitchTimer += Time.deltaTime;
            if (_glitchTimer >= _glitchTime)
            {
                foreach (Transform child in _leftPanel.transform)
                {
                    Destroy(child.gameObject);
                }
                foreach (Transform child in _rightPanel.transform)
                {
                    Destroy(child.gameObject);
                }
                Destroy(_glitchObject);
            }
        }
    }

    public void StartAnimation(List<string> oldList, List<string> newList)
    {
        string leftstring = "";
        string rightstring = "";
        for (int i = 0; i < newList.Count; i++)
        {
            if (newList[i] != oldList[i])
            {
                leftstring = oldList[i];
                rightstring = oldList[i+1];
            }
        }
        print(leftstring + " + " + rightstring);
        foreach (char character in leftstring)
        {
            InstantiateAnimation(character.ToString(),_leftPanel);
        }
        foreach (char character in rightstring)
        {
            InstantiateAnimation(character.ToString(),_rightPanel);
        }
        _moveTimer = 0;
    }
    
    
    public void InstantiateAnimation(string letter,Transform parent)
    {
        print(letter);
        foreach (GlitchAnimations glitchAnimations in _clips)
        {
            if (glitchAnimations.name == letter)
            {
                GameObject panel = Instantiate(_panelPrefab, parent);
                panel.name = letter;
                UIAnimator animator = panel.GetComponentInChildren<UIAnimator>();
                animator.frameDuration = _frameDuration;
                animator.frames = glitchAnimations.sprites;
                UISimpleShake simpleShake = panel.GetComponentInChildren<UISimpleShake>();
                simpleShake.range = _shakeRange;
                simpleShake.shakeInterval = _frameDuration / 2;
                break;
            }
        }
    }
    
    
    [Serializable]
    public class GlitchAnimations
    {
        public string name;
        public List<Sprite> sprites;
    }
    
}
