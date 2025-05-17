using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] int _encounterChancePercentage = 20;
    [SerializeField] int _width = 30, _depth = 30;
    [SerializeField] byte[,] _map;
    [SerializeField] int _scale = 6;
    [SerializeField] GameObject _floorPiece, _wallPiece;
    [SerializeField] GameObject _randomEncounter, _goal;
    [SerializeField] Transform _mazeParent, _encountersParent;
    GameObject _player;

    List<Vector2> _directions = new()
    {
        new Vector2(1, 0),
        new Vector2(0, 1),
        new Vector2(-1, 0),
        new Vector2(0, -1),
    };

    List<Vector2> _openSpaces = new();

    void Start()
    {
        _player = FindFirstObjectByType<PlayerHealth>().gameObject;
        // if(!_player) { return; }

        InitializeMap();
        Generate();
        DrawMap();
        PopulateMap();
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
        Generate(Random.Range(1, _width - 1), Random.Range(1, _depth - 1));
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
                    GameObject wall = Instantiate(_wallPiece, new(x * _scale, 0, z * _scale), Quaternion.identity, _mazeParent);
                    wall.transform.position = new(x * _scale, 0, z * _scale);
                    wall.name = $"Wall {x} {z}";
                }
                else
                {
                    GameObject floor = Instantiate(_floorPiece, new(x * _scale, 0, z * _scale), Quaternion.identity, _mazeParent);
                    floor.name = $"Floor {x} {z}";
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
                Instantiate(_goal, new(space.x * _scale, 0, space.y * _scale), Quaternion.identity, transform);
                return;
            }
            if(Random.Range(0, 100) < _encounterChancePercentage)
            {
                GameObject randomEnc = Instantiate(_randomEncounter, new(space.x * _scale, 0, space.y * _scale), Quaternion.identity, _encountersParent);

                randomEnc.name = $"Random Encounter {space.x} {space.y}";
            }
        }
    }
}
