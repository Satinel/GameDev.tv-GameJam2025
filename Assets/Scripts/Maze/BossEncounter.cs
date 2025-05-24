using UnityEngine;

public class BossEncounter : MonoBehaviour
{
    [SerializeField] Enemy _bossPrefab;
    [SerializeField] GameObject _mapIcon, _door;
    [SerializeField] BoxCollider _collider;

    Enemy _boss;
    bool _isRevealed;
    Vector2 _coordinates = new();

    void OnEnable()
    {
        _boss = Instantiate(_bossPrefab, transform);
    }

    void Start()
    {
        MazeGenerator.OnMazeUnitRevealed += MazeGenerator_OnMazeUnitRevealed;
        Enemy.OnEnemyKilled += Enemy_OnAnyEnemyKilled;
        Goal.OnKeyClaimed += Goal_OnKeyClaimed;
    }

    void OnDestroy()
    {
        MazeGenerator.OnMazeUnitRevealed -= MazeGenerator_OnMazeUnitRevealed;
        Enemy.OnEnemyKilled -= Enemy_OnAnyEnemyKilled;
        Goal.OnKeyClaimed -= Goal_OnKeyClaimed;
    }

    void OnTriggerEnter(Collider other)
    {
        if(!_boss) { return; }

        if(other.gameObject.GetComponent<PlayerHealth>())
        {
            Vector3 lookAtTarget = new(other.transform.position.x, _boss.transform.position.y, other.transform.position.z);
            _boss.transform.LookAt(lookAtTarget);
            _boss.StartBattle();
        }
    }

    public void SetCoordinates(int x, int z)
    {
        _coordinates = new(x, z);
    }

    void Enemy_OnAnyEnemyKilled(Enemy enemy)
    {
        if(enemy == _boss)
        {
            _boss.EndBattle();
            gameObject.SetActive(false);
            // TODO Go to next level of dungeon or show a results screen or whatever!
        }
    }

    void MazeGenerator_OnMazeUnitRevealed(Vector2 coordinates)
    {
        if(_isRevealed) { return; }
        if(coordinates != _coordinates) { return; }

        Reveal();
    }

    void Goal_OnKeyClaimed()
    {
        Reveal();
        _collider.isTrigger = true;
        _door.transform.SetPositionAndRotation(new(_door.transform.position.x, _door.transform.position.y + 6, _door.transform.position.z), Quaternion.Euler(270, 0, 0));
    }

    public void Reveal()
    {
        _mapIcon.SetActive(true);
        _isRevealed = true;
    }
}
