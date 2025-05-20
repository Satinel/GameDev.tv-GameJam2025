using UnityEngine;

public class Equipable : MonoBehaviour
{
    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public PlayerAbility Ability1 { get; private set; }
    [field:SerializeField] public PlayerAbility Ability2 { get; private set; }
    [field:SerializeField] public bool IsLeftSlot { get; private set; }

    public bool IsEquipped { get; private set; }

    void OnEnable()
    {
        PlayerInventory.OnWeaponEquipped += PlayerInventory_OnWeaponEquipped;
    }

    void OnDisable()
    {
        PlayerInventory.OnWeaponEquipped -= PlayerInventory_OnWeaponEquipped;
    }

    void PlayerInventory_OnWeaponEquipped(Equipable weapon, bool isLeftSlot)
    {
        if(IsLeftSlot != isLeftSlot) { return; }

        if(weapon != this)
        {
            IsEquipped = false;
        }
        else
        {
            IsEquipped = true;
        }
    }
}
