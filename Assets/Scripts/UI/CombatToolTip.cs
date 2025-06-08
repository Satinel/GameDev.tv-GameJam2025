using UnityEngine;
using TMPro;

public class CombatToolTip : MonoBehaviour
{
    [SerializeField] GameObject _toolTip;
    [SerializeField] TextMeshProUGUI _tipText;

    PlayerStats _playerStats;
    Enemy _currentEnemy;

    void Awake()
    {
        _playerStats = FindFirstObjectByType<PlayerStats>();
    }

    void Start()
    {
        PlayerInventory.OnAbilityFocused += PlayerInventory_OnAbilityFocused;
        PlayerInventory.OnAbilityNotFocused += PlayerInventory_OnAbilityNotFocused;
        Enemy.OnFightStarted += Enemy_OnFightStarted;
    }

    void OnDestroy()
    {
        PlayerInventory.OnAbilityFocused -= PlayerInventory_OnAbilityFocused;
        PlayerInventory.OnAbilityNotFocused -= PlayerInventory_OnAbilityNotFocused;
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
    }

    void PlayerInventory_OnAbilityFocused(PlayerAbility ability)
    {
        if(_currentEnemy)
        {
            if(ability.AlwaysHits)
            {
                _tipText.text = $"[100% Chance] ";
            }
            else
            {
                _tipText.text = $"[{ability.HitChance + _playerStats.CurrentAccuracy - _currentEnemy.Evasion}% Chance] ";
            }
            if(ability.DealsDamage)
            {
                _tipText.text += $"[{Mathf.Max(ability.Damage + _playerStats.CurrentStrength - _currentEnemy.Fortitude)} Damage] ";
            }
            _tipText.text += ability.Description;
        }
        else
        {
            _tipText.text = $"[{ability.HitChance + _playerStats.CurrentAccuracy}% Chance] ";
            if(ability.DealsDamage)
            {
                _tipText.text += $"[{Mathf.Max(ability.Damage + _playerStats.CurrentStrength)} Damage] ";
            }
            _tipText.text += ability.Description;
        }
        _toolTip.SetActive(true);
    }

    void PlayerInventory_OnAbilityNotFocused()
    {
        _toolTip.SetActive(false);
        _tipText.text = string.Empty;
    }

    void Enemy_OnFightStarted(Enemy enemy)
    {
        _currentEnemy = enemy;
    }
}
