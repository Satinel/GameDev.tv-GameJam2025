using UnityEngine;
using System;

public class CompoundEye : Trinket
{
    public static event Action<Trinket> OnActivated;

    protected override void Start()
    {
        base.Start();
        Activation();
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(this);
    }
}
