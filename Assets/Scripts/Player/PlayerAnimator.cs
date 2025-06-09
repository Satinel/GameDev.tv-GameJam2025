using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator _leftAnimator, _rightAnimator, _tailAnimator;
    [SerializeField] Animator _leftShadow, _rightShadow, _tailShadow;
    bool _isFighting, _eventStarted, _optionsOpen, _inventoryOpen;

    void Awake()
    {
        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        GameOverSplash.OnRespawn += GameOverSplash_OnRespawn;
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        Enemy.OnEnemyKilled += Enemy_OnAnyEnemyKilled;
        PlayerCombat.OnPlayerAbilityUsed += PlayerCombat_OnPlayerAbilityUsed;
        PlayerCombat.OnCombatResolved += PlayerCombat_OnCombatResolved;
        OptionsMenu.OnOptionsOpened += OptionsMenu_OnOptionsOpened;
        OptionsMenu.OnOptionsClosed += OptionsMenu_OnOptionsClosed;
        InventoryUI.OnInventoryOpened += InventoryUI_OnInventoryOpened;
        InventoryUI.OnInventoryClosed += InventoryUI_OnInventoryClosed;
        DeadEnd.OnAnyDeadEndEvent += DeadEnd_OnAnyDeadEndEvent;
        Store.OnEnteredStore += Store_OnEnteredStore;
        StoreUI.OnExitStore += StoreUI_OnExitStore;
        Exit.OnExitEntered += Exit_OnExitEntered;
        ExitUI.OnExitResolved += ExitUI_OnExitResolved;
        RestArea.OnRestAreaEntered += RestArea_OnRestAreaEntered;
        RestAreaUI.OnRestAreaResolved += RestAreaUI_OnRestAreaResolved;
    }

    void OnDestroy()
    {
        PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
        GameOverSplash.OnRespawn -= GameOverSplash_OnRespawn;
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        Enemy.OnEnemyKilled -= Enemy_OnAnyEnemyKilled;
        PlayerCombat.OnPlayerAbilityUsed -= PlayerCombat_OnPlayerAbilityUsed;
        PlayerCombat.OnCombatResolved -= PlayerCombat_OnCombatResolved;
        OptionsMenu.OnOptionsOpened -= OptionsMenu_OnOptionsOpened;
        OptionsMenu.OnOptionsClosed -= OptionsMenu_OnOptionsClosed;
        InventoryUI.OnInventoryOpened -= InventoryUI_OnInventoryOpened;
        InventoryUI.OnInventoryClosed -= InventoryUI_OnInventoryClosed;
        DeadEnd.OnAnyDeadEndEvent -= DeadEnd_OnAnyDeadEndEvent;
        Store.OnEnteredStore -= Store_OnEnteredStore;
        StoreUI.OnExitStore -= StoreUI_OnExitStore;
        Exit.OnExitEntered -= Exit_OnExitEntered;
        ExitUI.OnExitResolved -= ExitUI_OnExitResolved;
        RestArea.OnRestAreaEntered -= RestArea_OnRestAreaEntered;
        RestAreaUI.OnRestAreaResolved -= RestAreaUI_OnRestAreaResolved;
    }

    public void OnMove(InputValue value)
    {
        if(_isFighting || _eventStarted || _optionsOpen || _inventoryOpen) { return; }

        if(value.Get<Vector2>() != Vector2.zero)
        {
            _leftAnimator.SetBool("IsMoving", true);
            _rightAnimator.SetBool("IsMoving", true);
            _tailAnimator.SetBool("IsMoving", true);
            _leftShadow.SetBool("IsMoving", true);
            _rightShadow.SetBool("IsMoving", true);
            _tailShadow.SetBool("IsMoving", true);
        }
        else
        {
            _leftAnimator.SetBool("IsMoving", false);
            _rightAnimator.SetBool("IsMoving", false);
            _tailAnimator.SetBool("IsMoving", false);
            _leftShadow.SetBool("IsMoving", false);
            _rightShadow.SetBool("IsMoving", false);
            _tailShadow.SetBool("IsMoving", false);
        }
    }

    void PlayerHealth_OnPlayerDeath()
    {
        _leftAnimator.SetBool("InCombat", false);
        _rightAnimator.SetBool("InCombat", false);
        _tailAnimator.SetBool("InCombat", false);
        _leftShadow.SetBool("InCombat", false);
        _rightShadow.SetBool("InCombat", false);
        _tailShadow.SetBool("InCombat", false);
        _leftAnimator.enabled = false;
        _rightAnimator.enabled = false;
        _tailAnimator.enabled = false;
        _leftShadow.enabled = false;
        _rightShadow.enabled = false;
        _tailShadow.enabled = false;
    }

    void GameOverSplash_OnRespawn()
    {
        _leftAnimator.enabled = true;
        _rightAnimator.enabled = true;
        _tailAnimator.enabled = true;
        _leftShadow.enabled = true;
        _rightShadow.enabled = true;
        _tailShadow.enabled = true;
    }

    void Enemy_OnFightStarted(Enemy enemy)
    {
        _isFighting = true;
        _leftAnimator.SetBool("InCombat", true);
        _leftAnimator.SetBool("IsMoving", false);
        _rightAnimator.SetBool("InCombat", true);
        _rightAnimator.SetBool("IsMoving", false);
        _tailAnimator.SetBool("InCombat", true);
        _tailAnimator.SetBool("IsMoving", false);
        _leftShadow.SetBool("InCombat", true);
        _leftShadow.SetBool("IsMoving", false);
        _rightShadow.SetBool("InCombat", true);
        _rightShadow.SetBool("IsMoving", false);
        _tailShadow.SetBool("InCombat", true);
        _tailShadow.SetBool("IsMoving", false);
    }

    void Enemy_OnAnyEnemyKilled(Enemy enemy)
    {
        _leftAnimator.SetBool("InCombat", false);
        _rightAnimator.SetBool("InCombat", false);
        _tailAnimator.SetBool("InCombat", false);
        _leftShadow.SetBool("InCombat", false);
        _rightShadow.SetBool("InCombat", false);
        _tailShadow.SetBool("InCombat", false);
    }

    void PlayerCombat_OnPlayerAbilityUsed(int index, string clip)
    {
        switch(index)
        {
            case 0:
                _leftAnimator.Play(clip);
                _leftShadow.Play(clip);
                break;
            case 1:
                _leftAnimator.Play(clip);
                _leftShadow.Play(clip);
                break;
            case 2:
                _tailAnimator.Play(clip);
                _tailShadow.Play(clip);
                break;
            case 3:
                _tailAnimator.Play(clip);
                _tailShadow.Play(clip);
                break;
            case 4:
                _rightAnimator.Play(clip);
                _rightShadow.Play(clip);
                break;
            case 5:
                _rightAnimator.Play(clip);
                _rightShadow.Play(clip);
                break;
            default:
                _leftAnimator.Play(clip);
                _leftShadow.Play(clip);
                break;
        }
    }

    void PlayerCombat_OnCombatResolved()
    {
        _isFighting = false;
    }

    void OptionsMenu_OnOptionsOpened()
    {
        _optionsOpen = true;
    }

    void OptionsMenu_OnOptionsClosed()
    {
        _optionsOpen = false;
    }

    void InventoryUI_OnInventoryOpened()
    {
        _inventoryOpen = true;
    }

    void InventoryUI_OnInventoryClosed()
    {
        _inventoryOpen = false;
    }

    void DeadEnd_OnAnyDeadEndEvent()
    {
        _eventStarted = true;
    }

    void Store_OnEnteredStore(Transform tigey)
    {
        _eventStarted = true;
    }

    void StoreUI_OnExitStore()
    {
        _eventStarted = false;
    }

    void Exit_OnExitEntered(Transform empty)
    {
        _eventStarted = true;
    }

    void ExitUI_OnExitResolved()
    {
        _eventStarted = false;
    }

    void RestArea_OnRestAreaEntered(Transform empty)
    {
        _eventStarted = true;
    }

    void RestAreaUI_OnRestAreaResolved()
    {
        _eventStarted = false;
    }
}
