using System;
using UnityEngine;

public class PlayerAbilitySwapper : PlayerAbility
{
    public static event Action<int> OnGrabStateChanged;

    [SerializeField] int _accuracyBuff;
    [SerializeField] Equipable _swappedEquipment;
    [SerializeField] Equipable _baseEquipable;
    [SerializeField] bool _activatesOnMiss;

    PlayerInventory _playerInventory;
    PlayerStats _playerStats;

    void Awake()
    {
        _playerInventory = GetComponentInParent<PlayerInventory>();
        _playerStats = GetComponentInParent<PlayerStats>();
    }

    void OnEnable()
    {
        PlayerCombat.OnCombatResolved += PlayerCombat_OnCombatResolved;
    }

    void OnDisable()
    {
        PlayerCombat.OnCombatResolved -= PlayerCombat_OnCombatResolved;
    }

    void PlayerCombat_OnCombatResolved()
    {
        if(!_baseEquipable) { return; }

        if(_swappedEquipment.IsEquipped)
        {
            _playerInventory.EquipWeapon(_baseEquipable, _baseEquipable.IsLeftSlot);
        }
    }

    public override void Hit()
    {
        base.Hit();
        _playerStats.GainTempBonus(PlayerStats.Stats.Accuracy, _accuracyBuff);
        _playerInventory.EquipWeapon(_swappedEquipment, _swappedEquipment.IsLeftSlot);
        OnGrabStateChanged?.Invoke(_accuracyBuff);
    }

    public override void Miss()
    {
        if(_activatesOnMiss)
        {
            _playerStats.GainTempBonus(PlayerStats.Stats.Accuracy, _accuracyBuff);
            _playerInventory.EquipWeapon(_swappedEquipment, _swappedEquipment.IsLeftSlot);
            OnGrabStateChanged?.Invoke(_accuracyBuff);
        }
    }
}
