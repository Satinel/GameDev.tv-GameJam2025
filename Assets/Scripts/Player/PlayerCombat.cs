using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using TMPro;

public class PlayerCombat : MonoBehaviour
{
    public static event Action OnCombatResolved;

    [SerializeField] float _defaultDelay = 2f;

    [SerializeField] GameObject _combatMenu, _combatButtonsParent, _results;
    [SerializeField] GameObject _battleStartSplash, _initiativeSplash, _playerTurnSplash, _enemyTurnSplash, _finalResults;
    [SerializeField] GameObject[] _attackButtons;
    [SerializeField] Button[] _buttons;
    [SerializeField] GameObject _closeResultsButton, _mainMenuButton;
    [SerializeField] TextMeshProUGUI _combatLog, _resultsText, _playerInitiative, _enemyInitiative;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _defaultHit, _defaultMiss, _defaultUse; // TODO replace these with prefabs containing visual effects along with sounds
    [SerializeField] DamageSplash _damageSplashPrefab;

    // [SerializeField] GameObject _leftAttack1, _leftAttack2;
    // [SerializeField] GameObject _rightAttack1, _rightAttack2;
    // [SerializeField] GameObject _tailAttack1, _tailAttack2;

    // TODO Set/Get equipped items in claws if any

    PlayerHealth _playerHealth;
    PlayerStats _playerStats;
    PlayerInventory _playerInventory;
    Enemy _currentEnemy;
    bool _isPlayerTurn, _optionsOpen;

    void Awake()
    {
        _playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    void Start()
    {
        _playerStats = _playerHealth.GetComponent<PlayerStats>();
        _playerInventory = _playerHealth.GetComponent<PlayerInventory>();
        _combatMenu.SetActive(false);
    }

    void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        Enemy.OnEnemyTurnEnd += Enemy_OnEnemyTurnEnd;
        Enemy.OnEnemyKilled += Enemy_OnAnyEnemyKilled;
        EnemyAbility.OnEnemyAbilityStarted += EnemyAbility_OnEnemyAbilityStarted;
        EnemyAbility.OnEnemyAbilityUsed += EnemyAbility_OnEnemyAbilityUsed;
        PlayerAbility.OnPlayerAbilityUsed += PlayerAbility_OnPlayerAbilityUsed;
        OptionsMenu.OnOptionsOpened += OptionsMenu_OnOptionsOpened;
        OptionsMenu.OnOptionsClosed += OptionsMenu_OnOptionsClosed;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        Enemy.OnEnemyTurnEnd -= Enemy_OnEnemyTurnEnd;
        Enemy.OnEnemyKilled -= Enemy_OnAnyEnemyKilled;
        EnemyAbility.OnEnemyAbilityStarted -= EnemyAbility_OnEnemyAbilityStarted;
        EnemyAbility.OnEnemyAbilityUsed -= EnemyAbility_OnEnemyAbilityUsed;
        PlayerAbility.OnPlayerAbilityUsed -= PlayerAbility_OnPlayerAbilityUsed;
        OptionsMenu.OnOptionsOpened += OptionsMenu_OnOptionsOpened;
        OptionsMenu.OnOptionsClosed -= OptionsMenu_OnOptionsClosed;
    }

