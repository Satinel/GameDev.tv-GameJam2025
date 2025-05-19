using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnFightStarted;
    public static event Action OnEnemyTurnEnd;
    public static event Action<Enemy> OnAnyEnemyKilled;

    [SerializeField] int _maxHealth = 25;
    [SerializeField] int _health;
    bool _inBattle, _isDead;

    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _health = _maxHealth;
    }

    public void StartBattle()
    {
        _inBattle = true;
        OnFightStarted?.Invoke(this);
    }

    public void EndBattle()
    {
        _inBattle = false;
    }

    public void Attack() // This would be a great time to use a delegate?
    {
        // TODO Select at random from a list of attacks
        // TODO Calculate success/failure of attack
        // TODO Handle results of attack
        Debug.Log("Pretend the enemy did something!");
        Invoke(nameof(EndEnemyTurn), 2f);
    }

    void EndEnemyTurn()
    {
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
        OnAnyEnemyKilled?.Invoke(this);
    }

    void AnimateDeath()
    {
        _animator.SetTrigger("Death");
    }

    void AnimateHurt()
    {
        _animator.SetTrigger("Hurt");
    }
}
