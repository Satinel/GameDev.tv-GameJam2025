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
        _toolTipText = $"Regain {_healAmount} HP On Your Third Combat Turn";
    }

    void OnDestroy()
    {
        PlayerCombat.OnPlayerTurnStart -= PlayerCombat_OnPlayerTurnStart;
    }

    public override void LevelUp()
    {
        base.LevelUp();
        _toolTipText = $"Regain {_healAmount * (Level + 1)} HP On Your Third Combat Turn";
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
        _playerHealth.GainHealth(_healAmount * (Level + 1));
    }
}
