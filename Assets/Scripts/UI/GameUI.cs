using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameObject _map, _statsWindow, _combatLogWindow;
    [SerializeField] GameObject _mapButton, _statsButton, _combatLogButton;
    [SerializeField] PlayerHealthSlider _playerHealthSlider;
    [SerializeField] TextMeshProUGUI _statsText, _moneyText;
    [SerializeField] TextMeshProUGUI _leftClawText, _leftAttack1Text, _leftAttack2Text;
    [SerializeField] TextMeshProUGUI _tailText, _tailAttack1Text, _tailAttack2Text;
    [SerializeField] TextMeshProUGUI _rightClawText, _rightAttack1Text, _rightAttack2Text;

    PlayerStats _playerStats;
    PlayerInventory _playerInventory;
    PlayerHealth _playerHealth;
    bool _isInCombat;

    void Awake()
    {
        _playerStats = FindFirstObjectByType<PlayerStats>();
    }

    void OnEnable()
    {
        PlayerStats.OnExperienceGained += PlayerStats_OnExperienceGained;
        PlayerStats.OnStatIncreased += PlayerStats_OnStatIncreased;
        PlayerStats.OnMoneyChanged += PlayerStats_OnMoneyChanged;
        PlayerStats.OnTempStatChange += PlayerStats_OnTempStatChange;
        PlayerInventory.OnWeaponEquipped += PlayerInventory_OnWeaponEquipped;
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        PlayerCombat.OnCombatResolved += PlayerCombat_OnCombatResolved;
        PlayerStats.OnTempStatsReset += PlayerStats_OnTempStatsReset;
    }

    void OnDisable()
    {
        PlayerStats.OnExperienceGained -= PlayerStats_OnExperienceGained;
        PlayerStats.OnStatIncreased -= PlayerStats_OnStatIncreased;
        PlayerStats.OnMoneyChanged -= PlayerStats_OnMoneyChanged;
        PlayerInventory.OnWeaponEquipped -= PlayerInventory_OnWeaponEquipped;
        PlayerStats.OnTempStatChange -= PlayerStats_OnTempStatChange;
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        PlayerCombat.OnCombatResolved -= PlayerCombat_OnCombatResolved;
        PlayerStats.OnTempStatsReset -= PlayerStats_OnTempStatsReset;
    }

    void Start()
    {
        _playerInventory = _playerStats.GetComponent<PlayerInventory>();
        _playerHealth = _playerStats.GetComponent<PlayerHealth>();
        SetStatsText();
        SetAttackButtonTexts();
        _statsWindow.SetActive(false);
        _combatLogWindow.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            ToggleStats();
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            ToggleLog();
        }
    }

    void SetStatsText()
    {
        _statsText.text = $"<size=200%>-<u>Stats</u>-</size>\n\nLevel {_playerStats.Level}\n";
        _statsText.text += $"\nStrength {_playerStats.CurrentStrength}\nAccuracy {_playerStats.CurrentAccuracy}\nFortitude {_playerStats.CurrentFortitude}";
        _statsText.text += $"\nEvasion {_playerStats.CurrentEvasion}\nTenacity {_playerStats.Tenacity}\nInitiative {_playerStats.Initiative}";
        _statsText.text += $"\n\nXP {_playerStats.CurrentXP.FormatLargeNumbers()}/{_playerStats.NextLevelXP.FormatLargeNumbers()}";
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

    public void ToggleMap()
    {
        if(_isInCombat) { return; }

        _map.SetActive(!_map.activeSelf);
    }

    public void ToggleStats()
    {
        if(_isInCombat) { return; }

        _statsWindow.SetActive(!_statsWindow.activeSelf);
        _playerHealthSlider.SetHealthValues(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
    }

    public void ToggleLog()
    {
        if(_isInCombat) { return; }

        _combatLogWindow.SetActive(!_combatLogWindow.activeSelf);
    }

    void PlayerStats_OnExperienceGained()
    {
        SetStatsText();
    }

    void PlayerStats_OnStatIncreased(PlayerStats.Stats _, int __)
    {
        SetStatsText();
    }

    void PlayerStats_OnTempStatChange(string _)
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

    void Enemy_OnFightStarted(Enemy _)
    {
        _mapButton.SetActive(false);
        _statsButton.SetActive(false);
        _combatLogButton.SetActive(false);
        _isInCombat = true;
        _playerHealthSlider.SetHealthValues(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
        _statsWindow.SetActive(true);
        _combatLogWindow.SetActive(true);
        _map.SetActive(false);
    }

    void PlayerCombat_OnCombatResolved()
    {
        _isInCombat = false;
        _combatLogWindow.SetActive(false);
        _mapButton.SetActive(true);
        _statsButton.SetActive(true);
        _combatLogButton.SetActive(true);
    }

    void PlayerStats_OnTempStatsReset()
    {
        SetStatsText();
    }
}
