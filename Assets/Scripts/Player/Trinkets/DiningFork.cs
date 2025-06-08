using UnityEngine;
using System;

public class DiningFork : Trinket
{
    // public static event Action<string, int> OnActivated;

    // [SerializeField] int _healAmount = 15;

    // PlayerHealth _playerHealth;

    // void Awake()
    // {
    //     _playerHealth = GetComponentInParent<PlayerHealth>();
    //     Enemy.OnEnemyKilled += Enemy_OnEnemyKilled;
    //     _toolTipText = $"Gain {_healAmount} HP After Defeating An Enemy";
    // }

    // void OnDestroy()
    // {
    //     Enemy.OnEnemyKilled -= Enemy_OnEnemyKilled;
    // }

    // public override void LevelUp()
    // {
    //     base.LevelUp();
    //     _toolTipText = $"Gain {_healAmount + (_healAmount * Level)} HP After Defeating An Enemy";
    // }

    // void Enemy_OnEnemyKilled(Enemy _)
    // {
    //     Activation();
    // }

    // protected override void Activation()
    // {
    //     base.Activation();
    //     OnActivated?.Invoke(Name, _healAmount + (_healAmount * Level));
    //     _playerHealth.GainHealth(_healAmount + (_healAmount * Level));
    // }
}
