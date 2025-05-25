using UnityEngine;
using TMPro;

public class CombatToolTip : MonoBehaviour
{
    [SerializeField] GameObject _toolTip;
    [SerializeField] TextMeshProUGUI _tipText;

    void Start()
    {
        PlayerInventory.OnAbilityFocused += PlayerInventory_OnAbilityFocused;
        PlayerInventory.OnAbilityNotFocused += PlayerInventory_OnAbilityNotFocused;

    }

    void OnDestroy()
    {
        PlayerInventory.OnAbilityFocused -= PlayerInventory_OnAbilityFocused;
        PlayerInventory.OnAbilityNotFocused -= PlayerInventory_OnAbilityNotFocused;
    }

    void PlayerInventory_OnAbilityFocused(PlayerAbility ability)
    {
        _tipText.text = ability.Description;
        _toolTip.SetActive(true);
    }

    void PlayerInventory_OnAbilityNotFocused()
    {
        _toolTip.SetActive(false);
        _tipText.text = string.Empty;
    }
}
