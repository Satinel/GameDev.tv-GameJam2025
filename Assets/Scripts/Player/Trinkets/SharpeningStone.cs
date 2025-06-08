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
        _toolTipText = $"Gain 1 Strength At The Start Of Your Turn";
    }

    void OnDestroy()
    {
        PlayerCombat.OnPlayerTurnStart -= PlayerCombat_OnPlayerTurnStart;
    }

    void PlayerCombat_OnPlayerTurnStart(int turn)
    {
        Activation();
    }

    public override void LevelUp()
    {
        base.LevelUp();
        _toolTipText = $"Gain {Level + 1} Strength At The Start Of Your Turn";
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name);
        _playerStats.GainTempBonus(PlayerStats.Stats.Strength, Level + 1);
    }
}
