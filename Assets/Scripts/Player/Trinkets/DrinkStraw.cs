using UnityEngine;
using System;

public class DrinkStraw : Trinket
{
    public static event Action<string, int> OnActivated;

    [SerializeField] float _healPercentage = 0.1f;
    int _healAmount;

    PlayerHealth _playerHealth;

    void Awake()
    {
        _playerHealth = GetComponentInParent<PlayerHealth>();
        PlayerCombat.OnPlayerDealtDamage += PlayerCombat_OnPlayerDealtDamage;
    }

    void OnDestroy()
    {
        PlayerCombat.OnPlayerDealtDamage -= PlayerCombat_OnPlayerDealtDamage;
    }

    void PlayerCombat_OnPlayerDealtDamage(int amount)
    {
        _healAmount = Mathf.Max(1, Mathf.FloorToInt(amount * (_healPercentage + Level)));
        Activation();
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name, _healAmount);
        _playerHealth.GainHealth(_healAmount);
    }
}