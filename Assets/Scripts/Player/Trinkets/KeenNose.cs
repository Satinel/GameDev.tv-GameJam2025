using UnityEngine;
using System;

public class KeenNose : Trinket
{
    public static event Action<float> OnActivated;

    [SerializeField] float _bonusAmount = 0.1f;

    protected override void Start()
    {
        base.Start();
        OnActivated?.Invoke(_bonusAmount);
    }

    public override void LevelUp()
    {
        base.LevelUp();
        OnActivated?.Invoke(_bonusAmount);
    }
}
