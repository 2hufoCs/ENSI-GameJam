using UnityEngine;

[CreateAssetMenu(fileName = "Scene Transitions Data", menuName = "SceneTransitionsData")]
public class SceneTransitionsData : ScriptableObject
{
    public int sceneTransitionIndex = -1;
    public int uIAnimatorIndex = -1;
    public int colorIndex = -1;
}