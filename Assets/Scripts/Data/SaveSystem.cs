using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public static event Action OnMazeDataFound;
    public static event Action OnSaveFound;
    public static event Action OnLoadStarted;
    public static event Action OnSaveStarted;
    public static event Action<string> OnSaveFailed;
    public static event Action OnSaveCompleted;

    [SerializeField] List<Trinket> _allTrinkets = new();
    [SerializeField] PlayerInventory _inventory;
    [SerializeField] PlayerStats _stats;
    [SerializeField] PlayerHealth _health;

    bool _isSaving;
    int _sceneIndex = 2;

    const string WEBPATH = "/idbfs/FirstPersonScorpion/";
    const string SAVENAME = "gameData.txt";
    const string SAVEMAZENAME = "saveMaze.txt";
    const string PPSAVE = "ppSave";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        TitleScreen.OnMusicStarted += TitleScreen_OnMusicStarted;
        TitleScreen.OnCreepComplete += TitleScreen_OnCreepComplete;
        RestAreaUI.OnSaveConfirmed += ManualSave;
    }

    void OnDestroy()
    {
        TitleScreen.OnMusicStarted -= TitleScreen_OnMusicStarted;
        TitleScreen.OnCreepComplete -= TitleScreen_OnCreepComplete;
        RestAreaUI.OnSaveConfirmed -= ManualSave;
    }

    void TitleScreen_OnMusicStarted()
    {
#if UNITY_WEBGL
{
        // path = WEBPATH + SAVENAME;
        if(PlayerPrefs.GetString(PPSAVE, string.Empty) != string.Empty)
        {
            OnSaveFound?.Invoke();
        }
}
#else
{
        string path;
        path = Application.persistentDataPath + "/" +  SAVENAME;
        path = Application.persistentDataPath + "/" +  SAVEMAZENAME;

        if(File.Exists(path))
        {
            OnSaveFound?.Invoke();
        }
}
#endif
        if(CheckMazeData())
        {
            OnMazeDataFound?.Invoke();
        }
    }

    public void NewGamePlus() // UI Button on Title Screen
    {
        LoadDataFile(SAVENAME, false);
    }

    public void ContinueButton() // UI Button on Title Screen
    {
        LoadDataFile(SAVENAME, true);
    }

    public void ManualSave()
    {
        SaveDataFile(SAVENAME);
    }

    public void SaveDataFile(string fileName)
    {
        if(_isSaving) { return; }

        _isSaving = true;
        OnSaveStarted?.Invoke();

#if UNITY_WEBGL
{
        // savePath = WEBPATH + fileName;
        // if(!Directory.Exists(savePath))
        // {
        //     Directory.CreateDirectory("/idbfs/FirstPersonScorpion");
        // }
}
#else
{
        string savePath;
        savePath = Application.persistentDataPath + "/" +  fileName; // Note the / is needed here but not in WEBGL
}
#endif

        List<string> dataStrings = new();

        dataStrings.Insert(dataStrings.Count, _stats.Level.ToString());
        dataStrings.Insert(dataStrings.Count, _stats.CurrentXP.ToString());
        dataStrings.Insert(dataStrings.Count, _stats.NextLevelXP.ToString());

        dataStrings.Insert(dataStrings.Count, _stats.Strength.ToString());
        dataStrings.Insert(dataStrings.Count, _stats.Accuracy.ToString());
        dataStrings.Insert(dataStrings.Count, _stats.Fortitude.ToString());
        dataStrings.Insert(dataStrings.Count, _stats.Evasion.ToString());
        dataStrings.Insert(dataStrings.Count, _stats.Tenacity.ToString());
        dataStrings.Insert(dataStrings.Count, _stats.Initiative.ToString());
        
        dataStrings.Insert(dataStrings.Count, _stats.Money.ToString());
        
        bool isTutorial = SceneManager.GetActiveScene().buildIndex == 1;
        dataStrings.Insert(dataStrings.Count, isTutorial.ToString());

        dataStrings.Insert(dataStrings.Count, _health.MaxHealth.ToString());
        dataStrings.Insert(dataStrings.Count, PlayerHealth.DungeonFloor.ToString());


        foreach(Trinket trinket in _inventory.GetInventory())
        {
            dataStrings.Insert(dataStrings.Count, trinket.StartingName.ToString());
            dataStrings.Insert(dataStrings.Count, (trinket.Level + 1).ToString());
        }
#if UNITY_WEBGL
{
        string webSave = string.Empty;
        for(int i = 0; i < dataStrings.Count; i++)
        {
           webSave += $"{dataStrings[i]}\n";
        }

        try
        {
            PlayerPrefs.SetString(PPSAVE, webSave);
        }
        catch(Exception e)
        {
            OnSaveFailed?.Invoke(e.ToString());
        }
}
#else
{
        try
        {
            File.WriteAllLines(savePath, dataStrings);
            
            // if(fileName == AUTOSAVENAME)
            // {
            //     _isSaving = false;
            //     return;
            // }
        }
        catch(Exception ex)
        {
            OnSaveFailed?.Invoke(ex.ToString());
            // _errorText.text = $"Failed to Save: {ex}";
            // _savePrompt.SetActive(false);
            // _animator.SetTrigger(SAVEFAILED_HASH);
        }

        SaveMaze();
}
#endif

        _isSaving = false;
        OnSaveCompleted?.Invoke();
    }

    public void SaveMaze()
    {
#if UNITY_WEBGL
{
        // saveMazePath = WEBPATH + SAVEMAZENAME; // Note that if the Unity Editor is set to WebGL build this will create a folder in the root of the drive it is on
        // if(!Directory.Exists(saveMazePath))
        // {
        //     Directory.CreateDirectory("/idbfs/FirstPersonScorpion");
        // }
}
#else
{
        string saveMazePath;

        saveMazePath = Application.persistentDataPath + "/" + SAVEMAZENAME; // Note the / is needed here but not in WEBGL

        List<string> dataStrings = new();

        MazeGenerator currentMaze = FindFirstObjectByType<MazeGenerator>();
        if(!currentMaze) { return; }

        dataStrings.Insert(dataStrings.Count, currentMaze.AllMazeUnits.Count.ToString());
        foreach(MazeUnit unit in currentMaze.AllMazeUnits)
        {
            dataStrings.Insert(dataStrings.Count, unit.Coordinates.x.ToString());
            dataStrings.Insert(dataStrings.Count, unit.Coordinates.y.ToString());
            dataStrings.Insert(dataStrings.Count, unit.IsWall.ToString());
        }

        dataStrings.Insert(dataStrings.Count, _inventory.HasKey.ToString());

        dataStrings.Insert(dataStrings.Count, currentMaze.BossEncounter.transform.position.x.ToString());
        dataStrings.Insert(dataStrings.Count, currentMaze.BossEncounter.transform.position.z.ToString());
        dataStrings.Insert(dataStrings.Count, currentMaze.BossEncounter.transform.rotation.eulerAngles.y.ToString());

        dataStrings.Insert(dataStrings.Count, currentMaze.RestArea.transform.position.x.ToString());
        dataStrings.Insert(dataStrings.Count, currentMaze.RestArea.transform.position.z.ToString());
        dataStrings.Insert(dataStrings.Count, currentMaze.RestArea.transform.rotation.eulerAngles.y.ToString());

        dataStrings.Insert(dataStrings.Count, currentMaze.Store.transform.position.x.ToString());
        dataStrings.Insert(dataStrings.Count, currentMaze.Store.transform.position.z.ToString());
        dataStrings.Insert(dataStrings.Count, currentMaze.Store.transform.rotation.eulerAngles.y.ToString());

        dataStrings.Insert(dataStrings.Count, currentMaze.Elites.Count.ToString());
        foreach(DeadEnd elite in currentMaze.Elites)
        {
            dataStrings.Insert(dataStrings.Count, elite.transform.position.x.ToString());
            dataStrings.Insert(dataStrings.Count, elite.transform.position.z.ToString());
            dataStrings.Insert(dataStrings.Count, elite.transform.rotation.eulerAngles.y.ToString());
        }

        dataStrings.Insert(dataStrings.Count, currentMaze.RandomEncounters.Count.ToString());
        {
            foreach(RandomEncounter encounter in currentMaze.RandomEncounters)
            {
                dataStrings.Insert(dataStrings.Count, encounter.transform.position.x.ToString());
                dataStrings.Insert(dataStrings.Count, encounter.transform.position.z.ToString());
            }
        }

        dataStrings.Insert(dataStrings.Count, _health.GetSpawnPosition().x.ToString());
        dataStrings.Insert(dataStrings.Count, _health.GetSpawnPosition().z.ToString());

        File.WriteAllLines(saveMazePath, dataStrings);
}
#endif
    }

    public void LoadDataFile(string fileName, bool loadMaze)
    {
        string[] dataArray;
#if UNITY_WEBGL
{
        // loadPath = WEBPATH + fileName;
        if(PlayerPrefs.GetString(PPSAVE, string.Empty) == string.Empty) { return; }

        dataArray = PlayerPrefs.GetString(PPSAVE, string.Empty).Split('\n');
}
#else
{
        string loadPath;
        loadPath = Application.persistentDataPath + "/" +  fileName; // Note again the "/" is needed here but not in WEBGL

        if(File.Exists(loadPath))
        {
            dataArray = File.ReadAllLines(loadPath);
        }
        else
        {
            return;
        }
}
#endif

        if(loadMaze)
        {
            _stats.LoadData(int.Parse(dataArray[0]), int.Parse(dataArray[1]), int.Parse(dataArray[2]), int.Parse(dataArray[3]), int.Parse(dataArray[4]), 
                            int.Parse(dataArray[5]), int.Parse(dataArray[6]), int.Parse(dataArray[7]), int.Parse(dataArray[8]), int.Parse(dataArray[9]));
        }
        else
        {
            _stats.LoadStats(int.Parse(dataArray[3]), int.Parse(dataArray[4]), 
                             int.Parse(dataArray[5]), int.Parse(dataArray[6]), int.Parse(dataArray[7]), int.Parse(dataArray[8]), int.Parse(dataArray[9]));
        }

        bool isTutorial = bool.Parse(dataArray[10]);

        _health.LoadData(int.Parse(dataArray[11]), int.Parse(dataArray[12]));

        List<Trinket> savedTrinkets = new();
        List<int> trinketLevels = new();

        for(int i = 13; i < dataArray.Length; i += 2)
        {
            foreach(Trinket trinket in _allTrinkets)
            {
                if(trinket.StartingName == dataArray[i])
                {
                    savedTrinkets.Add(trinket);
                    trinketLevels.Add(int.Parse(dataArray[i + 1]));
                    break;
                }
            }
        }

        _inventory.LoadData(savedTrinkets, trinketLevels);

        if(isTutorial)
        {
            _sceneIndex = 1;
        }
        else if(loadMaze && CheckMazeData())
        {
            _sceneIndex = 3;
        }
        else
        {
            _sceneIndex = 2;
        }
        OnLoadStarted?.Invoke();
    }

    bool CheckMazeData()
    {
#if UNITY_WEBGL
{
        return false;
        // saveMazePath = WEBPATH + SAVEMAZENAME;
}
#else
{
        string saveMazePath;

        saveMazePath = Application.persistentDataPath + "/" + SAVEMAZENAME; // Note the / is needed here but not in WEBGL
        if(File.Exists(saveMazePath))
        {
            return true;
        }
        else
        {
            return false;
        }
}
#endif
    }

    void TitleScreen_OnCreepComplete()
    {
        if(_sceneIndex == 1)
        {
            SceneManager.LoadScene(_sceneIndex);
        }
        else if(_sceneIndex == 2)
        {
            StartCoroutine(NewSceneRoutine());
        }
        else
        {
            StartCoroutine(LoadMazeSceneRoutine());
        }
    }

    IEnumerator NewSceneRoutine()
    {
        yield return SceneManager.LoadSceneAsync(_sceneIndex);
    }

    IEnumerator LoadMazeSceneRoutine()
    {
        yield return SceneManager.LoadSceneAsync(_sceneIndex);

        yield return SetupMazeRoutine();
    }

    IEnumerator SetupMazeRoutine()
    {
        string saveMazePath;
        string[] dataArray;

#if UNITY_WEBGL
{
        saveMazePath = WEBPATH + SAVEMAZENAME;
}
#else
{
        saveMazePath = Application.persistentDataPath + "/" + SAVEMAZENAME; // Note the / is needed here but not in WEBGL
}
#endif
        dataArray = File.ReadAllLines(saveMazePath);

        MazeGenerator mazeGenerator = FindFirstObjectByType<MazeGenerator>();

        mazeGenerator.LoadMapData(dataArray);

        yield return null;
    }
}
