using UnityEngine;
using System;

public class CompoundEye : Trinket
{
    public static event Action<Trinket> OnActivated;

    void Awake()
    {
        _toolTipText = $"Reroll Your First Missed Attack";
    }

    protected override void Start()
    {
        base.Start();
        Activation();
    }

    public override void LevelUp()
    {
        base.LevelUp();
        _toolTipText = $"Reroll Your First {1 + Level} Missed Attacks";
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(this);
    }
}
