using System;
using UnityEngine;

public class StopWatch : Trinket // Please Note: 'Stopwatch' is some System.Diagnostics class
{
    public static event Action<string, int> OnActivated;

    [SerializeField] int _buffAmount = 5;

    void Awake()
    {
        PlayerCombat.OnPlayerWinsInitiative += PlayerCombat_OnPlayerWinsInitiative;
        _toolTipText = $"Winning Initiative Increases Strength, Accuracy, Fortitude and Evasion By {_buffAmount}";
    }

    void OnDestroy()
    {
        PlayerCombat.OnPlayerWinsInitiative -= PlayerCombat_OnPlayerWinsInitiative;
    }

    void PlayerCombat_OnPlayerWinsInitiative()
    {
        Activation();
    }

    public override void LevelUp()
    {
        base.LevelUp();
        _toolTipText = $"Winning Initiative Increases Strength, Accuracy, Fortitude and Evasion By {_buffAmount + Level}";
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name, _buffAmount + Level);
    }
}
