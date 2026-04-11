using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class TextRender : MonoBehaviour
{
    [SerializeField] private List<TextRenderCharacters> letters;
    [SerializeField] private string inputText;
    [SerializeField] private float fontSize;
    [SerializeField] private float spaceSize;
    private Stack<GameObject> childrens = new Stack<GameObject>();
    
    [Button]
    public void UpdateText()
    {
        if (Application.isPlaying) DeleteChild();
        
        
        foreach (char letter in inputText)
        {
            TextRenderCharacters textRenderCharacters = FindLetter(letter);
            if (textRenderCharacters != null)
            {
                InstantiateCharacters(textRenderCharacters);
            }
        }
    }
    
    private void DeleteChild()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    

    private void InstantiateCharacters(TextRenderCharacters textRenderCharacters)
    {
        GameObject characterGO = new GameObject(textRenderCharacters.letter.ToString());
        characterGO.transform.SetParent(transform);
        if(textRenderCharacters.sprite != null)
        {
            characterGO.AddComponent<Image>().rectTransform.sizeDelta = textRenderCharacters.sprite.rect.size * fontSize;
            characterGO.GetComponent<Image>().sprite = textRenderCharacters.sprite;
        }
        else
        {
            characterGO.AddComponent<Image>().rectTransform.sizeDelta = new Vector2(spaceSize, 0);
        }
        childrens.Push(characterGO);
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
public class TextRenderCharacters
{
    public char letter;
    public Sprite sprite;
}
