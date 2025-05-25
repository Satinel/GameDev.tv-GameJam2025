using UnityEngine;
using System;

public class Exit : MonoBehaviour
{
    public static event Action<Transform> OnExitEntered;
    [SerializeField] Transform _emptyTransform;
    [SerializeField] BoxCollider _collider;
    bool _notFirstTrigger;

    void Start()
    {
        PlayerCombat.OnCombatResolved += ExitTriggered;
    }

    void OnDestroy()
    {
        PlayerCombat.OnCombatResolved -= ExitTriggered;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerHealth>())
        {
            OnExitEntered?.Invoke(_emptyTransform);
        }
    }

    void ExitTriggered()
    {
        if(_notFirstTrigger) { return; }

        _notFirstTrigger = true;
        _collider.enabled = true;
        OnExitEntered?.Invoke(_emptyTransform);
    }
}
