using UnityEngine;
using System;

public class RestArea : MonoBehaviour
{
    public static event Action<Transform> OnRestAreaEntered;
    [SerializeField] Transform _emptyTransform;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerHealth>())
        {
            OnRestAreaEntered?.Invoke(_emptyTransform);
        }
    }
}
