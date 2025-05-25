using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class LevelUpWindow : MonoBehaviour
{
    public static event Action<PlayerStats.Stats, int> OnLevelStatPicked;

    [SerializeField] GameObject _toggleWindow;
    [SerializeField] GameObject _randomButton;
    [SerializeField] TextMeshProUGUI _levelText;

    void Start()
    {
        PlayerStats.OnLevelUp += PlayerStats_OnLevelUp;
        CloseLevelWindow();
    }

    void OnDestroy()
    {
        PlayerStats.OnLevelUp -= PlayerStats_OnLevelUp;
    }

    void PlayerStats_OnLevelUp(int level)
    {
        _levelText.text = $"Reached Level {level}!";

        _toggleWindow.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_randomButton);
    }

    void CloseLevelWindow()
    {
        _toggleWindow.SetActive(false);
    }

    public void StrengthIncrease()
    {
        CloseLevelWindow();
        OnLevelStatPicked?.Invoke(PlayerStats.Stats.Strength, 1);
    }

    public void AccuracyIncrease()
    {
        CloseLevelWindow();
        OnLevelStatPicked?.Invoke(PlayerStats.Stats.Accuracy, 1);
    }

    public void FortitudeIncrease()
    {
        CloseLevelWindow();
        OnLevelStatPicked?.Invoke(PlayerStats.Stats.Fortitude, 1);
    }

    public void EvasionIncrease()
    {
        CloseLevelWindow();
        OnLevelStatPicked?.Invoke(PlayerStats.Stats.Evasion, 1);
    }

    public void TenacityIncrease()
    {
        CloseLevelWindow();
        OnLevelStatPicked?.Invoke(PlayerStats.Stats.Tenacity, 1);
    }

    public void InitiativeIncrease()
    {
        CloseLevelWindow();
        OnLevelStatPicked?.Invoke(PlayerStats.Stats.Initiative, 1);
    }

    public void RandomIncrease()
    {
        CloseLevelWindow();
        int stat = UnityEngine.Random.Range(1, 7);
        switch(stat)
        {
            case 1:
                OnLevelStatPicked?.Invoke(PlayerStats.Stats.Strength, 2);
                break;
            case 2:
                OnLevelStatPicked?.Invoke(PlayerStats.Stats.Accuracy, 2);
                break;
            case 3:
                OnLevelStatPicked?.Invoke(PlayerStats.Stats.Fortitude, 2);
                break;
            case 4:
                OnLevelStatPicked?.Invoke(PlayerStats.Stats.Evasion, 2);
                break;
            case 5:
                OnLevelStatPicked?.Invoke(PlayerStats.Stats.Tenacity, 2);
                break;
            case 6:
                OnLevelStatPicked?.Invoke(PlayerStats.Stats.Initiative, 2);
                break;
            default:
                OnLevelStatPicked?.Invoke(PlayerStats.Stats.Tenacity, 2);
                break;
        }
    }

}
