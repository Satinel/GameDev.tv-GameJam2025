using UnityEngine;
using System;

public class AblativeShell : Trinket
{
    public static event Action<string> OnActivated;
    [SerializeField] int _buffAmount;
    PlayerStats _playerStats;
    bool _isActive;

    void Awake()
    {
        _playerStats = GetComponentInParent<PlayerStats>();
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        PlayerHealth.OnTakeDamage += PlayerHealth_OnTakeDamage;
    }

    void OnDestroy()
    {
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        PlayerHealth.OnTakeDamage -= PlayerHealth_OnTakeDamage;
    }

    void Enemy_OnFightStarted(Enemy _)
    {
        Activation();
    }

    void PlayerHealth_OnTakeDamage()
    {
        if(!_isActive) { return; }

        _isActive = false;
        _playerStats.GainTempBonus(PlayerStats.Stats.Fortitude, -(_buffAmount + Level));
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name);
        _playerStats.GainTempBonus(PlayerStats.Stats.Fortitude, _buffAmount + Level);
        _isActive = true;
    }
}