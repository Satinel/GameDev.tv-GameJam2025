using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _lookSpeed = 2.5f;

    bool _moveForward, _moveLeft, _moveRight;
    Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            _moveForward = true;
        }
        else
        {
            _moveForward = false;
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
        if(_moveForward)
        {
            _rigidbody.linearVelocity = _moveSpeed * Time.deltaTime * transform.forward;
        }
        else if(_moveLeft)
        {
            _rigidbody.linearVelocity = _moveSpeed * .35f * Time.deltaTime * -transform.right;
        }
        else if(_moveRight)
        {
            _rigidbody.linearVelocity = _moveSpeed * .35f * Time.deltaTime * transform.right;
        }
        else
        {
            _rigidbody.linearVelocity = Vector3.zero;
        }
    }
}
