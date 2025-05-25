using UnityEngine;
using System;

public class Exit : MonoBehaviour
{
    public static event Action<Transform> OnExitEntered;
    [SerializeField] Transform _emptyTransform;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerHealth>())
        {
            OnExitEntered?.Invoke(_emptyTransform);
        }
    }
}
