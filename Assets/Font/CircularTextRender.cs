using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class CircularTextRender : MonoBehaviour
{
    [SerializeField] private List<TextRenderCharacters> letters;
    public string inputText;
    [SerializeField] private float fontSize;
    [SerializeField,MinMaxSlider(0f,50f)] private Vector2 distanceRange;
    [SerializeField,MinMaxSlider(-20f,20f)] private Vector2 angleNoise;
    [SerializeField] private TextRender textRender;
    
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
        
        textRender.inputText = inputText;
        textRender.UpdateText();
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