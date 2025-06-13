using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public static event Action<Vector2> OnMazeUnitRevealed;
    public static event Action OnMazeReady;

    [SerializeField] [Range(0, 100)] int _encounterChancePercentage = 20;
    [SerializeField] int _width = 30, _depth = 30;
    [SerializeField] byte[,] _map;
    [SerializeField] int _scale = 6;
    [SerializeField] MazeUnit _mazeSpacePrefab, _mazeWallPrefab, _mazeWall2Prefab;
    [SerializeField] RandomEncounter _randomEncounterPrefab;
    [SerializeField] DeadEnd _deadEndPrefab;
    [SerializeField] Goal _goalPrefab;
    [SerializeField] BossEncounter _bossEncounterPrefab;
    [SerializeField] Store _storePrefab;
    [SerializeField] RestArea _restAreaPrefab;
    [SerializeField] Transform _mazeParent, _encountersParent, _endsParent;
    [SerializeField] bool _isNewMap;

    PlayerHealth _playerHealth;

    List<Vector2> _directions = new()
    {
        new Vector2(1, 0),
        new Vector2(0, 1),
        new Vector2(-1, 0),
        new Vector2(0, -1),
    };

    List<Vector2> _openSpaces = new();
    List<MazeSpace> _revealedSpaces = new();
    List<Vector2> _deadEnds = new();
    List<RandomEncounter> _randomEncounters = new();
    List<DeadEnd> _elites = new();
    public List<MazeUnit> AllMazeUnits { get; private set; } = new();
    public Goal Goal { get; private set; }
    public BossEncounter BossEncounter { get; set; }
    public RestArea RestArea { get; set; }
    public Store Store { get; private set; }
    public List<DeadEnd> Elites => _elites;
    public List<RandomEncounter> RandomEncounters => _randomEncounters;

    void Awake()
    {
        _playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    void OnEnable()
    {
        MazeSpace.OnAnySpaceEntered += MazeSpace_OnAnySpaceEntered;
        RestAreaUI.OnRestAreaUsed += RestAreaUI_OnRestAreaUsed;
    }

    void OnDisable()
    {
        MazeSpace.OnAnySpaceEntered -= MazeSpace_OnAnySpaceEntered;
        RestAreaUI.OnRestAreaUsed -= RestAreaUI_OnRestAreaUsed;
    }

    void Start()
    {
        if(_isNewMap)
        {
            GenerateNewMap();
        }
    }

    public void GenerateNewMap()
    {
        InitializeMap();
        Generate();
        DrawMap();
        PopulateMap();
        FillDeadEnds();
        FindFirstObjectByType<RestAreaUI>().PromptSave();
        OnMazeReady?.Invoke();
    }

    public void LoadMapData(string[] dataArray)
    {

        int currentIndex = int.Parse(dataArray[0]) * 3;
        _map = new byte[_width, _depth];
        for(int i = 1; i < currentIndex; i += 3)
        {
            _map[int.Parse(dataArray[i]), int.Parse(dataArray[i + 1])] = byte.Parse(dataArray[i + 2]);
        }

        DrawMap();

        currentIndex++;

        PlayerInventory playerInventory = FindFirstObjectByType<PlayerInventory>();

        if(bool.Parse(dataArray[currentIndex]))
        {
            playerInventory.GetKey();
        }
        currentIndex++;

        foreach(var space in _openSpaces)
        {
            if(space == _openSpaces[0])
            {
                RestArea restA = Instantiate(_restAreaPrefab, new(space.x * _scale, 0, space.y * _scale), Quaternion.identity, _endsParent);
                restA.SetCoordinates((int)space.x, (int)space.y);
                restA.name = $"restArea {space.x} {space.y}";
                RotateDeadEnd(restA.transform, space);
            }

            else if(CountSquareNeighbours((int)space.x, (int)space.y) == 1 && space != _openSpaces[_openSpaces.Count - 1] && space != _openSpaces[0])
            {
                _deadEnds.Add(space);
            }

            else if(space == _openSpaces[_openSpaces.Count - 1])
            {
                if(!playerInventory.HasKey)
                {    
                    Goal = Instantiate(_goalPrefab, new(space.x * _scale, 0, space.y * _scale), Quaternion.identity, transform);
                    Goal.SetCoordinates((int)space.x, (int)space.y);
                }
            }
        }

        BossEncounter = Instantiate(_bossEncounterPrefab, new(int.Parse(dataArray[currentIndex]), 0, int.Parse(dataArray[currentIndex + 1])), Quaternion.identity, _endsParent);
        int xBoss = (int)(BossEncounter.transform.position.x / _scale);
        int zBoss = (int)(BossEncounter.transform.position.z / _scale);
        BossEncounter.SetCoordinates(xBoss, zBoss);
        BossEncounter.transform.Rotate(0, int.Parse(dataArray[currentIndex + 2]), 0);
        if(playerInventory.HasKey)
        {
            BossEncounter.LoadOnKeyClaimed();
        }
        currentIndex += 3;

        RestArea = Instantiate(_restAreaPrefab, new(int.Parse(dataArray[currentIndex]), 0, int.Parse(dataArray[currentIndex + 1])), Quaternion.identity, _endsParent);
        int xRest = (int)(RestArea.transform.position.x / _scale);
        int zRest = (int)(RestArea.transform.position.z / _scale);
        RestArea.SetCoordinates(xRest, zRest);
        RestArea.SetCoordinates(int.Parse(dataArray[currentIndex / _scale]), int.Parse(dataArray[(currentIndex + 1) / _scale]));
        RestArea.name = $"RestArea {xRest} {zRest}";
        RestArea.transform.Rotate(0, int.Parse(dataArray[currentIndex + 2]), 0);
        currentIndex += 3;

        Store = Instantiate(_storePrefab, new(int.Parse(dataArray[currentIndex]), 0, int.Parse(dataArray[currentIndex + 1])), Quaternion.identity, _endsParent);
        int xStore = (int)(Store.transform.position.x / _scale);
        int zStore = (int)(Store.transform.position.z / _scale);
        Store.SetCoordinates(xStore, zStore);
        Store.name = $"Store {xStore} {zStore}";
        Store.transform.Rotate(0, int.Parse(dataArray[currentIndex + 2]), 0);
        currentIndex += 3;

        for(int i = currentIndex + 1; i < currentIndex + (int.Parse(dataArray[currentIndex]) * 3); i += 3)
        {
            DeadEnd deadEnd = Instantiate(_deadEndPrefab, new(int.Parse(dataArray[i]), 0, int.Parse(dataArray[i + 1])), Quaternion.identity, _endsParent);
            int xEnd = (int)(deadEnd.transform.position.x / _scale);
            int zEnd = (int)(deadEnd.transform.position.z / _scale);
            deadEnd.SetCoordinates(xEnd, zEnd);
            deadEnd.name = $"End {xEnd} {zEnd}";
            
            deadEnd.transform.Rotate(0, int.Parse(dataArray[i + 2]), 0);
            _elites.Add(deadEnd);
        }

        currentIndex += int.Parse(dataArray[currentIndex]) * 3;
        currentIndex++;

        for(int i = currentIndex + 1; i < currentIndex + (int.Parse(dataArray[currentIndex]) * 2); i += 2)
        {
            RandomEncounter randomEnc = Instantiate(_randomEncounterPrefab, new(int.Parse(dataArray[i]), 0, int.Parse(dataArray[i + 1])), Quaternion.identity, _encountersParent);
            int xCor = (int)(randomEnc.transform.position.x / _scale);
            int zCor = (int)(randomEnc.transform.position.z / _scale);
            randomEnc.SetCoordinates(xCor, zCor);
            randomEnc.name = $"Encounter {xCor} {zCor}";
            _randomEncounters.Add(randomEnc);
        }

        _playerHealth.gameObject.transform.position = new(float.Parse(dataArray[dataArray.Length - 2]), 0, float.Parse(dataArray[dataArray.Length - 1]));;
        _playerHealth.SetSpawnPoint();

        // foreach(MazeSpace space in _revealedSpaces)
        // {
        //     space.LoadRevealed();
        // }

        // OnMazeReady?.Invoke();
        Invoke(nameof(RevealDelay), 2.5f);
    }

    void RevealDelay()
    {
        foreach(MazeSpace space in _revealedSpaces)
        {
            space.LoadRevealed();
        }

        OnMazeReady?.Invoke();
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
                if(_map[x,z] == 1 || _map[x,z] == 3)
                {
                    MazeUnit wall;
                    if(z % 2 == 0)
                    {
                        if(x % 2 == 0)
                        {
                            wall = Instantiate(_mazeWall2Prefab, new(x * _scale, 0, z * _scale), Quaternion.identity, _mazeParent);
                        }
                        else
                        {
                            wall = Instantiate(_mazeWallPrefab, new(x * _scale, 0, z * _scale), Quaternion.identity, _mazeParent);
                        }
                    }
                    else
                    {
                        if(x % 2 == 0)
                        {
                            wall = Instantiate(_mazeWallPrefab, new(x * _scale, 0, z * _scale), Quaternion.identity, _mazeParent);
                        }
                        else
                        {
                            wall = Instantiate(_mazeWall2Prefab, new(x * _scale, 0, z * _scale), Quaternion.identity, _mazeParent);
                        }
                    }
                    wall.SetCoordinates(x, z);
                    wall.SetIsWall(_map[x,z]);
                    wall.transform.position = new(x * _scale, 0, z * _scale);
                    wall.name = $"Wall {x} {z}";
                    AllMazeUnits.Add(wall);
                }
                else
                {
                    MazeUnit space = Instantiate(_mazeSpacePrefab, new(x * _scale, 0, z * _scale), Quaternion.identity, _mazeParent);
                    space.SetCoordinates(x, z);
                    space.SetIsWall(_map[x,z]);
                    space.name = $"Floor {x} {z}";
                    _openSpaces.Add(new(x, z));
                    AllMazeUnits.Add(space);
                    if(_map[x,z] == 2)
                    {
                        _revealedSpaces.Add((MazeSpace)space);
                    }
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
                RestArea startRestArea = Instantiate(_restAreaPrefab, new(space.x * _scale, 0, space.y * _scale), Quaternion.identity, _endsParent);
                startRestArea.SetCoordinates((int)space.x, (int)space.y);
                startRestArea.name = $"restArea {space.x} {space.y}";
                RotateDeadEnd(startRestArea.transform, space);
                _playerHealth.gameObject.transform.position = new(space.x * _scale, 0, space.y * _scale);
                _playerHealth.SetSpawnPoint();
                continue;
            }

            if(space == _openSpaces[_openSpaces.Count - 1])
            {
                Goal = Instantiate(_goalPrefab, new(space.x * _scale, 0, space.y * _scale), Quaternion.identity, transform);
                Goal.SetCoordinates((int)space.x, (int)space.y);
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
                _randomEncounters.Add(randomEnc);
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
                BossEncounter = Instantiate(_bossEncounterPrefab, new(end.x * _scale, 0, end.y * _scale), Quaternion.identity, _endsParent);
                BossEncounter.SetCoordinates((int)end.x, (int)end.y);
                BossEncounter.name = $"Final Boss {end.x} {end.y}";
                RotateDeadEnd(BossEncounter.transform, end);
                return;
            }
            if(end == _deadEnds[0])
            {
                RestArea = Instantiate(_restAreaPrefab, new(end.x * _scale, 0, end.y * _scale), Quaternion.identity, _endsParent);
                RestArea.SetCoordinates((int)end.x, (int)end.y);
                RestArea.name = $"restArea {end.x} {end.y}";
                RotateDeadEnd(RestArea.transform, end);
                continue;
            }
            if(end == _deadEnds[1])
            {
                Store = Instantiate(_storePrefab, new(end.x * _scale, 0, end.y * _scale), Quaternion.identity, _endsParent);
                Store.SetCoordinates((int)end.x, (int)end.y);
                Store.name = $"Store {end.x} {end.y}";
                RotateDeadEnd(Store.transform, end);
                continue;
            }

            DeadEnd deadEnd = Instantiate(_deadEndPrefab, new(end.x * _scale, 0, end.y * _scale), Quaternion.identity, _endsParent);
            deadEnd.SetCoordinates((int)end.x, (int)end.y);
            deadEnd.name = $"Dead End {end.x} {end.y}";
            RotateDeadEnd(deadEnd.transform, end);
            _elites.Add(deadEnd);
        }
    }


    void RestAreaUI_OnRestAreaUsed()
    {
        foreach(RandomEncounter randomEncounter in _randomEncounters)
        {
            if(!randomEncounter.gameObject.activeSelf)
            {
                randomEncounter.gameObject.SetActive(true);
            }
        }
        foreach(DeadEnd end in _elites)
        {
            if(!end.gameObject.activeSelf && UnityEngine.Random.Range(0, 4) > 2)
            {
                end.RestAreaTrigger();
            }
        }
    }

    void RotateDeadEnd(Transform deadEnd, Vector2 loc)
    {
            if(_map[(int)loc.x + 1, (int)loc.y] == 0)
            {
                deadEnd.transform.Rotate(0, 90, 0);
            }
            if(_map[(int)loc.x, (int)loc.y - 1] == 0)
            {
                deadEnd.transform.Rotate(0, 180, 0);
            }
            if(_map[(int)loc.x - 1, (int)loc.y] == 0)
            {
                deadEnd.transform.Rotate(0, 270, 0);
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
