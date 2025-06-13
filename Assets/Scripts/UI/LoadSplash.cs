using UnityEngine;

public class LoadSplash : MonoBehaviour
{
    [SerializeField] Animator _animator;

    static readonly int COVER = Animator.StringToHash("Cover");
    static readonly int UNCOVER = Animator.StringToHash("Uncover");

    void Awake()
    {
        SaveSystem.OnMazeLoaded += Uncover;
        RestArea.OnRestAreaEntered += RestArea_RestAreaEntered;
        RestAreaUI.OnRestAreaResolved += Uncover;
    }

    void OnDestroy()
    {
        SaveSystem.OnMazeLoaded -= Uncover;
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
