using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static event Action<int, int> OnHealthChanged;
    public static event Action OnPlayerDeath;

    [SerializeField] int _tenacityMultiplyer = 125;

    int _maxHealth;
    int _currentHealth;
    PlayerStats _playerStats;

    void Awake()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    void OnEnable()
    {
        PlayerStats.OnStatIncreased += PlayerStats_OnTenacityIncreased;
        PlayerHealthSlider.OnSliderEnabled += PlayerHealthSlider_OnSliderEnabled;
    }

    void OnDisable()
    {
        PlayerStats.OnStatIncreased -= PlayerStats_OnTenacityIncreased;
        PlayerHealthSlider.OnSliderEnabled -= PlayerHealthSlider_OnSliderEnabled;
    }

    void Start()
    {
        _maxHealth = _playerStats.Tenacity * _tenacityMultiplyer;
        _currentHealth = _maxHealth;
    }

    void PlayerStats_OnTenacityIncreased(PlayerStats.Stats stat, int amount)
    {
        if(stat != PlayerStats.Stats.Tenacity) { return; }

        _maxHealth = _playerStats.Tenacity * _tenacityMultiplyer;
        _currentHealth += amount * _tenacityMultiplyer;
        if(_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
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
