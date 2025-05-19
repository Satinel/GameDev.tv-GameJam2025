using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDeath;

    [SerializeField] int _maxHealth;

    int _currentHealth;

    void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;

        if(_currentHealth <= 0)
        {
            _currentHealth = 0;
            HandleDeath();
        }

        // TODO Update Player Stats Health Text and Slider Values
    }

    void HandleDeath()
    {
        OnPlayerDeath?.Invoke();
        // TODO Some UI Somewhere shows final stats of the run
    }
}
