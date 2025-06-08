using UnityEngine;
using System;

public class KeenNose : Trinket
{
    public static event Action<float> OnActivated;

    [SerializeField] float _bonusAmount = 0.1f;

    void Awake()
    {
        _toolTipText = $"Gain {Mathf.RoundToInt(_bonusAmount * 100)}% More Experience";
    }

    protected override void Start()
    {
        base.Start();
        OnActivated?.Invoke(_bonusAmount);
    }

    public override void LevelUp()
    {
        base.LevelUp();
        _bonusAmount += 0.1f;
        _toolTipText = $"Gain {Mathf.RoundToInt(_bonusAmount * 100)}% More Experience";
        OnActivated?.Invoke(_bonusAmount);
    }
}
