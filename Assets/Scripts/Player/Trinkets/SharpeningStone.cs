using UnityEngine;
using System;

public class SharpeningStone : Trinket
{
    public static event Action<string> OnActivated;

    PlayerStats _playerStats;

    void Awake()
    {
        _playerStats = GetComponentInParent<PlayerStats>();
        PlayerCombat.OnPlayerTurnStart += PlayerCombat_OnPlayerTurnStart;
    }

    void OnDestroy()
    {
        PlayerCombat.OnPlayerTurnStart -= PlayerCombat_OnPlayerTurnStart;
    }

    void PlayerCombat_OnPlayerTurnStart(int turn)
    {
        Activation();
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name);
        _playerStats.GainTempBonus(PlayerStats.Stats.Strength, Level + 1);
    }
}
