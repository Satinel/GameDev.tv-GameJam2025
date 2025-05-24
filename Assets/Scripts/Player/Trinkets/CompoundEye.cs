using UnityEngine;
using System;

public class CompoundEye : Trinket
{
    public static event Action<Trinket> OnActivated;

    void Awake()
    {
        Enemy.OnFightStarted += Enemy_OnFightStarted;
    }

    void OnDestroy()
    {
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
    }

    void Enemy_OnFightStarted(Enemy _)
    {
        Activation();
    }

    protected override void Activation()
    {
        base.Activation();
        OnActivated?.Invoke(this);
    }
}
