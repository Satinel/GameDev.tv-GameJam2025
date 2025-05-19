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
    Enemy _currentEnemy;
    bool _isPlayerTurn, _optionsOpen;

    void Awake()
    {
        _playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    void Start()
    {
        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        Enemy.OnEnemyTurnEnd += Enemy_OnEnemyTurnEnd;
        Enemy.OnEnemyKilled += Enemy_OnAnyEnemyKilled;
        EnemyAbility.OnEnemyAbilityStarted += EnemyAbility_OnEnemyAbilityStarted;
        EnemyAbility.OnEnemyAbilityUsed += EnemyAbility_OnEnemyAbilityUsed;
        OptionsMenu.OnOptionsOpened += OptionsMenu_OnOptionsOpened;
        OptionsMenu.OnOptionsClosed += OptionsMenu_OnOptionsClosed;
    }

    void OnDestroy()
    {
        PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        Enemy.OnEnemyTurnEnd -= Enemy_OnEnemyTurnEnd;
        Enemy.OnEnemyKilled -= Enemy_OnAnyEnemyKilled;
        EnemyAbility.OnEnemyAbilityStarted -= EnemyAbility_OnEnemyAbilityStarted;
        EnemyAbility.OnEnemyAbilityUsed -= EnemyAbility_OnEnemyAbilityUsed;
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
        // TODO Initiative Role?
        StartPlayerTurn();
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
        if(ability.AlwaysHits || UnityEngine.Random.Range(0, 100) + ability.HitChance >= 100)
        {
            ability.Hit(); // TODO Effects other than Damage
            _combatLog.text += $"\nHit!\n";
            if(ability.DealsDamage)
            {
                _combatLog.text += $"\nYou Take {ability.Damage} {ability.Adjective} Damage!\n";
                _playerHealth.TakeDamage(ability.Damage);
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
        EventSystem.current.SetSelectedGameObject(null);
        HideAttackButtons();
        // TODO To-Hit Role/Damage/Status/etc. aka actual combat
        int damageDealt = 5 * index; // TODO use index to call attacks from player equipment (or whatever)
        bool enemyDead = _currentEnemy.TakeDamage(damageDealt); // Remember to fix this!
        _combatLog.text += $"\n{_currentEnemy.Name} Took {damageDealt} Damage!\n";
        if(!enemyDead)
        {
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
    }
}
