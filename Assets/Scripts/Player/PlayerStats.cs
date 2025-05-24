using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public static event Action OnExperienceGained;
    public static event Action<Stats, int> OnStatIncreased;
    public static event Action<string> OnTempStatChange;
    public static event Action OnTempStatsReset;
    public static event Action<int> OnMoneyChanged;

    [field:SerializeField] public int Strength { get; set; } // Primarily about dealing damage
    [field:SerializeField] public int Accuracy { get; set; } // Primarily about landing attacks
    [field:SerializeField] public int Fortitude { get; set; } // Primarily about reducing damage
    [field:SerializeField] public int Evasion { get; set; } // Primarily about avoiding attacks
    [field:SerializeField] public int Tenacity { get; set; } // Primarily governs Hitpoints
    [field:SerializeField] public int Initiative { get; set; } // Primarily governs turn order
    [field:SerializeField] public int Money { get; set; } // Primarily governs Tigey

    [SerializeField] int _baseLevelXP = 25;

    int _tempBonusStrength;
    int _tempBonusAccuracy;
    int _tempBonusFortitude;
    int _tempBonusEvasion;

    int _level = 1;
    int _experience;
    int _xpToLevel;
    float _xpBonusMultiplyer;
    public int Level => _level;
    public int CurrentXP => _experience;
    public int NextLevelXP => _xpToLevel;

    public int CurrentStrength => Strength + _tempBonusStrength;
    public int CurrentAccuracy => Accuracy + _tempBonusAccuracy;
    public int CurrentFortitude => Fortitude + _tempBonusFortitude;
    public int CurrentEvasion => Evasion + _tempBonusEvasion;

    public enum Stats
    {
        Strength,
        Accuracy,
        Fortitude,
        Evasion,
        Tenacity,
        Initiative
    }

    void Start()
    {
        _xpToLevel = _baseLevelXP;
    }

    void OnEnable()
    {
        PlayerCombat.OnCombatResolved += PlayerCombat_OnCombatResolved;
        Enemy.OnEnemyKilled += Enemy_OnEnemyKilled;
        KeenNose.OnActivated += KeenNose_OnActivated;
    }

    void OnDisable()
    {
        PlayerCombat.OnCombatResolved -= PlayerCombat_OnCombatResolved;
        Enemy.OnEnemyKilled -= Enemy_OnEnemyKilled;
        KeenNose.OnActivated += KeenNose_OnActivated;
    }

    void PlayerCombat_OnCombatResolved()
    {
        _tempBonusStrength = 0;
        _tempBonusAccuracy = 0;
        _tempBonusFortitude = 0;
        _tempBonusEvasion = 0;
        OnTempStatsReset?.Invoke();
    }

    void Enemy_OnEnemyKilled(Enemy enemy)
    {
        GainExperience(enemy.ExperienceValue);
        ChangeMoney(enemy.MoneyValue);
    }

    void KeenNose_OnActivated(float multiplyer)
    {
        _xpBonusMultiplyer += multiplyer;
    }

    public void GainExperience(int amount)
    {
        _experience += amount + Mathf.FloorToInt(amount * _xpBonusMultiplyer);

        CheckForLevelUp();

        OnExperienceGained?.Invoke();
    }

    void CheckForLevelUp()
    {
        if(_experience < _xpToLevel) { return; }
        
        _level++;
        _xpToLevel = _baseLevelXP * _level * _level;
        HandleLevelUp();
        CheckForLevelUp();
    }

    void HandleLevelUp()
    {
        // TODO Menu to select stat to increase or randomize
        // IncreaseStat(Stats.Strength, 1);
        // IncreaseStat(Stats.Accuracy, 1);
        // IncreaseStat(Stats.Fortitude, 1);
        // IncreaseStat(Stats.Evasion, 1);
        IncreaseStat(Stats.Tenacity, 1);
        IncreaseStat(Stats.Initiative, 1);
    }

    public void GainTempBonus(Stats stat, int amount)
    {
        switch(stat)
        {
            case Stats.Strength:
                _tempBonusStrength += amount;
                break;
            case Stats.Accuracy:
                _tempBonusAccuracy += amount;
                break;
            case Stats.Fortitude:
                _tempBonusFortitude += amount;
                break;
            case Stats.Evasion:
                _tempBonusEvasion += amount;
                break;
            default:
                break;
        }
        if(amount > 0)
        {
            OnTempStatChange?.Invoke($"\n{stat} +{amount.FormatLargeNumbers()}\n");
        }
        else
        {
            OnTempStatChange?.Invoke($"\n{stat} -{Mathf.Abs(amount).FormatLargeNumbers()}\n");
        }
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
        Money = Mathf.Max(0, Money + amount);

        OnMoneyChanged?.Invoke(Money);
    }
}
