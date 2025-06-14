using UnityEngine;
using System;

public class PlayerAbilityPoison : PlayerAbility
{
    public static event Action<int> OnPoisonHit;
    public static event Action<int> OnDamageIncrease;

    [SerializeField] int _poisonDamage = 10, _hitChanceBuff = 15;
    float _defaultHitChance = 65f;
    int _currentPoisonDamage, _poisonIncrease;
    Enemy _currentEnemy;

    void Awake()
    {
        _defaultHitChance = HitChance;
        PlayerAbilitySwapper.OnGrabStateChanged += PlayerAbilitySwapper_OnGrabStateChanged;
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        MushroomCap.OnActivated += MushroomCap_OnActivated;
        PlayerCombat.OnCombatResolved += PlayerCombat_OnCombatResolved;
    }

    void OnDestroy()
    {
        PlayerAbilitySwapper.OnGrabStateChanged -= PlayerAbilitySwapper_OnGrabStateChanged;
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        MushroomCap.OnActivated -= MushroomCap_OnActivated;
        PlayerCombat.OnCombatResolved -= PlayerCombat_OnCombatResolved;
    }

    void Enemy_OnFightStarted(Enemy enemy)
    {
        _currentEnemy = enemy;
        _poisonIncrease = Mathf.FloorToInt(_poisonDamage / 2f);
        Description = $"Cause {_poisonDamage} Ongoing Defense Piercing Damage";
    }

    void PlayerAbilitySwapper_OnGrabStateChanged(int buffValue)
    {
        if(buffValue > 0)
        {
            HitChance += _hitChanceBuff;
        }
        else
        {
            _hitChanceBuff -= _hitChanceBuff;
        }
    }

    void PlayerCombat_OnCombatResolved()
    {
        HitChance = _defaultHitChance;
    }

    public override void Hit()
    {
        base.Hit();
        if(!_currentEnemy.IsPoisoned)
        {
            _currentPoisonDamage = _poisonDamage;
        }
        else
        {
            _currentPoisonDamage += _poisonIncrease;
        }
        OnPoisonHit?.Invoke(_poisonDamage);
        Description = $"Increase Ongoing Damage By {_poisonIncrease} For {_currentPoisonDamage + _poisonIncrease} Total";
    }

    void MushroomCap_OnActivated(string name, int increase)
    {
        _poisonDamage += increase;
        OnDamageIncrease?.Invoke(_poisonDamage);
    }
}
