using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] protected GameObject _mapIcon;

    protected Vector2 _coordinates = new();

    bool _isRevealed;

    void OnEnable()
    {
        MazeGenerator.OnMazeUnitRevealed += MazeGenerator_OnMazeUnitRevealed;
    }

    void OnDisable()
    {
        MazeGenerator.OnMazeUnitRevealed -= MazeGenerator_OnMazeUnitRevealed;
    }

    public void SetCoordinates(int x, int z)
    {
        _coordinates = new(x, z);
    }

    void MazeGenerator_OnMazeUnitRevealed(Vector2 coordinates)
    {
        if(_isRevealed) { return; }
        if(coordinates != _coordinates) { return; }

        _mapIcon.SetActive(true);
        _isRevealed = true;
    }
}
