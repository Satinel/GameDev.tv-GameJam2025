using UnityEngine;
using System;

public class MushroomCap : Trinket
{
    public static event Action<string, int> OnActivated;

    [SerializeField] int _poisonDamageIncrease = 5;

    void Awake()
    {
        _toolTipText = $"Increase Venom Damage By {_poisonDamageIncrease}";
    }

    protected override void Start()
    {
        base.Start();
        OnActivated?.Invoke(Name, _poisonDamageIncrease);
    }

    public  override void LevelUp()
    {
        base.LevelUp();
        _toolTipText = $"Increase Venom Damage By {_poisonDamageIncrease * (Level + 1)}";
        OnActivated?.Invoke(Name, _poisonDamageIncrease);
    }
}
