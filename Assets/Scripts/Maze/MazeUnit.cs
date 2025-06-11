using UnityEngine;

public class MazeUnit : MonoBehaviour
{
    [SerializeField] protected GameObject _mapIcon;

    protected Vector2 _coordinates = new();

    bool _isRevealed;
    public byte IsWall { get; private set; }
    public Vector2 Coordinates => _coordinates;

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

        Reveal();
    }

    public void Reveal()
    {
        _mapIcon.SetActive(true);
        _isRevealed = true;
        IsWall += 2;
    }

    public void SetIsWall(byte wall)
    {
        IsWall = wall;
    }
}
