using UnityEngine;
using System;

public class CrabClaw : Trinket
{
    PlayerHealth _playerHealth;
    public int Multiplyer { get; private set; } = 125;

    void Awake()
    {
        _playerHealth = GetComponentInParent<PlayerHealth>();
        _toolTipText = $"The First Time You Take Lethal Damage Heal To 1 HP";
    }

    public override void LevelUp()
    {
        base.LevelUp();
        _toolTipText = $"The First Time You Take Lethal Damage Heal Up To {1 + (Level * Multiplyer)} HP";
    }

    protected override void Start()
    {
        base.Start();
        _playerHealth.GainRevive(this);
    }
}
