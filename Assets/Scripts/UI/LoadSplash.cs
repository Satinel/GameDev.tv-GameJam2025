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
        MazeGenerator.OnMazeReady += Uncover;
        PlayerHealth.OnPlayerDeath += Cover;
        GameOverSplash.OnRespawn += Uncover;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        MazeGenerator.OnMazeReady -= Uncover;
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

    void Cover()
    {
        _animator.SetTrigger(COVER);
    }

    void Uncover()
    {
        _animator.SetTrigger(UNCOVER);
    }
}
