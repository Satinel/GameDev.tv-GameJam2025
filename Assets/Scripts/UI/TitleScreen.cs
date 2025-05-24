using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] GameObject _startButton;
    [SerializeField] GameObject _buttonsParent, _optionsPrefab;
    [SerializeField] Animator _animator;
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

        _buttonsParent.SetActive(false);
        _optionsPrefab.SetActive(false);
        _isLoading = true;
        _animator.SetTrigger("Load");
    }

    public void CreepComplete() // Animation Trigger
    {
        SceneManager.LoadScene(1);
    }
}
