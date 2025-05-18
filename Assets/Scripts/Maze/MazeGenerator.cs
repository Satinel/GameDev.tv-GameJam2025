using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public static event Action<Vector2> OnMazeUnitRevealed;

    [SerializeField] [Range(0, 100)] int _encounterChancePercentage = 20;
    [SerializeField] int _width = 30, _depth = 30;
    [SerializeField] byte[,] _map;
    [SerializeField] int _scale = 6;
    [SerializeField] MazeUnit _mazeSpacePrefab, _mazeWallPrefab;
    [SerializeField] RandomEncounter _randomEncounterPrefab;
    [SerializeField] DeadEnd _deadEndPrefab;
    [SerializeField] Goal _goalPrefab;
    [SerializeField] BossEncounter _bossEncounterPrefab;
    [SerializeField] Transform _mazeParent, _encountersParent, _endsParent;
    GameObject _player;

    List<Vector2> _directions = new()
    {
        new Vector2(1, 0),
        new Vector2(0, 1),
        new Vector2(-1, 0),
        new Vector2(0, -1),
    };

    List<Vector2> _openSpaces = new();
    List<Vector2> _deadEnds = new();

    void OnEnable()
    {
        MazeSpace.OnAnySpaceEntered += MazeSpace_OnAnySpaceEntered;
    }

    void OnDisable()
    {
        MazeSpace.OnAnySpaceEntered -= MazeSpace_OnAnySpaceEntered;
    }

    void Start()
    {
        _player = FindFirstObjectByType<PlayerHealth>().gameObject;
        // if(!_player) { return; }

        InitializeMap();
        Generate();
        DrawMap();
        PopulateMap();
        FillDeadEnds();
    }

    void InitializeMap()
    {
        _map = new byte[_width, _depth];

        for(int z = 0; z < _depth; z++)
        {
            for(int x = 0; x < _width; x++)
            {
                _map[x,z] = 1; // 0 = floor, 1 = wall
            }
        }
    }

    void Generate()
    {
        Generate(UnityEngine.Random.Range(1, _width - 1), UnityEngine.Random.Range(1, _depth - 1));
    }

    void Generate(int x, int z)
    {
        if(CountSquareNeighbours(x, z) >= 2) { return; }

        _map[x, z] = 0;

        _directions.Shuffle();

        Generate(x + (int)_directions[0].x, z + (int)_directions[0].y);
        Generate(x + (int)_directions[1].x, z + (int)_directions[1].y);
        Generate(x + (int)_directions[2].x, z + (int)_directions[2].y);
        Generate(x + (int)_directions[3].x, z + (int)_directions[3].y);
    }

    void DrawMap()
    {
        for (int z = 0; z < _depth; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                if(_map[x,z] == 1)
                {
                    MazeUnit wall = Instantiate(_mazeWallPrefab, new(x * _scale, 0, z * _scale), Quaternion.identity, _mazeParent);
                    wall.SetCoordinates(x, z);
                    wall.transform.position = new(x * _scale, 0, z * _scale);
                    wall.name = $"Wall {x} {z}";
                }
                else
                {
                    MazeUnit space = Instantiate(_mazeSpacePrefab, new(x * _scale, 0, z * _scale), Quaternion.identity, _mazeParent);
                    space.SetCoordinates(x, z);
                    space.name = $"Floor {x} {z}";
                    _openSpaces.Add(new(x, z));
                }
            }
        }
    }

    int CountSquareNeighbours(int x, int z)
    {
        if(x <= 0 || x >= _width -1 || z <= 0 || z >= _depth - 1)
        {
            return 5;
        }

        int count = 0;

        if(_map[x - 1, z] == 0){ count++;}
        if(_map[x + 1, z] == 0){ count++;}
        if(_map[x, z - 1] == 0){ count++;}
        if(_map[x, z + 1] == 0){ count++;}

        return count;
    }

    void PopulateMap()
    {
        foreach(var space in _openSpaces)
        {
            if(space == _openSpaces[0])
            {
                _player.transform.position = new(space.x * _scale, 0, space.y * _scale);
                continue;
            }

            if(space == _openSpaces[_openSpaces.Count - 1])
            {
                Goal goal = Instantiate(_goalPrefab, new(space.x * _scale, 0, space.y * _scale), Quaternion.identity, transform);
                goal.SetCoordinates((int)space.x, (int)space.y);
                return;
            }

            if(CountSquareNeighbours((int)space.x, (int)space.y) == 1)
            {
                _deadEnds.Add(space);
            }
            else if(UnityEngine.Random.Range(0, 100) < _encounterChancePercentage)
            {
                RandomEncounter randomEnc = Instantiate(_randomEncounterPrefab, new(space.x * _scale, 0, space.y * _scale), Quaternion.identity, _encountersParent);
                randomEnc.SetCoordinates((int)space.x, (int)space.y);

                randomEnc.name = $"Random Encounter {space.x} {space.y}";
            }
        }
    }

    void FillDeadEnds()
    {
        _deadEnds.Shuffle();

        foreach(var end in _deadEnds)
        {
            if(end == _deadEnds[_deadEnds.Count - 1])
            {
                BossEncounter bossEncounter = Instantiate(_bossEncounterPrefab, new(end.x * _scale, 0, end.y * _scale), Quaternion.identity, _endsParent);
                bossEncounter.SetCoordinates((int)end.x, (int)end.y);
                bossEncounter.name = $"Final Boss {end.x} {end.y}";
                return;
            }
            // TODO Create a store at _deadEnds[0] (Note that this won't conflict with bossEncounter since in the case of 1 DeadEnd it would happen first and exit the foreach)

            DeadEnd deadEnd = Instantiate(_deadEndPrefab, new(end.x * _scale, 0, end.y * _scale), Quaternion.identity, _endsParent);
            deadEnd.SetCoordinates((int)end.x, (int)end.y);
            deadEnd.name = $"Dead End {end.x} {end.y}";
        }
    }

    void MazeSpace_OnAnySpaceEntered(Vector2 coordinates)
    {
        // OnMazeUnitRevealed?.Invoke(new Vector2(coordinates.x, coordinates.y));
        // Squares
        OnMazeUnitRevealed?.Invoke(new Vector2(coordinates.x, coordinates.y - 1));
        OnMazeUnitRevealed?.Invoke(new Vector2(coordinates.x - 1, coordinates.y));
        OnMazeUnitRevealed?.Invoke(new Vector2(coordinates.x + 1 , coordinates.y));
        OnMazeUnitRevealed?.Invoke(new Vector2(coordinates.x, coordinates.y + 1));
        // Diagonals
        // OnMazeUnitRevealed?.Invoke(new Vector2(coordinates.x - 1, coordinates.y - 1));
        // OnMazeUnitRevealed?.Invoke(new Vector2(coordinates.x + 1, coordinates.y - 1));
        // OnMazeUnitRevealed?.Invoke(new Vector2(coordinates.x - 1, coordinates.y + 1));
        // OnMazeUnitRevealed?.Invoke(new Vector2(coordinates.x + 1, coordinates.y + 1));
    }
}
