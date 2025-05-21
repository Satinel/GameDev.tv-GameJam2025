using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _lookSpeed = 2.5f;
    [SerializeField] float _autoRotateSpeed = 1f;

    bool _moveForward, _moveBackward, _moveLeft, _moveRight;
    bool _isFighting, _eventStarted, _optionsOpen, _isRotating;
    Rigidbody _rigidbody;
    Quaternion _targetRotation;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        PlayerCombat.OnCombatResolved += PlayerCombat_OnCombatResolved;
        OptionsMenu.OnOptionsOpened += MenuOptions_OnOptionsOpened;
        OptionsMenu.OnOptionsClosed += MenuOptions_OnOptionsClosed;
        DeadEnd.OnAnyDeadEndEvent += DeadEnd_OnAnyDeadEndEvent;
        // TODO Set _eventStarted to false
    }

    void OnDisable()
    {
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        PlayerCombat.OnCombatResolved -= PlayerCombat_OnCombatResolved;
        OptionsMenu.OnOptionsOpened -= MenuOptions_OnOptionsOpened;
        OptionsMenu.OnOptionsClosed -= MenuOptions_OnOptionsClosed;
        DeadEnd.OnAnyDeadEndEvent -= DeadEnd_OnAnyDeadEndEvent;
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

        if(_isFighting || _eventStarted || _optionsOpen) { return;}

        if(Input.GetKey(KeyCode.W))
        {
            _moveForward = true;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            _moveBackward = true;
            _moveForward = false;
        }
        else
        {
            _moveForward = false;
            _moveBackward = false;
        }

        if(Input.GetKey(KeyCode.A))
        {
            _moveLeft = true;
        }
        else{ _moveLeft = false; }

        if(Input.GetKey(KeyCode.D))
        {
            _moveRight = true;
        }
        else{ _moveRight = false; }

        if(Input.mousePositionDelta.x < 0)
        {
            transform.Rotate(0, -1 * _lookSpeed * Time.deltaTime, 0);
        }
        else if(Input.mousePositionDelta.x > 0)
        {
            transform.Rotate(0, 1 * _lookSpeed * Time.deltaTime, 0);
        }
    }

    void FixedUpdate()
    {
        _rigidbody.linearVelocity = Vector3.zero;

        if(_isFighting || _optionsOpen) { return;}

        if(_moveForward)
        {
            _rigidbody.linearVelocity += _moveSpeed * Time.deltaTime * transform.forward;
        }
        if(_moveBackward)
        {
            _rigidbody.linearVelocity += _moveSpeed * 0.45f * Time.deltaTime * -transform.forward;
        }
        if(_moveLeft)
        {
            _rigidbody.linearVelocity += _moveSpeed * .75f * Time.deltaTime * -transform.right;
        }
        if(_moveRight)
        {
            _rigidbody.linearVelocity += _moveSpeed * .75f * Time.deltaTime * transform.right;
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

    void MenuOptions_OnOptionsOpened()
    {
        _optionsOpen = true;
    }

    void MenuOptions_OnOptionsClosed()
    {
        _optionsOpen = false;
    }

    void DeadEnd_OnAnyDeadEndEvent()
    {
        _eventStarted = true;
    }
}