    void PlayerHealth_OnPlayerDeath()
    {
        HideAttackButtons();
        foreach(Button button in _buttons)
        {
            button.interactable = false;
            button.gameObject.SetActive(false);
        }
        _combatButtonsParent.SetActive(false);
        _finalResults.SetActive(true); // TODO Tally up results for the run
        if(!_optionsOpen)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_mainMenuButton); // TODO Replace with EventSystem.current.SetSelectedGameObject(_closeFinalResultsButton);
        }
        _combatLog.text += $"\nYou Were Defeated!\n";
    }

    void Enemy_OnFightStarted(Enemy enemy)
    {
        _battleStartSplash.SetActive(true);
        _combatMenu.SetActive(true);
        _currentEnemy = enemy;
        StartCoroutine(RollInitiative());
    }

    IEnumerator RollInitiative()
    {
        yield return new WaitForSeconds(_defaultDelay / 1.5f);

        _playerInitiative.text = string.Empty;
        _enemyInitiative.text = string.Empty;
        _initiativeSplash.SetActive(true);

        yield return new WaitForSeconds(_defaultDelay / 1.5f);

        _battleStartSplash.SetActive(false);

        int playerRoll = UnityEngine.Random.Range(0, 20);
        //TODO SFX/VFX to indicate something is happening (rolling dice)
        _playerInitiative.text = (playerRoll + _playerStats.Initiative).ToString();
        _combatLog.text += $"\nYou Roll Initiative!\n{playerRoll + _playerStats.Initiative} ({playerRoll} + {_playerStats.Initiative})\n";

        yield return new WaitForSeconds(_defaultDelay / 3f);

        int enemyRoll = UnityEngine.Random.Range(0, 20);
        //TODO SFX/VFX to indicate something is happening (rolling dice)
        _enemyInitiative.text = (enemyRoll + _currentEnemy.Initiative).ToString();
        _combatLog.text += $"\n{_currentEnemy.Name} Rolls Initiative!\n{enemyRoll + _currentEnemy.Initiative} ({enemyRoll} + {_currentEnemy.Initiative})\n";

        yield return new WaitForSeconds(_defaultDelay / 2f);
        _initiativeSplash.SetActive(false);

        if(playerRoll + _playerStats.Initiative >= enemyRoll + _currentEnemy.Initiative)
        {
            StartPlayerTurn();
        }
        else
        {
            EndPlayerTurn();
        }
    }

    void Enemy_OnEnemyTurnEnd()
    {
        Invoke(nameof(StartPlayerTurn), _defaultDelay / 1.75f);
    }

    void Enemy_OnAnyEnemyKilled(Enemy enemy)
    {
        HideAttackButtons();
        _combatButtonsParent.SetActive(false);
        _combatLog.text += $"\n{_currentEnemy.Name} Was Defeated!\n";
        _results.SetActive(true);
        _combatLog.text += $"\nEarned {_currentEnemy.ExperienceValue} XP!\n\nFound {_currentEnemy.MoneyValue} Bug Bucks!\n";
        _resultsText.text = $"-RESULTS-\n\nEarned {_currentEnemy.ExperienceValue} XP!\n\nFound {_currentEnemy.MoneyValue} Bug Bucks!\n\n";
        // TODO Message about gaining item (also shown in _resultsText)
        if(!_optionsOpen)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_closeResultsButton);
        }
    }

    void EnemyAbility_OnEnemyAbilityStarted(EnemyAbility ability)
    {
        _combatLog.text += $"\n{_currentEnemy.Name} Used {ability.Name}!\n";
    }

    void EnemyAbility_OnEnemyAbilityUsed(EnemyAbility ability)
    {
        if(ability.AlwaysHits || UnityEngine.Random.Range(0, 100) + ability.HitChance + _currentEnemy.Accuracy - _playerStats.CurrentEvasion >= 100)
        {
            _combatLog.text += $"\nHit!\n";
            ability.Hit();
            if(ability.DealsDamage)
            {
                int damageDealt = Mathf.Max(0, ability.Damage + _currentEnemy.Strength - _playerStats.CurrentFortitude);
                _combatLog.text += $"\nYou Take\n{damageDealt} {ability.Adjective} Damage!\n";
                _playerHealth.TakeDamage(damageDealt);

                _audioSource.PlayOneShot(_defaultHit); // Visual FX HERE

                DamageSplash damageFX = Instantiate(_damageSplashPrefab, transform);
                damageFX.transform.position = new(damageFX.transform.position.x + UnityEngine.Random.Range(-300, 300), damageFX.transform.position.y + UnityEngine.Random.Range(-100, 200));
                damageFX.Setup(UnityEngine.Random.ColorHSV(), UnityEngine.Random.ColorHSV(), UnityEngine.Random.ColorHSV(), damageDealt.FormatLargeNumbers()); // TODO set colors through ability
            }
            else
            {
                _audioSource.PlayOneShot(_defaultUse); // Visual FX HERE
            }
        }
        else
        {
            _combatLog.text += $"\nMiss!\n";
            _audioSource.PlayOneShot(_defaultMiss); // Visual FX HERE
        }
        _currentEnemy.AttackCompleted();
    }

    void PlayerAbility_OnPlayerAbilityUsed(string message)
    {
        _combatLog.text += message;
    }

    void OptionsMenu_OnOptionsOpened()
    {
        _optionsOpen = true;
    }

    void OptionsMenu_OnOptionsClosed()
    {
        _optionsOpen = false;
        if(!_isPlayerTurn) { return; }
        if(_results.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_closeResultsButton);
        }
        else
        {
            SelectFirstInteractableButton();
        }
    }

    public void EndPlayerTurn() // UI Button
    {
        _enemyTurnSplash.SetActive(true);
        _isPlayerTurn = false;
        if(!_optionsOpen)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        HideAttackButtons();
        foreach(Button button in _buttons)
        {
            button.interactable = false;
        }
        _combatLog.text += $"\n{_currentEnemy.Name}'s Turn Begins!\n";
        Invoke(nameof(HideEnemySplash), _defaultDelay / 2.5f);
        Invoke(nameof(StartEnemyTurn), _defaultDelay);
    }

    void HideEnemySplash()
    {
        _enemyTurnSplash.SetActive(false);
    }

    void StartEnemyTurn()
    {
        if(!_currentEnemy.IsPoisoned)
        {
            _currentEnemy.AttackStarted();
            return;
        }

        _audioSource.PlayOneShot(_defaultHit); // Visual FX HERE

        bool enemyDead = _currentEnemy.TakeDamage(_currentEnemy.PoisonDamage);

        DamageSplash damageFX = Instantiate(_damageSplashPrefab, transform);
        damageFX.transform.position = new(damageFX.transform.position.x + UnityEngine.Random.Range(-300, 300), damageFX.transform.position.y + UnityEngine.Random.Range(-100, 200));
        damageFX.Setup(Color.yellow, Color.green, Color.red, _currentEnemy.PoisonDamage.FormatLargeNumbers()); // TODO Better colors

        _combatLog.text += $"\n{_currentEnemy.Name} Took\n{_currentEnemy.PoisonDamage} <color=green>Venom</color> Damage!\n";

        if(!enemyDead)
        {
            Invoke(nameof(PostPoisonEnemyAttack), _defaultDelay / 2.75f);
        }
    }

    void PostPoisonEnemyAttack()
    {
        _currentEnemy.AttackStarted();
    }

    public void CloseCombatMenu() // UI Button
    {
        OnCombatResolved?.Invoke();
        _results.SetActive(false);
        _resultsText.text = string.Empty;
        _combatMenu.SetActive(false);
    }

    public void HideAttackButtons()
    {
        foreach(GameObject attackButton in _attackButtons)
        {
            attackButton.SetActive(false);
        }
    }

    public void UseAttack(int index) // UI Button
    {
        if(!_currentEnemy) { return; }

        EventSystem.current.SetSelectedGameObject(null);
        HideAttackButtons();
        PlayerAbility selectedAbility = _playerInventory.GetAbility(index);
        _combatLog.text += $"\nYou Used {selectedAbility.Name}!\n";

        int toHitRoll = UnityEngine.Random.Range(0, 100);
        bool criticalHit = toHitRoll > 94;

        if(selectedAbility.AlwaysHits || criticalHit || toHitRoll + selectedAbility.HitChance + _playerStats.CurrentAccuracy - _currentEnemy.Evasion >= 100)
        {
            if(criticalHit)
            {
                _combatLog.text += $"\n<color=red>Critical Hit!</color>\n";
            }
            else
            {
                _combatLog.text += $"\nHit!\n";
            }
            selectedAbility.Hit();
            if(selectedAbility.DealsDamage)
            {
                int damageDealt = Mathf.Max(0, selectedAbility.Damage + _playerStats.CurrentStrength - _currentEnemy.Fortitude);
                if(criticalHit)
                {
                    damageDealt *= 2; // Making critical damage double regular damage isn't very interesting but it's fine for a game jam
                }
                _combatLog.text += $"\n{_currentEnemy.Name} Took\n{damageDealt} {selectedAbility.Adjective} Damage!\n";

                _audioSource.PlayOneShot(_defaultHit); // Visual FX HERE + Different one if(criticalHit)

                DamageSplash damageFX = Instantiate(_damageSplashPrefab, transform);
                damageFX.transform.position = new(damageFX.transform.position.x + UnityEngine.Random.Range(-300, 300), damageFX.transform.position.y + UnityEngine.Random.Range(-100, 200));
                damageFX.Setup(UnityEngine.Random.ColorHSV(), UnityEngine.Random.ColorHSV(), UnityEngine.Random.ColorHSV(), damageDealt.FormatLargeNumbers()); // TODO set colors through ability

                bool enemyDead = _currentEnemy.TakeDamage(damageDealt); // Without checking for Enemy death here, the wrong UI button will be selected upon combat end
                if(!enemyDead)
                {
                    SelectFirstInteractableButton();
                }
            }
            else
            {
                _audioSource.PlayOneShot(_defaultUse); // Visual FX HERE
                SelectFirstInteractableButton();
            }
        }
        else
        {
            _audioSource.PlayOneShot(_defaultMiss);
            _combatLog.text += $"\nMiss!\n"; // Visual FX HERE
            SelectFirstInteractableButton();
        }
    }

    public void SelectFirstInteractableButton()
    {
        if(_optionsOpen) { return; }

        foreach(Button button in _buttons)
        {
            if(button.interactable)
            {
                EventSystem.current.SetSelectedGameObject(button.gameObject);
                break;
            }
        }
    }

    void StartPlayerTurn()
    {
        _playerTurnSplash.SetActive(true);
        Invoke(nameof(HidePlayerTurnSplash), _defaultDelay / 2);
        _isPlayerTurn = true;
        _combatButtonsParent.SetActive(true);
        foreach(Button button in _buttons)
        {
            button.gameObject.SetActive(true);
            button.interactable = true;
        }
        if(!_optionsOpen)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_buttons[0].gameObject);
        }
        _combatLog.text += $"\nYour Turn Begins!\n";
    }

    void HidePlayerTurnSplash()
    {
        _playerTurnSplash.SetActive(false);
    }
}
