using System;
using System.Collections.Generic;
using UnityEngine;

public class MergeAnimation : MonoBehaviour
{
    [SerializeField] private float _frameDuration = 0.5f;
    [SerializeField] private float _shakeRange = 5f;
    [SerializeField] private List<GlitchAnimations> _clips = new List<GlitchAnimations>();
    [SerializeField] private GameObject _panelPrefab;
    [SerializeField] private RectTransform _leftPanel;
    [SerializeField] private RectTransform _rightPanel;
    [SerializeField] private float _startDistance;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _moveTime;
    [SerializeField] private float _glitchTime;

    public void Start()
    {

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
    }
    
    
    public void InstantiateAnimation(string letter,Transform parent)
    {
        foreach (GlitchAnimations glitchAnimations in _clips)
        {
            print(glitchAnimations.name);
            print(glitchAnimations.name == letter);
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
