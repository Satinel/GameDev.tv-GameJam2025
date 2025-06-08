using System;
using UnityEngine;

public class AnimatorAbilityTrigger : MonoBehaviour
{
    public static event Action OnAnimatorHit;
    public static event Action OnAnimatorMiss;
    [SerializeField] bool _isShadow;

    public void HandleHit()
    {
        if(_isShadow) { return; }

        OnAnimatorHit?.Invoke();
    }

    public void HandleMiss()
    {
        if(_isShadow) { return; }

        OnAnimatorMiss?.Invoke();
    }
}
