using UnityEngine;
using System;

public class Spinneret : Trinket
{
    public static event Action<string, int> OnActivated;

    void Awake()
    {
        PlayerCombat.OnPlayerTurnStart += PlayerCombat_OnPlayerTurnStart;
    }

    void OnDestroy()
    {
        PlayerCombat.OnPlayerTurnStart -= PlayerCombat_OnPlayerTurnStart;
    }

    void PlayerCombat_OnPlayerTurnStart(int turn)
    {
        Activation();
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name, Level + 1);
    }
}