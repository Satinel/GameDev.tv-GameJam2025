using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpWindow : MonoBehaviour
{
    public static event Action<PlayerStats.Stats, int> OnLevelStatPicked;

    [SerializeField] GameObject _levelUpWindow;
    [SerializeField] GameObject _randomButton;

    void Start()
    {
        PlayerStats.OnLevelUp += PlayerStats_OnLevelUp;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_randomButton);
    }

    void OnDestroy()
    {
        PlayerStats.OnLevelUp -= PlayerStats_OnLevelUp;
    }

    void PlayerStats_OnLevelUp()
    {
        _levelUpWindow.SetActive(true);
    }

    public void CloseLevelWindow()
    {
        _levelUpWindow.SetActive(false);
    }

    public void StrengthIncrease()
    {
        OnLevelStatPicked?.Invoke(PlayerStats.Stats.Strength, 1);
    }

    public void AccuracyIncrease()
    {
        OnLevelStatPicked?.Invoke(PlayerStats.Stats.Accuracy, 1);
    }

    public void FortitudeIncrease()
    {
        OnLevelStatPicked?.Invoke(PlayerStats.Stats.Fortitude, 1);
    }

    public void EvasionIncrease()
    {
        OnLevelStatPicked?.Invoke(PlayerStats.Stats.Evasion, 1);
    }

    public void TenacityIncrease()
    {
        OnLevelStatPicked?.Invoke(PlayerStats.Stats.Tenacity, 1);
    }

    public void InitiativeIncrease()
    {
        OnLevelStatPicked?.Invoke(PlayerStats.Stats.Initiative, 1);
    }

    public void RandomIncrease()
    {
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
