using System.Collections.Generic;
using UnityEngine;

public class RandomSfxPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _clips;

    public void Play()
    {
        AudioSource.PlayClipAtPoint(_clips[Random.Range(0, _clips.Count)], Vector2.zero);
    }
}
