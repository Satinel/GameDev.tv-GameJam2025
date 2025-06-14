using UnityEngine;
using TMPro;

public class SaveUI : MonoBehaviour
{
    [SerializeField] GameObject _background;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] float _closeDelay = 0.75f;

    void Awake()
    {
        SaveSystem.OnSaveStarted += SaveSystem_OnSaveStarted;
        SaveSystem.OnSaveFailed += SaveSystem_OnSaveFailed;
        SaveSystem.OnSaveCompleted += SaveSystem_OnSaveCompleted;
    }

    void OnDestroy()
    {
        SaveSystem.OnSaveStarted += SaveSystem_OnSaveStarted;
        SaveSystem.OnSaveFailed += SaveSystem_OnSaveFailed;
        SaveSystem.OnSaveCompleted += SaveSystem_OnSaveCompleted;
    }

    void SaveSystem_OnSaveStarted()
    {
        _text.text = "Saving...";
        _background.SetActive(true);
    }

    void SaveSystem_OnSaveFailed(string error)
    {
        _text.text = error;
        Invoke(nameof(CloseWindow), _closeDelay);
    }

    void SaveSystem_OnSaveCompleted()
    {
        _text.text = "Save Complete!";
        Invoke(nameof(CloseWindow), _closeDelay);
    }

    void CloseWindow()
    {
        _background.SetActive(false);
    }
}
