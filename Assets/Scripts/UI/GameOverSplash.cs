using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameOverSplash : MonoBehaviour
{
    public static event Action OnRespawn;

    [SerializeField] GameObject _respawnButton, _mainMenuButton; // Note: Setting active buttons is handled in PlayerCombat
    [SerializeField] Animator _animator;

    bool _isLoading;

    PlayerController _playerController;

    public void Respawn()
    {
        OnRespawn?.Invoke();
        gameObject.SetActive(false);
    }

    public void LoadMainMenu()
    {
        if(_isLoading) { return; }

        _playerController = FindFirstObjectByType<PlayerController>();

        _respawnButton.SetActive(false);
        _mainMenuButton.SetActive(false);
        _isLoading = true;
        _animator.SetTrigger("Load");
    }

    public void CreepComplete() // Animation Trigger
    {
        _playerController.transform.SetParent(transform); // This is a pretty clean way to remove DontDestroyOnLoad
        SceneManager.LoadScene(0);
    }
}
