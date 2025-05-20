using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerCombat : MonoBehaviour
{
    public static event Action OnCombatResolved;

    [SerializeField] GameObject _combatMenu, _combatButtonsParent, _results;
    [SerializeField] GameObject[] _attackButtons;
    [SerializeField] Button[] _buttons;
    [SerializeField] GameObject _closeResultsButton;
    [SerializeField] TextMeshProUGUI _combatLog, _resultsText;

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
        _results.SetActive(true); // TODO Replace with _finalResults.SetActive(true)
        if(!_optionsOpen)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_closeResultsButton); // TODO Replace with EventSystem.current.SetSelectedGameObject(_closeFinalResultsButton);
        }
        _combatLog.text += $"\nYou Were Defeated!\n";
    }

    void Enemy_OnFightStarted(Enemy enemy)
    {
        _combatButtonsParent.SetActive(true);
        _combatMenu.SetActive(true);
        _currentEnemy = enemy;
        // TODO Initiative Role:
        int playerRoll = UnityEngine.Random.Range(0, 20);
        _combatLog.text += $"\nYou Roll Initiative!\n{playerRoll + _playerStats.Initiative} ({playerRoll} + {_playerStats.Initiative})\n";
        int enemyRoll = UnityEngine.Random.Range(0, 20);
        _combatLog.text += $"\n{_currentEnemy.Name} Rolls Initiative!\n{enemyRoll + _currentEnemy.Initiative} ({enemyRoll} + {_currentEnemy.Initiative})\n";
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
        StartPlayerTurn();
    }

    void Enemy_OnAnyEnemyKilled(Enemy enemy)
    {
        HideAttackButtons();
        _combatButtonsParent.SetActive(false);
        _results.SetActive(true);
        _combatLog.text += $"\n{_currentEnemy.Name} Was Defeated!\n";
        // TODO Message about gaining xp/money/item (also shown in _resultsText)
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
        if(ability.AlwaysHits || UnityEngine.Random.Range(0, 100) + ability.HitChance >= 100) // TODO Take _playerStats.CurrentEvasion into account
        {
            _combatLog.text += $"\nHit!\n";
            ability.Hit(); // TODO Effects other than Damage
            if(ability.DealsDamage)
            {
                 // TODO Take _playerStats.Fortitude into account ie. int damageDealt = ability.Damage - _playerStats.CurrentFortitude (if damageDealt < 0) damageDealt = 0;
                _combatLog.text += $"\nYou Take {ability.Damage} {ability.Adjective} Damage!\n";
                _playerHealth.TakeDamage(ability.Damage); // TODO? Critical Chance/Damage
            }
            // TODO Play Hit sound
        }
        else
        {
            _combatLog.text += $"\nMiss!\n";
            // TODO Play Missed sound
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
        _currentEnemy.AttackStarted();
    }

    public void CloseCombatMenu() // UI Button
    {
        OnCombatResolved?.Invoke();
        _results.SetActive(false);
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

        if(selectedAbility.AlwaysHits || UnityEngine.Random.Range(0, 100) + selectedAbility.HitChance >= 100) // TODO Take _playerStats.CurrentAccuracy into account
        {
            _combatLog.text += $"\nHit!\n";
            selectedAbility.Hit(); // TODO Effects other than Damage
            if(selectedAbility.DealsDamage)
            {
                int damageDealt = selectedAbility.Damage + _playerStats.CurrentStrength; // TODO? Critical Chance/Damage TODO? Something more interesting than just + CurrentStrength (multiplyer, etc.)
                _combatLog.text += $"\n{_currentEnemy.Name} Took\n{damageDealt} {selectedAbility.Adjective} Damage!\n";
                // TODO Play Hit sound
                bool enemyDead = _currentEnemy.TakeDamage(damageDealt);
                if(!enemyDead)
                {
                    SelectFirstInteractableButton();
                }
            }
        }
        else
        {
            // TODO Play Missed sound
            _combatLog.text += $"\nMiss!\n";
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
        _isPlayerTurn = true;
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
}
