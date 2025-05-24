using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] GameObject _startButton;
    [SerializeField] GameObject _optionsPrefab, _musicPlayer;
    [SerializeField] Animator _animator;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _punch1SFX, _punch2SFX, _tailSFX;

    bool _isLoading;

    void Start()
    {
        OptionsMenu.OnOptionsClosed += OptionsMenu_OnOptionsClosed;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_startButton);
    }

    void OnDestroy()
    {
        OptionsMenu.OnOptionsClosed -= OptionsMenu_OnOptionsClosed;
    }

    void OptionsMenu_OnOptionsClosed()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_startButton);
    }

    public void LoadGameScene()
    {
        if(_isLoading) { return; }

        _optionsPrefab.SetActive(false);
        _isLoading = true;
        _animator.SetTrigger("Load");
    }

    public void CreepComplete() // Animation Trigger
    {
        SceneManager.LoadScene(1);
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
    }
}
