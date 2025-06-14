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

    public static int DungeonFloor { get; private set; } = 1;
    [SerializeField] int _baseLevelIncrease = 25, _tenacityMultiplyer = 75;
    [SerializeField] int _maxHealth = 125;

    int _currentHealth = 125;
    bool _canRevive, _hasRevived;
    Vector3 _spawnPosition = Vector3.zero;
    CrabClaw _reviveTrinket;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;

    PlayerStats _playerStats;

    void Awake()
    {
        _playerStats = GetComponent<PlayerStats>();
        DungeonFloor = 1;
    }

    void OnEnable()
    {
        PlayerStats.OnLevelUp += PlayerStats_OnLevelUp;
        PlayerStats.OnStatIncreased += PlayerStats_OnTenacityIncreased;
        Enemy.OnFightStarted += EnemyStats_OnFightStarted;
        RestAreaUI.OnRestAreaUsed += RestAreaUI_OnRestAreaUsed;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        GameOverSplash.OnRespawn += GameOverSplash_OnRespawn;
    }

    void OnDisable()
    {
        PlayerStats.OnLevelUp -= PlayerStats_OnLevelUp;
        PlayerStats.OnStatIncreased -= PlayerStats_OnTenacityIncreased;
        Enemy.OnFightStarted -= EnemyStats_OnFightStarted;
        RestAreaUI.OnRestAreaUsed -= RestAreaUI_OnRestAreaUsed;
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        GameOverSplash.OnRespawn -= GameOverSplash_OnRespawn;
    }

    void Start()
    {
        _currentHealth = _maxHealth;
    }

    void PlayerStats_OnLevelUp(int level)
    {
        _maxHealth += _baseLevelIncrease;
        _currentHealth += _baseLevelIncrease;

        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    void PlayerStats_OnTenacityIncreased(PlayerStats.Stats stat, int amount)
    {
        if(stat != PlayerStats.Stats.Tenacity) { return; }

        _maxHealth += amount * _tenacityMultiplyer;
        _currentHealth = Mathf.Min(_maxHealth, amount * _tenacityMultiplyer);

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

    void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if(scene.buildIndex == 0) { return; }

        _currentHealth = _maxHealth;

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
        _currentHealth = Mathf.Min(_maxHealth, 1 + (_reviveTrinket.Level * _reviveTrinket.Multiplyer));
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        OnPlayerRevive?.Invoke(_reviveTrinket, _currentHealth);
    }

    public void GainHealth(int amount)
    {
        _currentHealth = Mathf.Min(MaxHealth, _currentHealth + amount);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public void GainRevive(CrabClaw trinket)
    {
        _reviveTrinket = trinket;
        _canRevive = true;
    }

    public void SetSpawnPoint()
    {
        _spawnPosition = transform.position;
    }

    public void SetSpawnPoint(Vector3 spawnPosition)
    {
        _spawnPosition = spawnPosition;
    }

    public Vector3 GetSpawnPosition()
    {
        return _spawnPosition;
    }

    void GameOverSplash_OnRespawn()
    {
        transform.position = _spawnPosition;
        _currentHealth = _maxHealth;
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public void IncreaseDungeonFloor()
    {
        DungeonFloor++;
    }

    public void LoadData(int maxHealth, int dungeonFloor)
    {
        _maxHealth = maxHealth;
        DungeonFloor = dungeonFloor;
        _currentHealth = _maxHealth;

        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }
}
