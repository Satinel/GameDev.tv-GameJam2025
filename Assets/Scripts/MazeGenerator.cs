using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] int _width = 30, _depth = 30;
    [SerializeField] byte[,] _map;
    [SerializeField] int _scale = 6;
    [SerializeField] GameObject _floorPiece, _wallPiece;
    [SerializeField] GameObject _player;

    protected List<Vector2> _directions = new()
    {
        new Vector2(1, 0),
        new Vector2(0, 1),
        new Vector2(-1, 0),
        new Vector2(0, -1),
    };

    void Start()
    {
        InitializeMap();
        Generate();
        DrawMap();
        PlacePlayer();
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
                    GameObject wall = Instantiate(_wallPiece, new(x * _scale, 0, z * _scale), Quaternion.identity);
                    wall.transform.position = new(x * _scale, 0, z * _scale);
                    wall.name = "Wall";
                }
                else
                {
                    GameObject floor = Instantiate(_floorPiece, new(x * _scale, 0, z * _scale), Quaternion.identity);
                    floor.name = "Floor";
                }
            }
        }
    }

    public int CountSquareNeighbours(int x, int z)
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

    void PlacePlayer()
    {
        if(!_player) { return; }

        for(int z = 1; z <= _depth - 1; z++)
        {
            for(int x = 1; x <= _width - 1; x++)
            {
                if(_map[x, z] == 0)
                {
                    _player.transform.position = new(x * _scale, 0, z * _scale);
                    return;
                }
            }
        }
    }
}
