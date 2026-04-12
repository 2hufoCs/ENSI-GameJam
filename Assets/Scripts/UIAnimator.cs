using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    public List<Sprite> frames;
    public float frameDuration;
    
    private float timer;
    private int frame;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameDuration)
        {
            timer = 0;
            
            image.sprite = frames[frame];
            
            frame++;
            if(frame >= frames.Count) frame = 0;
        }
    }
}
