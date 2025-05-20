using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static event Action<Equipable, bool> OnWeaponEquipped;
    [field:SerializeField] public Equipable LeftClawWeapon { get; private set; }
    [field:SerializeField] public Equipable TailWeapon { get; private set; }
    [field:SerializeField] public Equipable RightClawWeapon { get; private set; }

    public bool HasKey { get; private set; }

    public PlayerAbility GetAbility(int index)
    {
        switch(index)
        {
            case 0:
                return LeftClawWeapon.Ability1;
            case 1:
                return LeftClawWeapon.Ability2;
            case 2:
                return TailWeapon.Ability1;
            case 3:
                return TailWeapon.Ability2;
            case 4:
                return RightClawWeapon.Ability1;
            case 5:
                return RightClawWeapon.Ability2;
            default:
                return LeftClawWeapon.Ability1;
        }
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
