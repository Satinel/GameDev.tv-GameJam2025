using System;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static event Action OnKeyClaimed;

    [SerializeField] GameObject _mapIcon;

    Vector2 _coordinates = new();

    bool _isRevealed, _keyClaimed;

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
        if(_keyClaimed) { return; }

        if(!other.gameObject.TryGetComponent(out PlayerInventory playerInventory)) { return; }

        _keyClaimed = true;
        playerInventory.GetKey();
        OnKeyClaimed?.Invoke();
        _mapIcon.SetActive(false);
        // TODO Play a sound
        // TODO Show a real message
        Debug.Log("Key Claimed!");
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
