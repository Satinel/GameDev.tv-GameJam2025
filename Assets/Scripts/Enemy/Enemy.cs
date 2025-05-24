using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnFightStarted;
    public static event Action OnEnemyTurnEnd;
    public static event Action<int> OnEnemyHealthChanged;
    public static event Action<Enemy> OnEnemyKilled;

    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public bool IsBoss { get; private set; } = false;
    [field:SerializeField] public int Strength { get; private set; } = 1;
    [field:SerializeField] public int Accuracy { get; set; } = 1;
    [field:SerializeField] public int Fortitude { get; set; } = 1;
    [field:SerializeField] public int Evasion { get; set; } = 1;
    [field:SerializeField] public int Tenacity { get; set; } = 1;
    [field:SerializeField] public int Initiative { get; private set; } = 5;
    [field:SerializeField] public int ExperienceValue { get; private set; } = 2;
    [field:SerializeField] public int MoneyValue { get; private set; } = 3;
    [field:SerializeField] public int LootChance { get; private set; } = 25;

    public bool IsPoisoned { get; private set; }
    public int PoisonDamage { get; private set; }

    [SerializeField] int _maxHealth = 25;
    public int MaxHealth => _maxHealth;
    [SerializeField] int _attacksPerTurn = 1;
    [SerializeField] List<Trinket> _lootTable;

    int _health;
    public int CurrentHealth => _health;
    int _attacksPerformed;

    bool _inBattle, _isDead, _isFirstTurn, _playerDead;
    List<EnemyAbility> _abilities = new();
    EnemyAbility _firstAbility, _selectedAbility;

    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _health = _maxHealth;
        foreach(EnemyAbility ability in GetComponentsInChildren<EnemyAbility>())
        {
            _abilities.Add(ability);
            if(ability.IsOpener)
            {
                _firstAbility = ability;
            }
        }
        if(_lootTable.Count == 0)
        {
            LootChance = 0;
        }

        ExperienceValue = _maxHealth * Strength;
        MoneyValue = Strength + Accuracy + Fortitude + Evasion + Tenacity + Initiative;
    }

    void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        PlayerAbilityPoison.OnPoisonHit += PlayerAbilityPoison_OnPoisonHit;

        // Trinkets
        SpikedCarapace.OnActivated += SpikedCarapace_OnActivated;
        ParalyzingVenom.OnActivated += ParalyzingVenom_OnActivated;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
        PlayerAbilityPoison.OnPoisonHit -= PlayerAbilityPoison_OnPoisonHit;

        // Trinkets
        SpikedCarapace.OnActivated -= SpikedCarapace_OnActivated;
        ParalyzingVenom.OnActivated -= ParalyzingVenom_OnActivated;
    }

    void PlayerHealth_OnPlayerDeath()
    {
        _playerDead = true;
    }

    void PlayerAbilityPoison_OnPoisonHit(int poisonDamage)
    {
        if(!_inBattle) { return; }

        IsPoisoned = true;
        PoisonDamage = poisonDamage;
        AnimateHurt();
    }

    public void StartBattle()
    {
        _inBattle = true;
        _isFirstTurn = true;
        _attacksPerformed = 0;
        OnFightStarted?.Invoke(this);
    }

    public void EndBattle()
    {
        _inBattle = false;
    }

    public void AttackStarted()
    {
        _selectedAbility = _abilities[UnityEngine.Random.Range(0, _abilities.Count)];

        if(_isFirstTurn)
        {
            _isFirstTurn = false;
            if(_firstAbility)
            {
                _selectedAbility = _firstAbility;
            }
        }

        _selectedAbility.StartAbility();
        _attacksPerformed++;
        if(_selectedAbility.IsSingleUse)
        {
            _abilities.Remove(_selectedAbility);
        }
        AnimateAttack();
    }

    public void AttackCompleted()
    {
        if(_playerDead) { return; }
        if(_isDead) { return; }

        if(_attacksPerformed >= _attacksPerTurn)
        {
            EndEnemyTurn();
        }
        else
        {
            Invoke(nameof(AttackStarted), 1.5f);
        }
    }

    void EndEnemyTurn()
    {
        _attacksPerformed = 0;
        OnEnemyTurnEnd?.Invoke();
    }

    public bool TakeDamage(int amount, bool playHurt)
    {
        if(!_inBattle || _isDead) { return _isDead; }

        _health = Mathf.Max(0, _health - amount);

        _health = Mathf.Min(_maxHealth, _health);

        if(_health > _maxHealth)
        {
            _health = _maxHealth;
        }

        OnEnemyHealthChanged?.Invoke(amount);

        if(_health == 0)
        {
            _isDead = true;
            AnimateDeath();
        }
        else if(playHurt)
        {
            AnimateHurt();
        }

        return _isDead;
    }

    public void HandleDeath() // Animation Trigger
    {
        OnEnemyKilled?.Invoke(this);
    }

    public void HandleAttack() // Animation Trigger
    {
        _selectedAbility.UseAbility();
    }

    void AnimateDeath()
    {
        _animator.SetTrigger("Death");
    }

    void AnimateHurt()
    {
        _animator.SetTrigger("Hurt");
    }

    void AnimateAttack()
    {
        _animator.SetTrigger("Attack");
    }

    public Trinket RollForLoot() // TODO? make loot weighted if possible
    {
        int lootRoll = UnityEngine.Random.Range(0, _lootTable.Count - 1);
        return _lootTable[lootRoll];
    }

    // Trinkets

    void SpikedCarapace_OnActivated(string _, int damage)
    {
        TakeDamage(damage, false);
    }

    void ParalyzingVenom_OnActivated(string _, int debuff)
    {
        Evasion -= debuff;
    }
}
