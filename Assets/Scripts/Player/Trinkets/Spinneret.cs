using UnityEngine;
using System;

public class Spinneret : Trinket
{
    public static event Action<string, int> OnActivated;

    void Awake()
    {
        PlayerCombat.OnPlayerTurnStart += PlayerCombat_OnPlayerTurnStart;
        _toolTipText = $"Reduce Enemy Evasion By {Level + 1} At The Start Of Your Turn";
    }

    void OnDestroy()
    {
        PlayerCombat.OnPlayerTurnStart -= PlayerCombat_OnPlayerTurnStart;
    }

    public override void LevelUp()
    {
        base.LevelUp();
        _toolTipText = $"Reduce Enemy Evasion By {Level + 1} At The Start Of Your Turn";
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