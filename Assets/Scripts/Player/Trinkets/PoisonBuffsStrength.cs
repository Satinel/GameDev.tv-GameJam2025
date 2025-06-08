using UnityEngine;
using System;

public class PoisonBuffsStrength : Trinket
{
    public static event Action<string> OnActivated;
    [SerializeField] int _buffAmount;
    PlayerStats _playerStats;

    void Awake()
    {
        _playerStats = GetComponentInParent<PlayerStats>();
        PlayerAbilityPoison.OnPoisonHit += PlayerAbilityPoison_OnPoisonHit;
        _toolTipText = $"Envenomating Increases Strength By {_buffAmount} Until End of Combat";
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
        _toolTipText = $"Envenomating Increases Strength By {_buffAmount + Level} Until End of Combat";
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name);
        _playerStats.GainTempBonus(PlayerStats.Stats.Strength, _buffAmount + Level);
    }
}
