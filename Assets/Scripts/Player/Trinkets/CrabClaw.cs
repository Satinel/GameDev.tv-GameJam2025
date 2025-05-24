using UnityEngine;
using System;

public class CrabClaw : Trinket
{
    PlayerHealth _playerHealth;

    void Awake()
    {
        _playerHealth = GetComponentInParent<PlayerHealth>();
    }

    protected override void Start()
    {
        base.Start();
        _playerHealth.GainRevive(this);
    }
}
