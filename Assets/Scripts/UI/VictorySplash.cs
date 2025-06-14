using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class VictorySplash : MonoBehaviour
{
    [SerializeField] GameObject _window, _mainMenuButton, _newMazeButton;
    bool _isLoading;
    PlayerController _playerController;

    void Start()
    {
        Enemy.OnEnemyKilled += Enemy_OnEnemyKilled;
        OptionsMenu.OnOptionsClosed += MenuMenu_OnOptionsClosed;
    }

    void OnDestroy()
    {
        Enemy.OnEnemyKilled -= Enemy_OnEnemyKilled;
        OptionsMenu.OnOptionsClosed -= MenuMenu_OnOptionsClosed;
    }

    // void Update()
    // {
    //     if(!_victory) { return; }
    //     if(_isLoading) { return; }

    //     if(EventSystem.current != _mainMenuButton || EventSystem.current != _newMazeButton)
    //     {
    //         EventSystem.current.SetSelectedGameObject(null);
    //         EventSystem.current.SetSelectedGameObject(_mainMenuButton);
    //     }
    // }

    void Enemy_OnEnemyKilled(Enemy enemy)
    {
        if(!enemy.IsBoss) { return; }

        _window.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_newMazeButton);
    }

    void MenuMenu_OnOptionsClosed()
    {
        if(_window.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_newMazeButton);
        }
    }

    public void LoadMainMenu()
    {
        if(_isLoading) { return; }

        _isLoading = true;
        _mainMenuButton.SetActive(false);
        _newMazeButton.SetActive(false);
        _playerController = FindFirstObjectByType<PlayerController>();
        _playerController.transform.SetParent(transform); // This is a pretty clean way to remove DontDestroyOnLoad
        SceneManager.LoadScene(0);
    }

    public void LoadNewMaze()
    {
        if(_isLoading) { return; }

        _isLoading = true;
        FindFirstObjectByType<PlayerHealth>().IncreaseDungeonFloor();
        FindFirstObjectByType<PlayerInventory>().LoseKey();
        _mainMenuButton.SetActive(false);
        _newMazeButton.SetActive(false);
        SceneManager.LoadScene(2);
    }
}
