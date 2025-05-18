using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    public static event Action OnCombatResolved;

    [SerializeField] GameObject _combatMenu, _combatButtonsParent, _results;
    [SerializeField] GameObject[] _attackButtons;
    [SerializeField] Button[] _buttons;
    [SerializeField] TextMeshProUGUI _combatLog, _resultsText;

    // [SerializeField] GameObject _leftAttack1, _leftAttack2;
    // [SerializeField] GameObject _rightAttack1, _rightAttack2;
    // [SerializeField] GameObject _tailAttack1, _tailAttack2;

    // TODO Set/Get equipped items in claws if any
    // TODO EventSystems setting of buttons to be selected for controller support everywhere

    Enemy _currentEnemy;

    void Start()
    {
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        Enemy.OnEnemyTurnEnd += Enemy_OnEnemyTurnEnd;
        Enemy.OnAnyEnemyKilled += Enemy_OnAnyEnemyKilled;
    }

    void OnDestroy()
    {
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        Enemy.OnEnemyTurnEnd -= Enemy_OnEnemyTurnEnd;
        Enemy.OnAnyEnemyKilled -= Enemy_OnAnyEnemyKilled;
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
        _combatButtonsParent.SetActive(false);
        _results.SetActive(true);
        _combatLog.text += $"\n{_currentEnemy.name} Was Defeated!";
        // TODO Message about gaining xp/money/item (also shown in _resultsText)
    }

    public void CloseCombatMenu() // UI Button
    {
        OnCombatResolved?.Invoke();
        _results.SetActive(false);
        _combatMenu.SetActive(false);
    }

    // public void ShowAttacks(GameObject attack1, GameObject attack2) // UI Button
    // {
    //     attack1.SetActive(true);
    //     attack2.SetActive(true);
    // }

    void HideAttackButtons()
    {
        foreach(GameObject attackButton in _attackButtons)
        {
            attackButton.SetActive(false);
        }
    }

    public void UseAttack(int index) // UI Button
    {
        HideAttackButtons();
        // TODO To-Hit Role/Damage/Status/etc. aka actual combat
        int damageDealt = 5 * index; // TODO use index to call attacks from player equipment (or whatever)
        _currentEnemy.TakeDamage(damageDealt); // Remember to fix this!
        _combatLog.text += $"\n{_currentEnemy.name} took {damageDealt} Damage!";
    }

    public void EndPlayerTurn() // UI Button
    {
        foreach(Button button in _buttons)
        {
            button.interactable = false;
        }
        _currentEnemy.Attack(); // TODO? Figure out how to make this a delegate or whatever
    }

    void StartPlayerTurn()
    {
        foreach(Button button in _buttons)
        {
            button.gameObject.SetActive(true);
            button.interactable = true;
        }
    }
}
