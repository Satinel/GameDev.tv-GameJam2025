using UnityEngine;
using System;

public class QuickMolt : Trinket
{
    public static event Action<string, int> OnActivated;

    [SerializeField] int _healAmount;

    PlayerHealth _playerHealth;

    void Awake()
    {
        _playerHealth = GetComponentInParent<PlayerHealth>();
        PlayerCombat.OnPlayerTurnStart += PlayerCombat_OnPlayerTurnStart;
    }

    void OnDestroy()
    {
        PlayerCombat.OnPlayerTurnStart -= PlayerCombat_OnPlayerTurnStart;
    }

    void PlayerCombat_OnPlayerTurnStart(int turn)
    {
        if(turn == 3)
        {
            Activation();
        }
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name, _healAmount);
        _playerHealth.GainHealth(_healAmount * Level);
    }
}
