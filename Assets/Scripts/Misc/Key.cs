using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] float _rotationSpeed;

    void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }
}
