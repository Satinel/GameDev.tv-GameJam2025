using UnityEngine;
using System;

public class MidasPincer : Trinket
{
    public static event Action<string, int> OnActivated;

    int _moneyGained;
    int _multiplyer = 10;
    bool _isFirstAttack = true;
    PlayerStats _playerStats;

    void Awake()
    {
        _playerStats = GetComponentInParent<PlayerStats>();
        PlayerCombat.OnPlayerDealtDamage += PlayerCombat_OnPlayerDealtDamage;
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        _toolTipText = $"Gain BugBucks Equal To Damage Of Your First Successful Attack";
    }

    void OnDestroy()
    {
        PlayerCombat.OnPlayerDealtDamage -= PlayerCombat_OnPlayerDealtDamage;
        Enemy.OnFightStarted += Enemy_OnFightStarted;
    }

    public override void LevelUp()
    {
        base.LevelUp();
        _toolTipText = $"Gain BugBucks Equal To Damage (+{Level * _multiplyer}) Of Your First Successful Attack";
    }

    void PlayerCombat_OnPlayerDealtDamage(int amount)
    {
        if(!_isFirstAttack) { return; }

        _moneyGained = amount + (Level * _multiplyer);
        Activation();
    }

    void Enemy_OnFightStarted(Enemy _)
    {
        _isFirstAttack = true;
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name, _moneyGained);
        _playerStats.ChangeMoney(_moneyGained);
        _isFirstAttack = false;
    }
}
