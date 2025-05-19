using UnityEngine;
using System.Collections.Generic;

public class RandomEncounter : MonoBehaviour
{
    [SerializeField] List<Enemy> _enemies = new();
    [SerializeField] protected GameObject _mapIcon;

    bool _isRevealed, _wasTriggered;
    Vector2 _coordinates = new();
    Enemy _enemy;

    void OnEnable()
    {
        _enemy = Instantiate(_enemies[Random.Range(0, _enemies.Count)], transform);
        _enemy.transform.Rotate(0, Random.Range(0f, 359f), 0);
        _wasTriggered = false;
    }

    void Start()
    {
        Enemy.OnEnemyKilled += Enemy_OnAnyEnemyKilled;
        MazeGenerator.OnMazeUnitRevealed += MazeGenerator_OnMazeUnitRevealed;
    }

    void OnDestroy()
    {
        Enemy.OnEnemyKilled -= Enemy_OnAnyEnemyKilled;
        MazeGenerator.OnMazeUnitRevealed -= MazeGenerator_OnMazeUnitRevealed;
    }

    void OnTriggerEnter(Collider other)
    {
        if(_wasTriggered) { return; }
        if(!_enemy) { return; }

        if(other.gameObject.GetComponent<PlayerHealth>())
        {
            Vector3 lookAtTarget = new(other.transform.position.x, _enemy.transform.position.y, other.transform.position.z);
            _enemy.transform.LookAt(lookAtTarget);
            _enemy.StartBattle();
            _wasTriggered = true;
        }
    }

    public void SetCoordinates(int x, int z)
    {
        _coordinates = new(x, z);
    }

    void Enemy_OnAnyEnemyKilled(Enemy enemy)
    {
        if(enemy == _enemy)
        {
            _enemy.EndBattle();
            gameObject.SetActive(false);
        }
    }

    void MazeGenerator_OnMazeUnitRevealed(Vector2 coordinates)
    {
        if(!gameObject.activeSelf) { return; }
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
