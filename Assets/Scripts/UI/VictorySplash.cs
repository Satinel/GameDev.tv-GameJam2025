using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class VictorySplash : MonoBehaviour
{
    [SerializeField] GameObject _window, _mainMenuButton, _newMazeButton;
    bool _victory;
    PlayerController _playerController;

    void Start()
    {
        Enemy.OnEnemyKilled += Enemy_OnEnemyKilled;
    }

    void OnDestroy()
    {
        Enemy.OnEnemyKilled -= Enemy_OnEnemyKilled;
    }

    void Update()
    {
        if(!_victory) { return; }

        if(EventSystem.current != _mainMenuButton)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_mainMenuButton);
        }
    }

    void Enemy_OnEnemyKilled(Enemy enemy)
    {
        if(!enemy.IsBoss) { return; }

        _window.SetActive(true);
        _victory = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_mainMenuButton);
    }

    public void LoadMainMenu()
    {
        _mainMenuButton.SetActive(false);
        _newMazeButton.SetActive(false);
        _playerController = FindFirstObjectByType<PlayerController>();
        _playerController.transform.SetParent(transform); // This is a pretty clean way to remove DontDestroyOnLoad
        SceneManager.LoadScene(0);
    }

    public void LoadNewMaze()
    {
        FindFirstObjectByType<PlayerHealth>().IncreaseDungeonFloor();
        FindFirstObjectByType<PlayerInventory>().LoseKey();
        _mainMenuButton.SetActive(false);
        _newMazeButton.SetActive(false);
        SceneManager.LoadScene(2);
    }
}
