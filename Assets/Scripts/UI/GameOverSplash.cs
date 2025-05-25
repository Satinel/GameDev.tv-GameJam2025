using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSplash : MonoBehaviour
{
    [SerializeField] GameObject _button;
    [SerializeField] Animator _animator;

    bool _isLoading;

    PlayerController _playerController;

    public void LoadMainMenu()
    {
        if(_isLoading) { return; }

        _playerController = FindFirstObjectByType<PlayerController>();

        _button.SetActive(false);
        _isLoading = true;
        _animator.SetTrigger("Load");
    }

    public void CreepComplete() // Animation Trigger
    {
        _playerController.transform.SetParent(transform); // This is a pretty clean way to remove DontDestroyOnLoad
        SceneManager.LoadScene(0);
    }
}
