using UnityEngine;
using System;

public class MushroomCap : Trinket
{
    public static event Action<string, int> OnActivated;

    [SerializeField] int _poisonDamageIncrease = 5;

    protected override void Start()
    {
        base.Start();
        OnActivated?.Invoke(Name, _poisonDamageIncrease);
    }

    public  override void LevelUp()
    {
        base.LevelUp();
        OnActivated?.Invoke(Name, _poisonDamageIncrease);
    }
}
