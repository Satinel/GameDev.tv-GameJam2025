using UnityEngine;
using System;

public class PrehensileTongue : Trinket // Please Note that farming these breaks the game as at 95 all player attacks crit and cannot miss
{
    public static event Action<int> OnActivated;

    [SerializeField] int _critChanceIncrease = 1;

    protected override void Start()
    {
        base.Start();
        OnActivated?.Invoke(_critChanceIncrease);
    }

    public  override void LevelUp()
    {
        base.LevelUp();
        OnActivated?.Invoke(_critChanceIncrease);
    }
}
