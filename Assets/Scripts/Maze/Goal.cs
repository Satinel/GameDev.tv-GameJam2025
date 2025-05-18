using System;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static event Action OnKeyClaimed;

    [SerializeField] GameObject _mapIcon;

    Vector2 _coordinates = new();

    bool _isRevealed;

    void OnEnable()
    {
        MazeGenerator.OnMazeUnitRevealed += MazeGenerator_OnMazeUnitRevealed;
    }

    void OnDisable()
    {
        MazeGenerator.OnMazeUnitRevealed -= MazeGenerator_OnMazeUnitRevealed;
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.TryGetComponent(out PlayerInventory playerInventory)) { return; }

        playerInventory.GetKey();
        OnKeyClaimed?.Invoke();
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
    }
}
