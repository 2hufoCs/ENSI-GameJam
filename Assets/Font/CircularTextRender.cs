using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class CircularTextRender : MonoBehaviour
{
    [SerializeField] private List<TextRenderCharacters> letters;
    public string inputText;
    [SerializeField] private float fontSize;
    [SerializeField,MinMaxSlider(0f,50f)] private Vector2 distanceRange;
    [SerializeField,MinMaxSlider(-20f,20f)] private Vector2 angleNoise;
    [SerializeField] private TextRender textRender;
    [CurveRange(0, 0, 10, 10)]
    [SerializeField] private AnimationCurve shakeDistanceCurve;
    [CurveRange(0, 0.01f, 10, 0.5f)]
    [SerializeField] private AnimationCurve shakeIntervalCurve;
    [SerializeField] private List<AnimationRange> animationRanges;


    private bool _isGlowing;
    public bool IsGlowing
    {
        get
        {
            return _isGlowing;
        }
        set
        {
            _isGlowing = value;
            GlowStateChange();
        }
    }

    void GlowStateChange()
    {
        
    }
    
    [Button]
    public void UpdateText()
    {
        if (Application.isPlaying) DeleteChild();
        
        for (int i = 0 ; i < inputText.Length ;i++)
        {
            float angle = 360f / inputText.Length * i;
            angle += Random.Range(angleNoise.x*10, angleNoise.y*10)/10;
            
            Vector3 position = Quaternion.AngleAxis(angle,Vector3.forward ) * Vector3.up;
            position *= Random.Range(distanceRange.x*10, distanceRange.y*10)/10;
            TextRenderCharacters textRenderCharacters = FindLetter(inputText[i]);
            if (textRenderCharacters != null)
            {
                InstantiateCharacters(textRenderCharacters , position);
            }
        }
        
        InstantiateAnimation();
        
        textRender.inputText = inputText;
        textRender.UpdateText();
    }

    private void InstantiateAnimation()
    {
        List<int> Indexs = new List<int>();
        for (int i = 0; i < animationRanges.Count; i++)
        {
            if(animationRanges[i].IsInRange(inputText.Length)) Indexs.Add(i) ;
        }

        AnimationRange selectedAnimation;
        if (Indexs.Count < 1)
        {
            selectedAnimation = animationRanges[^1];
        }
        else
        {
            selectedAnimation = animationRanges[Indexs[Random.Range(0, Indexs.Count)]];
        }
        GameObject animationGo = new GameObject("Animation");
        animationGo.transform.SetParent(transform);
        animationGo.transform.localPosition = Vector3.zero;
        animationGo.AddComponent<Image>().rectTransform.sizeDelta = selectedAnimation.frames[0].rect.size * selectedAnimation.scale;
        UIAnimator goAnimation = animationGo.AddComponent<UIAnimator>();
        goAnimation.frameDuration = selectedAnimation.frameDuration;
        goAnimation.frames = selectedAnimation.frames;
    }
    
    private void DeleteChild()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject != textRender.gameObject) Destroy(child.gameObject);
        }
    }
    
    private void InstantiateCharacters(TextRenderCharacters textRenderCharacters,Vector2 position)
    {
        if(textRenderCharacters.sprite != null)
        {
            GameObject characterGo = new GameObject(textRenderCharacters.letter.ToString());
            characterGo.transform.SetParent(transform);
            characterGo.AddComponent<Image>().rectTransform.sizeDelta = textRenderCharacters.sprite.rect.size * fontSize;
            characterGo.transform.localPosition = position;
            characterGo.GetComponent<Image>().sprite = textRenderCharacters.sprite;
            UISimpleShake shake = characterGo.AddComponent<UISimpleShake>();
            shake.range = shakeDistanceCurve.Evaluate(inputText.Length);
            shake.shakeInterval = shakeIntervalCurve.Evaluate(inputText.Length);
        }
    }
    
    
    
    
    private TextRenderCharacters FindLetter(char letter)
    {
        TextRenderCharacters textRenderCharacters = null;
        foreach (TextRenderCharacters character in letters)
        {
            if (character.letter == letter){
                textRenderCharacters = character;
                break;
            }
        }

        return textRenderCharacters;
    }
}

[Serializable]
public class AnimationRange
{
    public List<Sprite> frames;
    public float frameDuration;
    [MinMaxSlider(0f,10f)]
    public Vector2 range;

    public float scale;

    public bool IsInRange(int nm)
    {
        if(range.x <= nm && nm<= range.y) return true;
        return false;
    }
}