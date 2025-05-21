using UnityEngine;
using System;
using System.Collections.Generic;

public class DeadEnd : MonoBehaviour
{
    public static event Action OnAnyDeadEndEvent;

    [SerializeField] List<Enemy> _enemies = new(); // TODO create stronger enemies for these special fights
    [SerializeField] GameObject _mapIcon;

    bool _isRevealed;
    Vector2 _coordinates = new();
    Enemy _enemy;

    void OnEnable()
    {
        MazeGenerator.OnMazeUnitRevealed += MazeGenerator_OnMazeUnitRevealed;
        Enemy.OnEnemyKilled += Enemy_OnAnyEnemyKilled;
    }

    void OnDisable()
    {
        MazeGenerator.OnMazeUnitRevealed -= MazeGenerator_OnMazeUnitRevealed;
        Enemy.OnEnemyKilled -= Enemy_OnAnyEnemyKilled;
    }

    void Start()
    {
        // TODO Random Roll to determine type of event and only set _enemy if that is the event chosen
        _enemy = Instantiate(_enemies[UnityEngine.Random.Range(0, _enemies.Count)], transform);
        _enemy.transform.Rotate(0, UnityEngine.Random.Range(-35f, 35f), 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.GetComponent<PlayerHealth>()) { return; }

        if(_enemy)
        {
            Vector3 lookAtTarget = new(other.transform.position.x, _enemy.transform.position.y, other.transform.position.z);
            _enemy.transform.LookAt(lookAtTarget);
            _enemy.StartBattle();
            return;
        }
        else
        {
            OnAnyDeadEndEvent?.Invoke();
            // TODO Activate non-enemy events!
        }
    }

    public void SetCoordinates(int x, int z)
    {
        _coordinates = new(x, z);
    }

    void MazeGenerator_OnMazeUnitRevealed(Vector2 coordinates)
    {
        if(!gameObject.activeSelf) { return; }
        if(_isRevealed) { return; }
        if(coordinates != _coordinates) { return; }

        Reveal();
    }

    void Enemy_OnAnyEnemyKilled(Enemy enemy)
    {
        if(enemy == _enemy)
        {
            // TODO Give special reward
            _enemy.EndBattle();
            gameObject.SetActive(false);
        }
    }

    public void Reveal()
    {
        _mapIcon.SetActive(true);
        _isRevealed = true;
    }
}
