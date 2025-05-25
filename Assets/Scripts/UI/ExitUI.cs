using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ExitUI : MonoBehaviour
{
    public static event Action OnExitResolved;

    [SerializeField] GameObject _exitWindow, _exitButton;
    [SerializeField] Animator _animator;

    bool _isLoading;

    void Start()
    {
        Exit.OnExitEntered += Exit_OnExitEntered;
        BossEncounter.OnBossDefeated += BossEncounter_OnBossDefeated;
    }

    void OnDestroy()
    {
        Exit.OnExitEntered -= Exit_OnExitEntered;
        BossEncounter.OnBossDefeated += BossEncounter_OnBossDefeated;
    }

    void Exit_OnExitEntered(Transform t)
    {
        _exitWindow.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_exitButton);
    }

    void BossEncounter_OnBossDefeated()
    {
        _exitWindow.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_exitButton);
    }

    public void LoadNextMaze() // UI Button
    {
        if(_isLoading) { return; }

        _exitButton.SetActive(false);
        _isLoading = true;
        _animator.SetTrigger("Load");
    }

    public void CreepComplete() // Animation Trigger
    {
        OnExitResolved?.Invoke();
        _exitWindow.SetActive(false);
        // TODO (Not really) set an int in player to track number of mazes cleared and use that number to add to stats of every enemy in the next version of maze
        SceneManager.LoadScene(2);
    }

    public void CloseWindow() // UI Button
    {
        OnExitResolved?.Invoke();
        _exitWindow.SetActive(false);
    }
}
