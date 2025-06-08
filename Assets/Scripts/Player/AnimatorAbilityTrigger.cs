using System;
using UnityEngine;

public class AnimatorAbilityTrigger : MonoBehaviour
{
    public static event Action OnAnimatorHit;
    public static event Action OnAnimatorMiss;

    public void HandleHit()
    {
        OnAnimatorHit?.Invoke();
    }

    public void HandleMiss()
    {
        OnAnimatorMiss?.Invoke();
    }
}
