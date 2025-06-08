using UnityEngine;
using System;

public class SpikedCarapace : Trinket
{
    public static event Action<string, int> OnActivated;

    PlayerStats _playerStats;

    void Awake()
    {
        _playerStats = GetComponentInParent<PlayerStats>();
        PlayerHealth.OnTakeDamage += Activation;
        _toolTipText = $"Deal Damage Equal to Current Fortitude When Hurt";
    }

    void OnDestroy()
    {
        PlayerHealth.OnTakeDamage -= Activation;
    }

    public override void LevelUp()
    {
        base.LevelUp();
        _toolTipText = $"Deal Damage Equal to Current Fortitude (+{Level}) When Hurt";
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name, _playerStats.CurrentFortitude + Level);
    }
}
