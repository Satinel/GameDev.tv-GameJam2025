using UnityEngine;
using System.Collections.Generic;

public class RandomEncounter : MonoBehaviour
{
    [SerializeField] List<Enemy> _enemies = new();
    [SerializeField] protected GameObject _mapIcon;

    Vector2 _coordinates = new();
    Enemy _enemy;

    void OnEnable()
    {
        _enemy = Instantiate(_enemies[Random.Range(0, _enemies.Count)], transform);
        _enemy.transform.Rotate(0, Random.Range(0f, 359f), 0);
    }

    void Start()
    {
        MazeGenerator.OnMazeUnitRevealed += MazeGenerator_OnMazeUnitRevealed;
    }

    void OnDestroy()
    {
        MazeGenerator.OnMazeUnitRevealed -= MazeGenerator_OnMazeUnitRevealed;
    }

    void OnTriggerEnter(Collider other)
    {
        if(!_enemy) { return; }

        if(other.gameObject.GetComponentInParent<PlayerHealth>())
        {
            Debug.Log("Start Battle!");
            Vector3 lookAtTarget = new(other.transform.position.x, _enemy.transform.position.y, other.transform.position.z);
            _enemy.transform.LookAt(lookAtTarget);
            _enemy.SetInBattle(true);
        }
    }

    public void EnemyDead()
    {
        Debug.Log("End Battle!");
        if(_enemy)
        {
            _enemy.SetInBattle(false);
        }
        gameObject.SetActive(false);
    }

    public void SetCoordinates(int x, int z)
    {
        _coordinates = new(x, z);
    }

    void MazeGenerator_OnMazeUnitRevealed(Vector2 coordinates)
    {
        if(!gameObject.activeSelf) { return; }

        if(coordinates != _coordinates) { return; }

        _mapIcon.SetActive(true);
    }
}
