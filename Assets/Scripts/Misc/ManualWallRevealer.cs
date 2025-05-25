using UnityEngine;

public class ManualWallRevealer : MonoBehaviour
{
    bool _isRevealed;
    MazeUnit _mazeUnit;
    [SerializeField] GameObject _extraIcon;

    void Awake()
    {
        _mazeUnit = GetComponent<MazeUnit>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(_isRevealed) { return; }

        if(other.gameObject.GetComponentInParent<PlayerHealth>())
        {
            _isRevealed = true;
            _mazeUnit.Reveal();
            if(_extraIcon)
            {
                _extraIcon.SetActive(true);
            }
        }
    }
}
