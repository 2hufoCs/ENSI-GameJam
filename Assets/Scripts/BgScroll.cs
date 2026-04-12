using UnityEngine;
using UnityEngine.UI;

public class BgScroll : MonoBehaviour
{
    [SerializeField] float scrollSpeed;
    [SerializeField] Vector2 scrollDir;
    float timer = 0;

    Vector2 scroll;
    RectTransform rectTransform;
    RawImage img;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        img = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        scroll += Time.deltaTime * scrollSpeed * scrollDir;
        img.uvRect = new Rect(scroll, img.uvRect.size);
    }
}
