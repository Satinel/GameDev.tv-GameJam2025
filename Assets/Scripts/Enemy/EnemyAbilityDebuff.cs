using System;
using UnityEngine;

public class EnemyAbilityDebuff : EnemyAbility
{
    [SerializeField] PlayerStats.Stats _stat = PlayerStats.Stats.Evasion;
    [SerializeField] int _debuffValue = 1;
    PlayerStats _playerStats;

    void Awake()
    {
        _playerStats = FindFirstObjectByType<PlayerStats>();
    }

    public override void Hit()
    {
        _playerStats.GainTempBonus(_stat, -_debuffValue);
    }
}
