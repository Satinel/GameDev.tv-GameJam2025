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
    // TODO EventSystems setting of buttons to be selected for controller support everywhere

    Enemy _currentEnemy;
    bool _isPlayerTurn, _optionsOpen;

    void Start()
    {
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        Enemy.OnEnemyTurnEnd += Enemy_OnEnemyTurnEnd;
        Enemy.OnAnyEnemyKilled += Enemy_OnAnyEnemyKilled;
        OptionsMenu.OnOptionsOpened += OptionsMenu_OnOptionsOpened;
        OptionsMenu.OnOptionsClosed += OptionsMenu_OnOptionsClosed;
    }

    void OnDestroy()
    {
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        Enemy.OnEnemyTurnEnd -= Enemy_OnEnemyTurnEnd;
        Enemy.OnAnyEnemyKilled -= Enemy_OnAnyEnemyKilled;
        OptionsMenu.OnOptionsOpened += OptionsMenu_OnOptionsOpened;
        OptionsMenu.OnOptionsClosed -= OptionsMenu_OnOptionsClosed;
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
        _combatLog.text += $"\n{_currentEnemy.name} Was Defeated!";
        // TODO Message about gaining xp/money/item (also shown in _resultsText)
        if(!_optionsOpen)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_closeResultsButton);
        }
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
        _combatLog.text += $"\n{_currentEnemy.name} took {damageDealt} Damage!";
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
        _currentEnemy.Attack(); // TODO? Figure out how to make this a delegate or whatever
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
