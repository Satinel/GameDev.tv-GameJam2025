using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action OnFightStarted;
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

    public void SetInBattle(bool inBattle)
    {
        _inBattle = inBattle;
        OnFightStarted?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        if(!_inBattle || _isDead) { return; }

        _health -= amount;

        if(_health <= 0)
        {
            AnimateDeath();
        }
        else
        {
            AnimateHurt();
        }
    }

    public void HandleDeath() // Animation Trigger
    {
        OnAnyEnemyKilled?.Invoke(this);
    }

    void AnimateDeath()
    {
        _isDead = true;
        _animator.SetTrigger("Death");
    }

    void AnimateHurt()
    {
        _animator.SetTrigger("Hurt");
    }
}
