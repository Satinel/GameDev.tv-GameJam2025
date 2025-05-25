using UnityEngine;

public class Tigey : MonoBehaviour
{
    Transform _playerTransform;

    void Start()
    {
        _playerTransform = FindFirstObjectByType<PlayerController>().transform;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(_playerTransform.position - transform.position);
    }
}
