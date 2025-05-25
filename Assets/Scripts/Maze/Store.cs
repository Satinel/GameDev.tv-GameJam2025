using UnityEngine;
using System;

public class Store : MonoBehaviour
{
    public static event Action<Transform> OnEnteredStore;

    [SerializeField] Transform _tigey;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerHealth>())
        {
            OnEnteredStore?.Invoke(_tigey);
        }
    }
}
