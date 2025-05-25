using UnityEngine;

public class PlayerAbilitySwapper : PlayerAbility
{
    [SerializeField] int _accuracyBuff;
    [SerializeField] Equipable _swappedEquipment;
    [SerializeField] Equipable _baseEquipable;

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
    }
}
