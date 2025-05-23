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
    }

    void Onestroy()
    {
        PlayerHealth.OnTakeDamage -= Activation;
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name, _playerStats.CurrentFortitude + Level);
    }
}
