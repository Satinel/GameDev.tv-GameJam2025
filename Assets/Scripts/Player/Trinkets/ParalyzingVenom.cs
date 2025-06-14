using UnityEngine;
using System;

public class ParalyzingVenom : Trinket
{
    public static event Action<string, int> OnActivated;

    [SerializeField] int _debuffAmount;

    void Awake()
    {
        PlayerAbilityPoison.OnPoisonHit += PlayerAbilityPoison_OnPoisonHit;
        _toolTipText = $"Envenomating Decreases Enemy Evasion By {_debuffAmount}";
    }

    void OnDestroy()
    {
        PlayerAbilityPoison.OnPoisonHit -= PlayerAbilityPoison_OnPoisonHit;
    }

    void PlayerAbilityPoison_OnPoisonHit(int _)
    {
        Activation();
    }

    public override void LevelUp()
    {
        base.LevelUp();
        _toolTipText = $"Envenomating Decreases Enemy Evasion By {_debuffAmount + Level}";
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name, _debuffAmount + Level);
    }
}
