using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static event Action<int, int> OnHealthChanged;
    public static event Action<int, int> OnInitialHealthSettings;
    
    public static event Action OnTakeDamage;
    public static event Action OnPlayerDeath;
    public static event Action<Trinket, int> OnPlayerRevive;

    [SerializeField] int _tenacityMultiplyer = 125;

    int _maxHealth = 125;
    int _currentHealth = 125;
    bool _canRevive, _hasRevived;
    Trinket _reviveTrinket;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;

    PlayerStats _playerStats;

    void Awake()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    void OnEnable()
    {
        PlayerStats.OnStatIncreased += PlayerStats_OnTenacityIncreased;
        Enemy.OnFightStarted += EnemyStats_OnFightStarted;
        RestAreaUI.OnRestAreaUsed += RestAreaUI_OnRestAreaUsed;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    void OnDisable()
    {
        PlayerStats.OnStatIncreased -= PlayerStats_OnTenacityIncreased;
        Enemy.OnFightStarted -= EnemyStats_OnFightStarted;
        RestAreaUI.OnRestAreaUsed -= RestAreaUI_OnRestAreaUsed;
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
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

    void EnemyStats_OnFightStarted(Enemy _)
    {
        _hasRevived = false;
    }

    void RestAreaUI_OnRestAreaUsed()
    {
        GainHealth(_maxHealth);
    }

    void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Invoke(nameof(CallOnInitialHealthSettings), 0.5f); // This should update PlayerHealthSlider before a combat starts but after PlayerHealthSlider has had time to subscribe to OnHealthChanged
    }

    void CallOnInitialHealthSettings()
    {
        OnInitialHealthSettings?.Invoke(_currentHealth, MaxHealth);
    }

    public void TakeDamage(int amount)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - amount);

        OnTakeDamage?.Invoke();

        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        if(_currentHealth <= 0)
        {
            if(!_canRevive || _hasRevived)
            {
                HandleDeath();
            }
            else
            {
                Revive();
            }
        }
    }

    void HandleDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    void Revive()
    {
        _hasRevived = true;
        _currentHealth = 1 + (_reviveTrinket.Level * _tenacityMultiplyer);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        OnPlayerRevive?.Invoke(_reviveTrinket, _currentHealth);
    }

    public void GainHealth(int amount)
    {
        _currentHealth = Mathf.Min(MaxHealth, _currentHealth + amount);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public void GainRevive(Trinket trinket)
    {
        _reviveTrinket = trinket;
        _canRevive = true;
    }
}
