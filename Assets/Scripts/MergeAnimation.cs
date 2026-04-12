using System;
using System.Collections.Generic;
using UnityEngine;

public class MergeAnimation : MonoBehaviour
{
    List<GlitchAnimations> clips = new List<GlitchAnimations>();
    



    [Serializable]
    public class GlitchAnimations
    {
        public string name;
        public List<Sprite> sprites;
    }
}
