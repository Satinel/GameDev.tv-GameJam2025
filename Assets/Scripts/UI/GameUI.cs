using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameObject _map;
    [SerializeField] TextMeshProUGUI _statsText, _moneyText;
    [SerializeField] TextMeshProUGUI _leftClawText, _leftAttack1Text, _leftAttack2Text;
    [SerializeField] TextMeshProUGUI _tailText, _tailAttack1Text, _tailAttack2Text;
    [SerializeField] TextMeshProUGUI _rightClawText, _rightAttack1Text, _rightAttack2Text;

    PlayerStats _playerStats;
    PlayerInventory _playerInventory;

    void Awake()
    {
        _playerStats = FindFirstObjectByType<PlayerStats>();
    }

    void OnEnable()
    {
        PlayerStats.OnStatIncreased += PlayerStats_OnStatIncreased;
        PlayerStats.OnMoneyChanged += PlayerStats_OnMoneyChanged;
        PlayerInventory.OnWeaponEquipped += PlayerInventory_OnWeaponEquipped;
    }

    void OnDisable()
    {
        PlayerStats.OnStatIncreased -= PlayerStats_OnStatIncreased;
        PlayerStats.OnMoneyChanged -= PlayerStats_OnMoneyChanged;
        PlayerInventory.OnWeaponEquipped -= PlayerInventory_OnWeaponEquipped;
    }

    void Start()
    {
        _playerInventory = _playerStats.GetComponent<PlayerInventory>();
        SetStatsText();
        SetAttackButtonTexts();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    void SetStatsText()
    {
        _statsText.text = $"<size=200%>-<u>Stats</u>-</size>\n";
        _statsText.text += $"\nStrength {_playerStats.Strength}\nAccuracy {_playerStats.Accuracy}\nFortitude {_playerStats.Fortitude}";
        _statsText.text += $"\nEvasion {_playerStats.Evasion}\nTenacity {_playerStats.Tenacity}\nInitiative {_playerStats.Initiative}";
    }

    void SetAttackButtonTexts()
    {
        _leftClawText.text = _playerInventory.LeftClawWeapon.Name;
        _leftAttack1Text.text = _playerInventory.LeftClawWeapon.Ability1.Name;
        _leftAttack2Text.text = _playerInventory.LeftClawWeapon.Ability2.Name;

        _tailText.text = _playerInventory.TailWeapon.Name;
        _tailAttack1Text.text = _playerInventory.TailWeapon.Ability1.Name;
        _tailAttack2Text.text = _playerInventory.TailWeapon.Ability2.Name;

        _rightClawText.text = _playerInventory.RightClawWeapon.Name;
        _rightAttack1Text.text = _playerInventory.RightClawWeapon.Ability1.Name;
        _rightAttack2Text.text = _playerInventory.RightClawWeapon.Ability2.Name;
    }

    void ToggleMap()
    {
        _map.SetActive(!_map.activeSelf);
    }

    void PlayerStats_OnStatIncreased(PlayerStats.Stats _, int __)
    {
        SetStatsText();
    }

    void PlayerStats_OnMoneyChanged(int totalMoney)
    {
        _moneyText.text = $"BugBucks {totalMoney.FormatLargeNumbers()}";
    }

    void PlayerInventory_OnWeaponEquipped(Equipable weapon, bool isLeft)
    {
        if(isLeft)
        {
            _leftClawText.text = weapon.Name;
            _leftAttack1Text.text = weapon.Ability1.Name;
            _leftAttack2Text.text = weapon.Ability2.Name;
        }
        else
        {
            _rightClawText.text = weapon.Name;
            _rightAttack1Text.text = weapon.Ability1.Name;
            _rightAttack2Text.text = weapon.Ability2.Name;
        }
    }
}
