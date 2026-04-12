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

    private void Start()
    {
        _leftPanel.offsetMax = new Vector2(-100,_leftPanel.offsetMax.y);
    }


    [Serializable]
    public class GlitchAnimations
    {
        public string name;
        public List<Sprite> sprites;
    }
}
