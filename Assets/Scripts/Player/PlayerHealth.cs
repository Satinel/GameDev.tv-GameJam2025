using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action<int, int> OnHealthChanged;
    public static event Action OnPlayerDeath;

    [SerializeField] int _maxHealth;

    int _currentHealth;

    void OnEnable()
    {
        PlayerHealthSlider.OnSliderEnabled += PlayerHealthSlider_OnSliderEnabled;

    }

    void OnDisable()
    {
        PlayerHealthSlider.OnSliderEnabled += PlayerHealthSlider_OnSliderEnabled;
    }

    void Start()
    {
        _currentHealth = _maxHealth;
    }

    void PlayerHealthSlider_OnSliderEnabled()
    {
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;

        if(_currentHealth < 0) { _currentHealth = 0; }

        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        if(_currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    void HandleDeath()
    {
        OnPlayerDeath?.Invoke();
        // TODO Some UI Somewhere shows final stats of the run
    }
}
