using UnityEngine;
using System;

public class PlayerAbilityPoison : PlayerAbility
{
    public static event Action<int> OnPoisonHit;
    public static event Action<int> OnDamageIncrease;

    [SerializeField] int _poisonDamage = 10;

    void Awake()
    {
        MushroomCap.OnActivated += MushroomCap_OnActivated;
    }

    void OnDestroy()
    {
        MushroomCap.OnActivated -= MushroomCap_OnActivated;
    }

    public override void Hit()
    {
        base.Hit();
        OnPoisonHit?.Invoke(_poisonDamage);
    }

    void MushroomCap_OnActivated(string name, int increase)
    {
        _poisonDamage += increase;
        OnDamageIncrease?.Invoke(_poisonDamage);
    }
}
