using UnityEngine;
using UnityEngine.UI;

public class UISpriteCopy : MonoBehaviour
{
    //Copys the current sprite to an other gameobject
    [SerializeField] private Image _originalSprite;
    [SerializeField] private Image _imageTarget;

    void Start()
    {
        _originalSprite = GetComponent<Image>();
    }

    void FixedUpdate()
    {
        _imageTarget.sprite = _originalSprite.sprite;
    }
}
