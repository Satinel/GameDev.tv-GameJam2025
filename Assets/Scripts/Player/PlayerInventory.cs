using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public static event Action<Equipable, bool> OnWeaponEquipped;
    [field:SerializeField] public Equipable LeftClawWeapon { get; private set; }
    [field:SerializeField] public Equipable TailWeapon { get; private set; }
    [field:SerializeField] public Equipable RightClawWeapon { get; private set; }

    public bool HasKey { get; private set; }

    [SerializeField] Transform _trinketsParent, _weaponsParent;
    [SerializeField] Equipable _unarmedLeft, _unarmedRight;

    List<Equipable> _weapons = new();
    List<Trinket> _trinkets = new();

    void Start()
    {
        foreach(Equipable weapon in _weaponsParent.GetComponentsInChildren<Equipable>())
        {
            _weapons.Add(weapon);
        }

        foreach(Trinket trinket in _trinketsParent.GetComponentsInChildren<Trinket>())
        {
            _trinkets.Add(trinket);
        }
    }

    public PlayerAbility GetAbility(int index)
    {
        return index switch
        {
            0 => LeftClawWeapon.Ability1,
            1 => LeftClawWeapon.Ability2,
            2 => TailWeapon.Ability1,
            3 => TailWeapon.Ability2,
            4 => RightClawWeapon.Ability1,
            5 => RightClawWeapon.Ability2,
            _ => LeftClawWeapon.Ability1,
        };
    }

    public void EquipWeapon(Equipable weapon, bool isLeft)
    {
        if(isLeft)
        {
            LeftClawWeapon = weapon;
        }
        else
        {
            RightClawWeapon = weapon;
        }

        OnWeaponEquipped?.Invoke(weapon, isLeft);
    }

    public void RemoveWeapon(bool isLeft)
    {
        if(isLeft)
        {
            LeftClawWeapon = _unarmedLeft;
        }
        else
        {
            RightClawWeapon = _unarmedRight;
        }
    }

    public void GetKey()
    {
        HasKey = true;
    }

    public void AddTrinket(Trinket newTrinket)
    {
        foreach(Trinket trinket in _trinkets)
        {
            if(trinket.GetType() == newTrinket.GetType()) // If Player already has this exact type of trinket, upgrade it
            {
                trinket.LevelUp();
                return;
            }
        }

        Trinket addedTrinket = Instantiate(newTrinket, _trinketsParent);
        _trinkets.Add(addedTrinket);
    }
}
