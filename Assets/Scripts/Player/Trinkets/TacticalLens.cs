using UnityEngine;
using System;

public class TacticalLens : Trinket
{
    public static event Action<string> OnActivated;
    [SerializeField] int _buffAmount;
    PlayerStats _playerStats;

    void Awake()
    {
        _playerStats = GetComponentInParent<PlayerStats>();
        PlayerCombat.OnEnemyMiss += PlayerCombat_OnEnemyMiss;
        _toolTipText = $"Increase Accuracy By {_buffAmount} After Evading An Attack";
    }

    void OnDestroy()
    {
        PlayerCombat.OnEnemyMiss -= PlayerCombat_OnEnemyMiss;
    }

    public override void LevelUp()
    {
        base.LevelUp();
        _toolTipText = $"Increase Accuracy By {_buffAmount + Level} After Evading An Attack";
    }

    void PlayerCombat_OnEnemyMiss()
    {
        Activation();
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name);
        _playerStats.GainTempBonus(PlayerStats.Stats.Accuracy, _buffAmount + Level);
    }
}
