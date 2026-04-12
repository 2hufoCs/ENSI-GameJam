using NaughtyAttributes;
using UnityEngine;

public class PlayAnimationOnDeath : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField,AnimatorParam("_anim")] private string _triggerName;

    
    
    void Awake()
    {
        Actions.OnPlayerDie += () =>  { _anim.SetTrigger(_triggerName); };
    }
    
    void OnDisable()
    {
        Actions.OnPlayerDie -= () =>  { _anim.SetTrigger(_triggerName); };
    }
}
