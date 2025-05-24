using UnityEngine;
using System;

public class NutCracker :Trinket
{
    public static event Action<string, int> OnActivated;

    int _debuffAmount;

    void Awake()
    {
        PlayerCombat.OnPlayerDealtDamage += PlayerCombat_OnPlayerDealtDamage;
    }

    void OnDestroy()
    {
        PlayerCombat.OnPlayerDealtDamage -= PlayerCombat_OnPlayerDealtDamage;
    }

    void PlayerCombat_OnPlayerDealtDamage(int amount)
    {
        Activation();
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(Name, _debuffAmount);
    }
}
