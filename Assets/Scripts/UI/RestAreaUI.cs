using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class RestAreaUI : MonoBehaviour
{
    public static event Action OnRestAreaResolved;
    public static event Action OnRestAreaUsed; // TODO Repopulate map with enemies if you want

    [SerializeField] GameObject _restWindow, _exitButton;

    void Start()
    {
        RestArea.OnRestAreaEntered += RestArea_OnRestAreaEntered;
    }

    void OnDestroy()
    {
        RestArea.OnRestAreaEntered -= RestArea_OnRestAreaEntered;
    }

    void RestArea_OnRestAreaEntered(Transform t)
    {
        _restWindow.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_exitButton);
    }

    public void UseRestArea()
    {
        OnRestAreaUsed?.Invoke();
        CloseWindow();
    }

    public void CloseWindow() // UI Button
    {
        OnRestAreaResolved?.Invoke();
        _restWindow.SetActive(false);
    }
}
