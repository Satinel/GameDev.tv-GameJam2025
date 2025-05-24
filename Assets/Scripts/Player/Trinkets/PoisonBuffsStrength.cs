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
        OnActivated?.Invoke(Name);
        _playerStats.GainTempBonus(PlayerStats.Stats.Strength, _buffAmount + Level);
    }
}
