using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSfxPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClipName> _clips;
    public static RandomSfxPlayer Instance { get; private set; }
    [Serializable]
    public class AudioClipName
    {
        public string name;
        public List<AudioClip> clips;

        public bool CheckName(string name)
        {
            return name == name;
        }
    }
    
    public void Play(string clipName)
    {
        foreach (AudioClipName clip in _clips)
        {
            if (clip.CheckName(clipName))
            {
                AudioSource.PlayClipAtPoint(clip.clips[Random.Range(0, _clips.Count)], transform.position);
                return;
            }
        }
    }

    void Singleton()
    {
        if (Instance !=null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
