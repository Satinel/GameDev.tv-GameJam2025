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
    [SerializeField] float _mouseSensitivity = 1f;
    [SerializeField] Texture2D _customCursor;

    Vector2 _moveValue = Vector2.zero;
    float _lookValue;
    bool _isFighting, _eventStarted, _optionsOpen, _inventoryOpen, _isRotating;
    Rigidbody _rigidbody;
    Quaternion _targetRotation;

    public void OnMove(InputValue value)
    {
        _moveValue = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        _lookValue = value.Get<Vector2>().x;
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
        _mouseSensitivity = PlayerPrefs.GetFloat("MouseLook", 1);
    }

    void Start()
    {
        HideCursor();
    }

    void OnEnable()
    {
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        PlayerCombat.OnCombatResolved += PlayerCombat_OnCombatResolved;
        OptionsMenu.OnOptionsOpened += OptionsMenu_OnOptionsOpened;
        OptionsMenu.OnOptionsClosed += OptionsMenu_OnOptionsClosed;
        OptionsMenu.OnMouseLookChanged += OptionsMenu_OnMouseLookChanged;
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
        OptionsMenu.OnMouseLookChanged -= OptionsMenu_OnMouseLookChanged;
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
#if UNITY_WEBGL
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
#endif
            return;
        }
        else
        {
#if UNITY_WEBGL
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
#endif
        }

        transform.Rotate(0, _lookValue * _lookSpeed * _mouseSensitivity * Time.deltaTime, 0);
    }

    void FixedUpdate()
    {
        if(_isFighting || _eventStarted || _optionsOpen || _inventoryOpen)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            return;
        }

        _rigidbody.linearVelocity = _moveSpeed * Time.deltaTime * _moveValue.y * transform.forward;
        _rigidbody.linearVelocity += _moveSpeed * Time.deltaTime * _moveValue.x * transform.right;
    }

    void ShowCursor()
    {
#if !UNITY_WEBGL
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.SetCursor(_customCursor, default, CursorMode.Auto);
        Cursor.visible = true;
#endif
    }

    void HideCursor()
    {
#if !UNITY_WEBGL
        if(_isFighting || _eventStarted || _optionsOpen || _inventoryOpen) { return; }

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
#endif
    }

    void Enemy_OnFightStarted(Enemy enemy)
    {
        _isFighting = true;
        ShowCursor();
        Vector3 lookAtTarget = new(enemy.transform.position.x, transform.position.y, enemy.transform.position.z);
        _targetRotation = Quaternion.LookRotation(lookAtTarget - transform.position);
        _isRotating = true;
    }

    void PlayerCombat_OnCombatResolved()
    {
        _isFighting = false;
        HideCursor();
    }

    void OptionsMenu_OnOptionsOpened()
    {
        _optionsOpen = true;
        ShowCursor();
    }

    void OptionsMenu_OnOptionsClosed()
    {
        _optionsOpen = false;
        HideCursor();
    }

    void OptionsMenu_OnMouseLookChanged(float value)
    {
        _mouseSensitivity = value;
    }

    void InventoryUI_OnInventoryOpened()
    {
        _inventoryOpen = true;
        ShowCursor();
    }

    void InventoryUI_OnInventoryClosed()
    {
        _inventoryOpen = false;
        HideCursor();
    }

    void DeadEnd_OnAnyDeadEndEvent()
    {
        _eventStarted = true;
        ShowCursor();
    }

    void Store_OnEnteredStore(Transform tigey)
    {
        _eventStarted = true;
        Vector3 lookAtTarget = new(tigey.transform.position.x, transform.position.y, tigey.transform.position.z);
        _targetRotation = Quaternion.LookRotation(lookAtTarget - transform.position);
        _isRotating = true;
        ShowCursor();
    }

    void StoreUI_OnExitStore()
    {
        _eventStarted = false;
        _isRotating = false;
        HideCursor();
    }

    void Exit_OnExitEntered(Transform empty)
    {
        _eventStarted = true;
        Vector3 lookAtTarget = new(empty.transform.position.x, transform.position.y, empty.transform.position.z);
        _targetRotation = Quaternion.LookRotation(lookAtTarget - transform.position);
        _isRotating = true;
        ShowCursor();
    }

    void ExitUI_OnExitResolved()
    {
        _eventStarted = false;
        _isRotating = false;
        HideCursor();
    }

    void RestArea_OnRestAreaEntered(Transform empty)
    {
        _eventStarted = true;
        Vector3 lookAtTarget = new(empty.transform.position.x, transform.position.y, empty.transform.position.z);
        _targetRotation = Quaternion.LookRotation(lookAtTarget - transform.position);
        _isRotating = true;
        ShowCursor();
    }

    void RestAreaUI_OnRestAreaResolved()
    {
        _eventStarted = false;
        _isRotating = false;
        HideCursor();
    }
}
