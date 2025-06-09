using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class RestAreaUI : MonoBehaviour
{
    public static event Action OnRestAreaResolved;
    public static event Action OnRestAreaUsed; // TODO Repopulate map with enemies if you want

    [SerializeField] GameObject _restWindow, _exitButton;

    Vector3 _spawnPoint;

    void Start()
    {
        RestArea.OnRestAreaEntered += RestArea_OnRestAreaEntered;
        OptionsMenu.OnOptionsClosed += OptionsMenu_OnOptionsClosed;
    }

    void OnDestroy()
    {
        RestArea.OnRestAreaEntered -= RestArea_OnRestAreaEntered;
        OptionsMenu.OnOptionsClosed -= OptionsMenu_OnOptionsClosed;
    }

    void RestArea_OnRestAreaEntered(Transform emptyTransform)
    {
        _spawnPoint = new(emptyTransform.position.x, 0, emptyTransform.position.z);
        _restWindow.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_exitButton);
    }

    void OptionsMenu_OnOptionsClosed()
    {
        if(!_restWindow.activeSelf) { return; }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_exitButton);
    }

    public void UseRestArea()
    {
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        playerHealth.transform.position = _spawnPoint;
        playerHealth.SetSpawnPoint();
        OnRestAreaUsed?.Invoke();
        CloseWindow();
    }

    public void CloseWindow() // UI Button
    {
        OnRestAreaResolved?.Invoke();
        _restWindow.SetActive(false);
    }
}
