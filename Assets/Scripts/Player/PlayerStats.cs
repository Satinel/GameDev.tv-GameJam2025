using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static event Action<Stats, int> OnStatIncreased;
    public static event Action<int> OnMoneyChanged;

    [field:SerializeField] public int Strength { get; set; } // Primarily about dealing damage
    [field:SerializeField] public int Accuracy { get; set; } // Primarily about landing attacks
    [field:SerializeField] public int Fortitude { get; set; } // Primarily about reducing damage
    [field:SerializeField] public int Evasion { get; set; } // Primarily about avoiding attacks
    [field:SerializeField] public int Tenacity { get; set; } // Primarily governs Hitpoints
    [field:SerializeField] public int Initiative { get; set; } // Primarily governs turn order
    [field:SerializeField] public int Money { get; set; } // Primarily governs Tigey


    public enum Stats
    {
        Strength,
        Accuracy,
        Fortitude,
        Evasion,
        Tenacity,
        Initiative
    }

    public void IncreaseStat(Stats stat, int amount)
    {
        switch(stat)
        {
            case Stats.Strength:
                Strength += amount;
                break;
            case Stats.Accuracy:
                Accuracy += amount;
                break;
            case Stats.Fortitude:
                Fortitude += amount;
                break;
            case Stats.Evasion:
                Evasion += amount;
                break;
            case Stats.Tenacity:
                Tenacity += amount;
                break;
            case Stats.Initiative:
                Initiative += amount;
                break;
            default:
                break;
        }
        OnStatIncreased?.Invoke(stat, amount);
    }

    public void ChangeMoney(int amount)
    {
        Money += amount;

        if(Money < 0)
        {
            Money = 0;
        }

        OnMoneyChanged?.Invoke(Money);
    }

}
