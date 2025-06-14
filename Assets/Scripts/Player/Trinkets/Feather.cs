using UnityEngine;
using System;

public class Feather : Trinket
{
    public static event Action<string, int> OnActivated;

    [SerializeField] int _healAmount = 50;
    PlayerHealth _playerHealth;

    void Awake()
    {
        _playerHealth = GetComponentInParent<PlayerHealth>();
        PlayerCombat.OnEnemyMiss += PlayerCombat_OnEnemyMiss;
        _toolTipText = $"Evading An Attack Heals You For {_healAmount}";
    }

    void OnDestroy()
    {
        PlayerCombat.OnEnemyMiss -= PlayerCombat_OnEnemyMiss;
    }

    public override void LevelUp()
    {
        base.LevelUp();
        _toolTipText = $"Evading An Attack Heals You For {_healAmount * (Level + 1)}";
    }

    void PlayerCombat_OnEnemyMiss()
    {
        Activation();
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name, _healAmount * (Level + 1));
        _playerHealth.GainHealth(_healAmount * (Level + 1));
    }
}
