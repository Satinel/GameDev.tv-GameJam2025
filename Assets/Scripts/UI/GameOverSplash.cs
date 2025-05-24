using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSplash : MonoBehaviour
{
    [SerializeField] GameObject _button;
    [SerializeField] Animator _animator;
    [SerializeField] float _overlaySpeed;
    bool _isLoading;

    public void LoadMainMenu()
    {
        if(_isLoading) { return; }

        _button.SetActive(false);
        _isLoading = true;
        _animator.SetTrigger("Load");
    }

    public void CreepComplete() // Animation Trigger
    {
        SceneManager.LoadScene(0);
    }
}
