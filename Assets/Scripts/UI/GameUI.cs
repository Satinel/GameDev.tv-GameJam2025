using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameObject _map;
    [SerializeField] TextMeshProUGUI _statsText, _moneyText;

    PlayerStats _playerStats;

    void Awake()
    {
        _playerStats = FindFirstObjectByType<PlayerStats>();
    }

    void OnEnable()
    {
        PlayerStats.OnStatIncreased += PlayerStats_OnStatIncreased;
        PlayerStats.OnMoneyChanged += PlayerStats_OnMoneyChanged;
    }

    void OnDisable()
    {
        PlayerStats.OnStatIncreased -= PlayerStats_OnStatIncreased;
        PlayerStats.OnMoneyChanged -= PlayerStats_OnMoneyChanged;
    }

    void Start()
    {
        SetStatsText();
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
}
