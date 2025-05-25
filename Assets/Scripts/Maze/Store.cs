using UnityEngine;
using System;

public class Store : MonoBehaviour
{
    public static event Action<Transform> OnEnteredStore;

    [SerializeField] Transform _tigey;

    [SerializeField] GameObject _mapIcon;
    bool _isRevealed;
    Vector2 _coordinates = new();

    void Start()
    {
        MazeGenerator.OnMazeUnitRevealed += MazeGenerator_OnMazeUnitRevealed;
    }

    void OnDestroy()
    {
        MazeGenerator.OnMazeUnitRevealed -= MazeGenerator_OnMazeUnitRevealed;
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
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerHealth>())
        {
            OnEnteredStore?.Invoke(_tigey);
        }
    }

    public void SetCoordinates(int x, int z)
    {
        _coordinates = new(x, z);
    }
}
