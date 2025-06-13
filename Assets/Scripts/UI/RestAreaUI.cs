using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class RestAreaUI : MonoBehaviour
{
    // public static event Action OnRestUIActivated;
    public static event Action OnRestAreaResolved;
    public static event Action OnRestAreaUsed; // TODO Repopulate map with enemies if you want
    public static event Action OnSaveConfirmed;
    public static event Action OnSavePrompted;

    [SerializeField] GameObject _restWindow, _exitButton;
    [SerializeField] GameObject _saveMenu;
    [SerializeField] GameObject _saveButton;

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
        // OnRestUIActivated?.Invoke();
    }

    void OptionsMenu_OnOptionsClosed()
    {
        if(_restWindow.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_exitButton);
        }
        if(_saveMenu.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_saveButton);
        }
    }

    public void UseRestArea()
    {
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        playerHealth.transform.position = _spawnPoint;
        playerHealth.SetSpawnPoint(_spawnPoint);
        OnRestAreaUsed?.Invoke();
        CloseWindow();
    }

    public void CloseWindow() // UI Button
    {
        _restWindow.SetActive(false);
        OpenSaveMenu();
    }

    void OpenSaveMenu()
    {
        _saveMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_saveButton);
    }

    public void ConfirmSave() // UI Button
    {
        OnSaveConfirmed?.Invoke();
        CloseSaveMenu();
    }

    public void CancelSave() // UI Button
    {
        CloseSaveMenu();
    }

    void CloseSaveMenu()
    {
        _saveMenu.SetActive(false);
        OnRestAreaResolved?.Invoke();
    }

    public void PromptSave()
    {
        OpenSaveMenu();
        OnSavePrompted?.Invoke();
    }
}
