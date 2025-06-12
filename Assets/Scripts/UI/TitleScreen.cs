using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public static event Action OnMusicStarted;
    public static event Action OnCreepComplete;

    [SerializeField] int _sceneIndex = 1;
    [SerializeField] GameObject _startButton, _continueButton, _newPlusButton;
    [SerializeField] GameObject _optionsPrefab, _musicPlayer;
    [SerializeField] Animator _animator;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _punch1SFX, _punch2SFX, _tailSFX;

    bool _isLoading, _noTrigger;

    void Start()
    {
        OptionsMenu.OnOptionsClosed += OptionsMenu_OnOptionsClosed;
        SaveSystem.OnAutoSaveFound += SaveSystem_OnAutoSaveFound;
        SaveSystem.OnSaveDataFound += SaveSystem_OnSaveDataFound;
        SaveSystem.OnLoadStarted += SaveSystem_OnLoadStarted;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_startButton);
    }

    void OnDestroy()
    {
        OptionsMenu.OnOptionsClosed -= OptionsMenu_OnOptionsClosed;
        SaveSystem.OnAutoSaveFound -= SaveSystem_OnAutoSaveFound;
        SaveSystem.OnSaveDataFound -= SaveSystem_OnSaveDataFound;
        SaveSystem.OnLoadStarted -= SaveSystem_OnLoadStarted;
    }

    void OptionsMenu_OnOptionsClosed()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if(_continueButton.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(_continueButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(_startButton);
        }
    }

    void SaveSystem_OnAutoSaveFound()
    {
        _newPlusButton.SetActive(true);
    }

    void SaveSystem_OnSaveDataFound()
    {
        _continueButton.SetActive(true);
    }

    void SaveSystem_OnLoadStarted()
    {
        if(_isLoading) { return; }

        _optionsPrefab.SetActive(false);
        _isLoading = true;
        _noTrigger = true;
        _animator.SetTrigger("Load");
    }

    public void LoadGameScene()
    {
        if(_isLoading) { return; }

        _optionsPrefab.SetActive(false);
        _isLoading = true;
        _noTrigger = false;
        _animator.SetTrigger("Load");
    }

    public void CreepComplete() // Animation Trigger
    {
        if(_noTrigger)
        {
            OnCreepComplete?.Invoke();
            return;
        }
        else
        {
            SceneManager.LoadScene(_sceneIndex);
        }
    }

    public void PlayPunch1() // Animation Trigger
    {
        _audioSource.PlayOneShot(_punch1SFX);
    }

    public void PlayPunch2() // Animation Trigger
    {
        _audioSource.PlayOneShot(_punch2SFX);
    }

    public void PlayTail() // Animation Trigger
    {
        _audioSource.PlayOneShot(_tailSFX);
    }

    public void StartMusic()
    {
        _musicPlayer.SetActive(true);
        OnMusicStarted?.Invoke();
    }
}
