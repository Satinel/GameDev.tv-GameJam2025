using UnityEngine;
using System;

public class PlayerAbilityPoison : PlayerAbility
{
    public static event Action<int> OnPoisonHit;

    public override void Hit()
    {
        base.Hit();
        OnPoisonHit?.Invoke(Damage);
    }
}
