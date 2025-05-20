using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    public static event Action<Equipable, bool> OnWeaponEquipped;
    [field:SerializeField] public Equipable LeftClawWeapon { get; private set; }
    [field:SerializeField] public Equipable TailWeapon { get; private set; }
    [field:SerializeField] public Equipable RightClawWeapon { get; private set; }

    public bool HasKey { get; private set; }

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

    public void GetKey()
    {
        HasKey = true;
    }
}
