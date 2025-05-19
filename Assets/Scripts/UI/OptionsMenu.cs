using UnityEngine;
using UnityEngine.EventSystems;
// using UnityEngine.SceneManagement;
using System;

public class OptionsMenu : MonoBehaviour
{
    public static event Action OnOptionsOpened;
    public static event Action OnOptionsClosed;

    [SerializeField] Canvas _optionsCanvas, _audioCanvas;
    [SerializeField] GameObject _closeMenuButton;
    [SerializeField] GameObject _quitPrompt;
    [SerializeField] GameObject _cancelQuitButton, _quitPromptButton, _audioButton;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleOptions();
        }
    }

    void ToggleOptions()
    {
        if(_optionsCanvas.enabled)
        {
            if(_quitPrompt.activeSelf)
            {
                CancelQuit();
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(_quitPromptButton);
            }
            else
            {
                DisableOptionsCanvas();
            }
        }
        else if(_audioCanvas.gameObject.activeSelf)
        {
            DisableAudioCanvas();
            EventSystem.current.SetSelectedGameObject(_audioButton);
        }
        else
        {
            EnableOptionsCanvas();
        }
    }

    public void EnableOptionsCanvas()
    {
        _optionsCanvas.enabled = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_closeMenuButton);
        OnOptionsOpened?.Invoke();
    }

    public void DisableOptionsCanvas()
    {
        _optionsCanvas.enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
        OnOptionsClosed?.Invoke();
    }

    public void EnableAudioCanvas()
    {
        _optionsCanvas.enabled = false;
        _audioCanvas.gameObject.SetActive(true);
    }

    public void DisableAudioCanvas()
    {
        _audioCanvas.gameObject.SetActive(false);
        _optionsCanvas.enabled = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_audioButton);
    }

    // public void ReloadScene()
    // {
    //     Time.timeScale = 1;
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    // }

    public void PromptQuit()
    {
        _quitPrompt.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_cancelQuitButton);
    }

    public void CancelQuit()
    {
        _quitPrompt.SetActive(false);
        EnableOptionsCanvas();
    }

    public void ConfirmQuit()
    {
        Application.Quit();
    }
}
