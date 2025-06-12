using UnityEngine;
using System;

public class RestArea : MonoBehaviour
{
    public static event Action<Transform> OnRestAreaEntered;
    [SerializeField] Transform _emptyTransform;

    [SerializeField] GameObject _mapIcon;
    bool _isRevealed;
    Vector2 _coordinates = new();
    [SerializeField] Collider _collider;

    void Start()
    {
        MazeGenerator.OnMazeUnitRevealed += MazeGenerator_OnMazeUnitRevealed;
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        Store.OnEnteredStore += Store_OnEnteredStore;
    }

    void OnDestroy()
    {
        MazeGenerator.OnMazeUnitRevealed -= MazeGenerator_OnMazeUnitRevealed;
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        Store.OnEnteredStore -= Store_OnEnteredStore;
    }

    void MazeGenerator_OnMazeUnitRevealed(Vector2 coordinates)
    {
        if(_isRevealed) { return; }
        if(coordinates != _coordinates) { return; }

        Reveal();
    }

    void Enemy_OnFightStarted(Enemy _)
    {
        _collider.enabled = true;
    }

    void Store_OnEnteredStore(Transform _)
    {
        _collider.enabled = true;
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
            OnRestAreaEntered?.Invoke(_emptyTransform);
        }
    }

    public void SetCoordinates(int x, int z)
    {
        _coordinates = new(x, z);
    }

    public void DeactivateCollider()
    {
        _collider.enabled = false;
    }
}
