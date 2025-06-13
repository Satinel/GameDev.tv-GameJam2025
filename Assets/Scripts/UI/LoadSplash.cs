using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSplash : MonoBehaviour
{
    [SerializeField] Animator _animator;

    static readonly int COVER = Animator.StringToHash("Cover");
    static readonly int UNCOVER = Animator.StringToHash("Uncover");

    void Awake()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SaveSystem.OnMazeLoaded += Uncover;
        RestArea.OnRestAreaEntered += RestArea_RestAreaEntered;
        RestAreaUI.OnRestAreaResolved += Uncover;
        PlayerHealth.OnPlayerDeath += Cover;
        GameOverSplash.OnRespawn += Uncover;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SaveSystem.OnMazeLoaded -= Uncover;
        RestArea.OnRestAreaEntered -= RestArea_RestAreaEntered;
        RestAreaUI.OnRestAreaResolved -= Uncover;
        PlayerHealth.OnPlayerDeath -= Cover;
        GameOverSplash.OnRespawn -= Uncover;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode _)
    {
        if(scene.buildIndex == 1)
        {
            Uncover();
        }
    }

    void RestArea_RestAreaEntered(Transform _)
    {
        Cover();
    }

    void Cover()
    {
        _animator.SetTrigger(COVER);
    }

    void Uncover()
    {
        _animator.SetTrigger(UNCOVER);
    }
}
