using UnityEngine;
using System;

public class Exit : MonoBehaviour
{
    public static event Action<Transform> OnExitEntered;
    [SerializeField] Transform _emptyTransform;
    [SerializeField] BoxCollider _collider;
    [SerializeField] bool _isTutorial;
    bool _bossDefeated, _notFirstTrigger;

    void Start()
    {
        BossEncounter.OnBossDefeated += BossEncounter_OnBossDefeated;
        PlayerCombat.OnCombatResolved += ExitTriggered;
    }

    void OnDestroy()
    {
        BossEncounter.OnBossDefeated -= BossEncounter_OnBossDefeated;
        PlayerCombat.OnCombatResolved -= ExitTriggered;
    }

    void BossEncounter_OnBossDefeated()
    {
        _bossDefeated = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(!_isTutorial && !_bossDefeated) { return; }

        if(other.gameObject.GetComponent<PlayerHealth>())
        {
            OnExitEntered?.Invoke(_emptyTransform);
        }
    }

    void ExitTriggered()
    {
        if(!_bossDefeated) { return; }

        if(_notFirstTrigger) { return; }

        _notFirstTrigger = true;
        _collider.enabled = true;
        OnExitEntered?.Invoke(_emptyTransform);
    }
}
