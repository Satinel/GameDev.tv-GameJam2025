using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnFightStarted;
    public static event Action OnEnemyTurnEnd;
    public static event Action<Enemy> OnEnemyKilled;

    [field:SerializeField] public string Name { get; set; }
    [SerializeField] int _maxHealth = 25;
    [SerializeField] int _attacksPerTurn = 1;

    int _health;
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
    }

    void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
    }

    void PlayerHealth_OnPlayerDeath()
    {
        _playerDead = true;
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
        AnimateAttack();
    }

    public void AttackCompleted()
    {
        if(_playerDead) { return; }

        if(_attacksPerformed >= _attacksPerTurn)
        {
            EndEnemyTurn();
        }
        else
        {
            AttackStarted();
        }
    }

    void EndEnemyTurn()
    {
        _attacksPerformed = 0;
        OnEnemyTurnEnd?.Invoke();
    }

    public bool TakeDamage(int amount)
    {
        if(!_inBattle || _isDead) { return _isDead; }

        _health -= amount;

        if(_health <= 0)
        {
            _isDead = true;
            AnimateDeath();
        }
        else
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
}
