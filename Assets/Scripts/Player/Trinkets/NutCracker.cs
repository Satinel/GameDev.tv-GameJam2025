using UnityEngine;
using System;

public class NutCracker :Trinket
{
    public static event Action<string, int> OnActivated;

    [SerializeField] int _debuffAmount;

    void Awake()
    {
        PlayerCombat.OnPlayerDealtDamage += PlayerCombat_OnPlayerDealtDamage;
        _toolTipText = $"Your Damage Dealing Attacks Reduce Enemy Fortitude By {_debuffAmount}";
    }

    void OnDestroy()
    {
        PlayerCombat.OnPlayerDealtDamage -= PlayerCombat_OnPlayerDealtDamage;
    }

    void PlayerCombat_OnPlayerDealtDamage(int amount)
    {
        Activation();
    }

    public override void LevelUp()
    {
        base.LevelUp();
        _toolTipText = $"Your Damage Dealing Attacks Reduce Enemy Fortitude By {_debuffAmount + Level}";
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name, _debuffAmount + Level);
    }
}
