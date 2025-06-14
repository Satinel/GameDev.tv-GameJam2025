using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public static event Action OnExperienceGained;
    public static event Action<int> OnLevelUp;
    public static event Action NoLevelUp;
    public static event Action<Stats, int> OnStatIncreased;
    public static event Action<string> OnTempStatChange;
    public static event Action OnTempStatsReset;
    public static event Action<int> OnMoneyChanged;
    public static event Action<int> OnBonusXPEarned;
    public static event Action OnSAFEIncreased;

    [field:SerializeField] public int Strength { get; private set; } // Primarily about dealing damage
    [field:SerializeField] public int Accuracy { get; private set; } // Primarily about landing attacks
    [field:SerializeField] public int Fortitude { get; private set; } // Primarily about reducing damage
    [field:SerializeField] public int Evasion { get; private set; } // Primarily about avoiding attacks
    [field:SerializeField] public int Tenacity { get; private set; } // Primarily governs Hitpoints
    [field:SerializeField] public int Initiative { get; private set; } // Primarily governs turn order
    [field:SerializeField] public int Money { get; private set; } // Primarily governs Tigey
    [field:SerializeField] public int CriticalHitBonus { get; private set; } // Increases chance of critical hits

    [SerializeField] int _baseLevelXP = 25;

    int _tempBonusStrength;
    int _tempBonusAccuracy;
    int _tempBonusFortitude;
    int _tempBonusEvasion;

    int _level = 1;
    int _experience;
    int _xpToLevel;
    float _xpBonusMultiplyer;

    public CompoundEye RerollTrinket;
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
        LevelUpWindow.OnLevelStatPicked += LevelUpWindow_OnLevelStatPicked;
        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        Enemy.OnEnemyKilled += Enemy_OnEnemyKilled;
        KeenNose.OnActivated += KeenNose_OnActivated;
        PrehensileTongue.OnActivated += PrehensileTongue_OnActivated;
        CompoundEye.OnActivated += CompoundEye_OnActivated;
        StopWatch.OnActivated += StopWatch_OnActivated;
    }

    void OnDisable()
    {
        LevelUpWindow.OnLevelStatPicked -= LevelUpWindow_OnLevelStatPicked;
        PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
        Enemy.OnEnemyKilled -= Enemy_OnEnemyKilled;
        KeenNose.OnActivated -= KeenNose_OnActivated;
        PrehensileTongue.OnActivated -= PrehensileTongue_OnActivated;
        CompoundEye.OnActivated -= CompoundEye_OnActivated;
        StopWatch.OnActivated -= StopWatch_OnActivated;
    }

    void LevelUpWindow_OnLevelStatPicked(Stats stat, int amount)
    {
        IncreaseStat(stat, amount);
        CheckForLevelUp();
    }

    void PlayerHealth_OnPlayerDeath()
    {
        _tempBonusStrength = 0;
        _tempBonusAccuracy = 0;
        _tempBonusFortitude = 0;
        _tempBonusEvasion = 0;
        OnTempStatsReset?.Invoke();
    }

    void Enemy_OnEnemyKilled(Enemy enemy)
    {
        _tempBonusStrength = 0;
        _tempBonusAccuracy = 0;
        _tempBonusFortitude = 0;
        _tempBonusEvasion = 0;
        OnTempStatsReset?.Invoke();
        GainExperience(enemy.ExperienceValue);
        ChangeMoney(enemy.MoneyValue);
    }

    void KeenNose_OnActivated(float multiplyer)
    {
        _xpBonusMultiplyer = multiplyer;
    }

    void PrehensileTongue_OnActivated(int amount)
    {
        CriticalHitBonus += amount;
    }

    void CompoundEye_OnActivated(CompoundEye eye)
    {
        RerollTrinket = eye;
    }

    void StopWatch_OnActivated(string _, int amount)
    {
        IncreaseSAFE(amount);
    }

    void IncreaseSAFE(int amount)
    {
        _tempBonusStrength += amount;
        _tempBonusAccuracy += amount;
        _tempBonusFortitude += amount;
        _tempBonusEvasion += amount;
        OnSAFEIncreased?.Invoke();
    }

    public void GainExperience(int amount)
    {
        _experience += amount;

        if(_xpBonusMultiplyer > 0)
        {
            int bonus = Mathf.FloorToInt(amount * _xpBonusMultiplyer);
            _experience += bonus;
            OnBonusXPEarned?.Invoke(bonus);
        }

        CheckForLevelUp();

        OnExperienceGained?.Invoke();
    }

    void CheckForLevelUp()
    {
        if(_experience < _xpToLevel)
        {
            NoLevelUp?.Invoke();
            return;
        }

        _level++;
        _xpToLevel = _baseLevelXP * _level * _level;
        OnLevelUp?.Invoke(_level);
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

    public void LoadData(int level, int experience, int xpToLevel, int strength, int accuracy, int fortitude, int evastion, int tenacity, int initiative, int money)
    {
        _level = level;
        _experience = experience;
        _xpToLevel = xpToLevel;
        Strength = strength;
        Accuracy = accuracy;
        Fortitude = fortitude;
        Evasion = evastion;
        Tenacity = tenacity;
        Initiative = initiative;
        Money = money;

        OnExperienceGained?.Invoke(); // This doesn't need to be OnExperienceGained but it's a convenient one to update all stat UI and affect nothing else
    }

    public void LoadStats(int strength, int accuracy, int fortitude, int evastion, int tenacity, int initiative, int money)
    {
        Strength = strength;
        Accuracy = accuracy;
        Fortitude = fortitude;
        Evasion = evastion;
        Tenacity = tenacity;
        Initiative = initiative;
        Money = money;

        OnExperienceGained?.Invoke(); // This doesn't need to be OnExperienceGained but it's a convenient one to update all stat UI and affect nothing else
    }
}
