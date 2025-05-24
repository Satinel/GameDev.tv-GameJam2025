using UnityEngine;
using System;

public class DiningFork : Trinket
{
    public static event Action<string, int> OnActivated;

    [SerializeField] int _healAmount = 15;

    PlayerHealth _playerHealth;

    void Awake()
    {
        _playerHealth = GetComponentInParent<PlayerHealth>();
        Enemy.OnEnemyKilled += Enemy_OnEnemyKilled;
    }

    void OnDestroy()
    {
        Enemy.OnEnemyKilled -= Enemy_OnEnemyKilled;
    }

    void Enemy_OnEnemyKilled(Enemy _)
    {
        Activation();
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name, _healAmount + Level);
        _playerHealth.GainHealth(_healAmount + Level);
    }
}
