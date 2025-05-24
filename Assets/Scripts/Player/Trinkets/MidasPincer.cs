using UnityEngine;
using System;

public class MidasPincer : Trinket
{
    public static event Action<string, int> OnActivated;

    int _moneyGained;
    bool _isFirstAttack = true;
    PlayerStats _playerStats;

    void Awake()
    {
        _playerStats = GetComponentInParent<PlayerStats>();
        PlayerCombat.OnPlayerDealtDamage += PlayerCombat_OnPlayerDealtDamage;
        Enemy.OnFightStarted += Enemy_OnFightStarted;
    }

    void OnDestroy()
    {
        PlayerCombat.OnPlayerDealtDamage -= PlayerCombat_OnPlayerDealtDamage;
        Enemy.OnFightStarted += Enemy_OnFightStarted;
    }

    void PlayerCombat_OnPlayerDealtDamage(int amount)
    {
        if(!_isFirstAttack) { return; }

        _moneyGained = amount + Level;
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
