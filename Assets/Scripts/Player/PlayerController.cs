using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static event Action OnMapPressed;
    public static event Action OnInventoryPressed;
    public static event Action OnLogPressed;
    public static event Action OnStatsPressed;
    public static event Action OnOptionsPressed;

    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _lookSpeed = 2.5f;
    [SerializeField] float _autoRotateSpeed = 1f;

    bool _moveForward, _moveBackward, _moveLeft, _moveRight;
    bool _lookLeft, _lookRight;
    bool _isFighting, _eventStarted, _optionsOpen, _inventoryOpen, _isRotating;
    Rigidbody _rigidbody;
    Quaternion _targetRotation;

    public void OnMove(InputValue value)
    {
        _moveForward = value.Get<Vector2>().y > 0.5;
        _moveLeft = value.Get<Vector2>().x < -0.5;
        _moveBackward = value.Get<Vector2>().y < -0.5;
        _moveRight = value.Get<Vector2>().x > 0.5;
    }

    public void OnLook(InputValue value)
    {
        _lookLeft = value.Get<Vector2>().x < -0.5;
        _lookRight = value.Get<Vector2>().x > 0.5;
    }

    public void OnMap(InputValue value)
    {
        if(value.isPressed)
        {
            OnMapPressed?.Invoke();
        }
    }

    public void OnInventory(InputValue value)
    {
        if(value.isPressed)
        {
            OnInventoryPressed?.Invoke();
        }
    }

    public void OnLog(InputValue value)
    {
        if(value.isPressed)
        {
            OnLogPressed?.Invoke();
        }
    }

    public void OnStats(InputValue value)
    {
        if(value.isPressed)
        {
            OnStatsPressed?.Invoke();
        }
    }

    public void OnOptions(InputValue value)
    {
        if(value.isPressed)
        {
            OnOptionsPressed?.Invoke();
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        Enemy.OnFightStarted += Enemy_OnFightStarted;
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

    void OnDisable()
    {
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
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

    void Update()
    {
        if(_isRotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, _autoRotateSpeed * Time.deltaTime);
            if(Quaternion.Angle(transform.rotation, _targetRotation) < 1f)
            {
                _isRotating = false;
            }
        }

        if(_isFighting || _eventStarted || _optionsOpen || _inventoryOpen)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if(_lookLeft)
        {
            transform.Rotate(0, -1 * _lookSpeed * Time.deltaTime, 0);
        }

        if(_lookRight)
        {
            transform.Rotate(0, 1 * _lookSpeed * Time.deltaTime, 0);
        }
    }

    void FixedUpdate()
    {
        _rigidbody.linearVelocity = Vector3.zero;

        if(_isFighting || _eventStarted || _optionsOpen || _inventoryOpen) { return; }

        if(_moveForward)
        {
            _rigidbody.linearVelocity += _moveSpeed * Time.deltaTime * transform.forward;
        }
        if(_moveBackward)
        {
            _rigidbody.linearVelocity += _moveSpeed * 0.75f * Time.deltaTime * -transform.forward;
        }
        if(_moveLeft)
        {
            _rigidbody.linearVelocity += _moveSpeed * .9f * Time.deltaTime * -transform.right;
        }
        if(_moveRight)
        {
            _rigidbody.linearVelocity += _moveSpeed * .9f * Time.deltaTime * transform.right;
        }
    }

    void Enemy_OnFightStarted(Enemy enemy)
    {
        _isFighting = true;
        Vector3 lookAtTarget = new(enemy.transform.position.x, transform.position.y, enemy.transform.position.z);
        _targetRotation = Quaternion.LookRotation(lookAtTarget - transform.position);
        _isRotating = true;
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
        Vector3 lookAtTarget = new(tigey.transform.position.x, transform.position.y, tigey.transform.position.z);
        _targetRotation = Quaternion.LookRotation(lookAtTarget - transform.position);
        _isRotating = true;
    }

    void StoreUI_OnExitStore()
    {
        _eventStarted = false;
        _isRotating = false;
    }

    void Exit_OnExitEntered(Transform empty)
    {
        _eventStarted = true;
        Vector3 lookAtTarget = new(empty.transform.position.x, transform.position.y, empty.transform.position.z);
        _targetRotation = Quaternion.LookRotation(lookAtTarget - transform.position);
        _isRotating = true;
    }

    void ExitUI_OnExitResolved()
    {
        _eventStarted = false;
        _isRotating = false;
    }

    void RestArea_OnRestAreaEntered(Transform empty)
    {
        _eventStarted = true;
        Vector3 lookAtTarget = new(empty.transform.position.x, transform.position.y, empty.transform.position.z);
        _targetRotation = Quaternion.LookRotation(lookAtTarget - transform.position);
        _isRotating = true;
    }

    void RestAreaUI_OnRestAreaResolved()
    {
        _eventStarted = false;
        _isRotating = false;
    }
}
