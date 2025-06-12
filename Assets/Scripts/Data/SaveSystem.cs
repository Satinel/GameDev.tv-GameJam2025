using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public static event Action OnSaveDataFound;
    public static event Action OnAutoSaveFound;
    public static event Action OnLoadStarted;
    public static event Action OnMazeLoaded;

    [SerializeField] List<Trinket> _allTrinkets = new();
    [SerializeField] PlayerInventory _inventory;
    [SerializeField] PlayerStats _stats;
    [SerializeField] PlayerHealth _health;
    
    // [SerializeField] GameObject _loadPrompt, _loadMenu, _savePrompt, _saveMenu;
    // [SerializeField] Animator _animator;
    // [SerializeField] SaveButton _loadButton, _loadAutoSaveButton, _saveButton;
    // [SerializeField] TextMeshProUGUI _errorText, _saveButtonText;

    bool _isSaving;
    int _sceneIndex = 2;

    // static readonly int SAVED_HASH = Animator.StringToHash("Saved");
    // static readonly int NOFILE_HASH = Animator.StringToHash("NoFile");
    // static readonly int SAVEFAILED_HASH = Animator.StringToHash("SaveFailed");
    const string WEBPATH = "/idbfs/FirstPersonScorpion/";
    const string AUTOSAVENAME = "autoSave.txt";
    const string SAVENAME = "gameData.txt";
    const string SAVEMAZENAME = "saveMaze.txt";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        TitleScreen.OnMusicStarted += TitleScreen_OnMusicStarted;
        TitleScreen.OnCreepComplete += TitleScreen_OnCreepComplete;
        PlayerHealth.OnPlayerDeath += AutoSave;
        RestAreaUI.OnRestAreaUsed += AutoSave;
    }

    void OnDestroy()
    {
        TitleScreen.OnMusicStarted -= TitleScreen_OnMusicStarted;
        TitleScreen.OnCreepComplete -= TitleScreen_OnCreepComplete;
        PlayerHealth.OnPlayerDeath -= AutoSave;
        RestAreaUI.OnRestAreaUsed -= AutoSave;
    }

    void TitleScreen_OnMusicStarted()
    {
        string path;
        string autoPath;
#if UNITY_WEBGL
{
        path = WEBPATH + SAVENAME;
        autoPath = WEBPATH + AUTOSAVENAME;
}
#else
{
        path = Application.persistentDataPath + "/" +  SAVENAME;
        path = Application.persistentDataPath + "/" +  SAVEMAZENAME;
        autoPath = Application.persistentDataPath + "/" +  AUTOSAVENAME;
}
#endif
        if(File.Exists(path) && CheckMazeData())
        {
            OnSaveDataFound?.Invoke();
        }
        if(File.Exists(autoPath))
        {
            OnAutoSaveFound?.Invoke();
        }
    }

    public void OpenSaveMenu() // TODO? Move to Rest Area
    {
        // _saveMenu.SetActive(true);
    }

    public void CloseSaveMenu() // TODO Move to Rest Area
    {
        // _savePrompt.SetActive(false);
        // _saveMenu.SetActive(false);
    }

    public void PromptSave() // TODO Move to Rest Area
    {
        // _savePrompt.SetActive(true);
    }

    public void CancelSave() // TODO Move to Rest Area
    {
        // _savePrompt.SetActive(false);
    }

    public void NewGamePlus() // UI Button on Title Screen
    {
        LoadDataFile(AUTOSAVENAME, false);
    }

    public void ContinueButton() // UI Button on Title Screen
    {
        LoadDataFile(SAVENAME, true);
    }

    public void AutoSave()
    {
        SaveDataFile(AUTOSAVENAME);
    }

    public void ManualSave()
    {
        SaveDataFile(SAVENAME);
    }

    public void SaveDataFile(string fileName)
    {
        if(_isSaving) { return; }

        _isSaving = true;

        string savePath;

#if UNITY_WEBGL
{
        savePath = WEBPATH + fileName;
        if(!Directory.Exists(savePath))
        {
            Directory.CreateDirectory("/idbfs/FirstPersonScorpion");
        }
}
#else
{
        savePath = Application.persistentDataPath + "/" +  fileName; // Note the / is needed here but not in WEBGL
}
#endif

        List<string> dataStrings = new ();

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
        
        dataStrings.Insert(dataStrings.Count, _health.CurrentHealth.ToString());
        dataStrings.Insert(dataStrings.Count, _health.MaxHealth.ToString());
        dataStrings.Insert(dataStrings.Count, PlayerHealth.DungeonFloor.ToString());


        foreach(Trinket trinket in _inventory.GetInventory())
        {
            dataStrings.Insert(dataStrings.Count, trinket.StartingName.ToString());
            dataStrings.Insert(dataStrings.Count, (trinket.Level + 1).ToString());
        }

        try
        {
            File.WriteAllLines(savePath, dataStrings);
            
            if(fileName == AUTOSAVENAME) { _isSaving = false; return; }
// #if UNITY_WEBGL
// {
//             // _campaign.ReturnToTitle();
// }
// #endif
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
            // _errorText.text = $"Failed to Save: {ex}";
            // _savePrompt.SetActive(false);
            // _animator.SetTrigger(SAVEFAILED_HASH);
        }
        SaveMaze();
        _isSaving = false;
    }

    public void SaveMaze()
    {
        MazeGenerator currentMaze = FindFirstObjectByType<MazeGenerator>();
        if(!currentMaze) { return; }

        string saveMazePath;
#if UNITY_WEBGL
{
        saveMazePath = WEBPATH + SAVEMAZENAME; // Note that if the Unity Editor is set to WebGL build this will create a folder in the root of the drive it is on
        if(!Directory.Exists(saveMazePath))
        {
            Directory.CreateDirectory("/idbfs/FirstPersonScorpion");
        }
}
#else
{
        saveMazePath = Application.persistentDataPath + "/" + SAVEMAZENAME; // Note the / is needed here but not in WEBGL
}
#endif
        List<string> dataStrings = new ();

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

        File.WriteAllLines(saveMazePath, dataStrings);
    }

    public void LoadDataFile(string fileName, bool loadMaze)
    {
        string loadPath;
        string[] dataArray;
#if UNITY_WEBGL
{
        loadPath = WEBPATH + fileName;
}
#else
{
        loadPath = Application.persistentDataPath + "/" +  fileName; // Note again the "/" is needed here but not in WEBGL
}
#endif
        if(File.Exists(loadPath))
        {
            dataArray = File.ReadAllLines(loadPath);
        }
        else
        {
            return;
        }

        _stats.LoadData(int.Parse(dataArray[0]), int.Parse(dataArray[1]), int.Parse(dataArray[2]), int.Parse(dataArray[3]), int.Parse(dataArray[4]), 
                        int.Parse(dataArray[5]), int.Parse(dataArray[6]), int.Parse(dataArray[7]), int.Parse(dataArray[8]), int.Parse(dataArray[9]));

        _health.LoadData(int.Parse(dataArray[10]), int.Parse(dataArray[11]), int.Parse(dataArray[12]));

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

        if(loadMaze && CheckMazeData())
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
        string saveMazePath;
#if UNITY_WEBGL
{
        saveMazePath = WEBPATH + SAVEMAZENAME;
}
#else
{
        saveMazePath = Application.persistentDataPath + "/" + SAVEMAZENAME; // Note the / is needed here but not in WEBGL
}
#endif
        if(File.Exists(saveMazePath))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void TitleScreen_OnCreepComplete()
    {
        if(_sceneIndex == 2)
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

        OnMazeLoaded?.Invoke();
    }

    IEnumerator LoadMazeSceneRoutine()
    {
        yield return SceneManager.LoadSceneAsync(_sceneIndex);

        yield return SetupMazeRoutine();

        OnMazeLoaded?.Invoke();
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
