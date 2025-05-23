using UnityEngine;
using System;

public class ParalyzingVenom : Trinket
{
    public static event Action<string, int> OnActivated;
    [SerializeField] int _debuffAmount;

    void Awake()
    {
        PlayerAbilityPoison.OnPoisonHit += PlayerAbilityPoison_OnPoisonHit;
    }

    void OnDestroy()
    {
        PlayerAbilityPoison.OnPoisonHit -= PlayerAbilityPoison_OnPoisonHit;
    }

    void PlayerAbilityPoison_OnPoisonHit(int _)
    {
        Activation();
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name, _debuffAmount);
    }
}
