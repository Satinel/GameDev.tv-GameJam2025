using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] List<Trinket> _allTrinkets = new();
    [SerializeField] PlayerInventory _inventory;
    [SerializeField] PlayerStats _stats;
    [SerializeField] PlayerHealth _health;
    
    // [SerializeField] GameObject _loadPrompt, _loadMenu, _savePrompt, _saveMenu;
    // [SerializeField] Animator _animator;
    // [SerializeField] SaveButton _loadButton, _loadAutoSaveButton, _saveButton;
    // [SerializeField] TextMeshProUGUI _errorText, _saveButtonText;

    bool _isSaving;

    // static readonly int SAVED_HASH = Animator.StringToHash("Saved");
    // static readonly int NOFILE_HASH = Animator.StringToHash("NoFile");
    // static readonly int SAVEFAILED_HASH = Animator.StringToHash("SaveFailed");
    const string WEBPATH = "/idbfs/FirstPersonScorpion/";
    const string SAVENAME = "gameData.txt";
    const string AUTOSAVENAME = "autoSave.txt";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        PlayerHealth.OnPlayerDeath += AutoSave;
        RestAreaUI.OnRestAreaUsed += AutoSave;
    }

    void OnDestroy()
    {
        PlayerHealth.OnPlayerDeath -= AutoSave;
        RestAreaUI.OnRestAreaUsed -= AutoSave;
    }

    void Start()
    {
        string path;
        string autoPath;
#if UNITY_WEBGL
{
            path = WEBPATH + SAVENAME;
            autoPath = WEBPATH + AUTOSAVENAME;
            // _saveButtonText.text = "Save & Quit";
}
#else
{
            path = Application.persistentDataPath + "/" +  SAVENAME;
            autoPath = Application.persistentDataPath + "/" +  AUTOSAVENAME;
}
#endif
        if(File.Exists(path))
        {
            string[] data = File.ReadAllLines(path);
            // _loadButton.Setup(data[0], data[1], data[2], data[3], data[8]);
            // _saveButton.Setup(data[0], data[1], data[2], data[3], data[8]);
        }
        if(File.Exists(autoPath))
        {
            string[] data = File.ReadAllLines(autoPath);
            // _loadAutoSaveButton.Setup(data[0], data[1], data[2], data[3], data[8]);
        }

        // AutoLoadAchievements();
    }

    public void OpenLoadMenu()
    {
        // _loadMenu.SetActive(true);
    }

    public void CloseLoadMenu()
    {
        // _loadPrompt.SetActive(false);
        // _loadMenu.SetActive(false);
    }

    public void PromptLoad()
    {
        // _loadPrompt.SetActive(true);
    }

    public void CancelLoad()
    {
        // _loadPrompt.SetActive(false);
    }

    public void OpenSaveMenu()
    {
        // _saveMenu.SetActive(true);
    }

    public void CloseSaveMenu()
    {
        // _savePrompt.SetActive(false);
        // _saveMenu.SetActive(false);
    }

    public void PromptSave()
    {
        // _savePrompt.SetActive(true);
    }

    public void CancelSave()
    {
        // _savePrompt.SetActive(false);
    }

    public void SaveMaze() // TODO? Save maze layout and location of everything...
    {
        MazeGenerator currentMaze = FindFirstObjectByType<MazeGenerator>();
        if(!currentMaze) { return; }

        string saveMazePath;
#if UNITY_WEBGL
{
        saveMazePath = WEBPATH + "saveMaze.txt"; // Note that if the Unity Editor is set to WebGL build this will create a folder in the root of the drive it is on
        if(!Directory.Exists(saveMazePath))
        {
            Directory.CreateDirectory("/idbfs/FirstPersonScorpion");
        }
}
#else
{
        saveMazePath = Application.persistentDataPath + "/saveMaze.txt"; // Note the / is needed here but not in WEBGL
}
#endif
        List<string> dataStrings = new ();

        dataStrings.Insert(dataStrings.Count, currentMaze.AllMazeUnits.Count.ToString());
        foreach(MazeUnit unit in currentMaze.AllMazeUnits)
        {
            dataStrings.Insert(dataStrings.Count, unit.IsWall.ToString());
        }

        dataStrings.Insert(dataStrings.Count, currentMaze.Goal.transform.position.ToString());
        dataStrings.Insert(dataStrings.Count, currentMaze.BossEncounter.transform.position.ToString());
        dataStrings.Insert(dataStrings.Count, currentMaze.RestArea.transform.position.ToString());
        dataStrings.Insert(dataStrings.Count, currentMaze.Store.transform.position.ToString());

        dataStrings.Insert(dataStrings.Count, currentMaze.Elites.Count.ToString());
        foreach(DeadEnd elite in currentMaze.Elites)
        {
            dataStrings.Insert(dataStrings.Count, elite.transform.position.ToString());
        }

        dataStrings.Insert(dataStrings.Count, currentMaze.RandomEncounters.Count.ToString());
        {
            foreach(RandomEncounter encounter in currentMaze.RandomEncounters)
            {
                dataStrings.Insert(dataStrings.Count, encounter.transform.position.ToString());
            }
        }

        File.WriteAllLines(saveMazePath, dataStrings);
    }


    public void LoadMaze() // TODO???? Load maze layout and location of everything.......
    {
        string saveMazePath;
#if UNITY_WEBGL
{
        saveMazePath = WEBPATH + "saveMaze.txt";
}
#else
{
        saveMazePath = Application.persistentDataPath + "/saveMaze.txt"; // Note the / is needed here but not in WEBGL
}
#endif
        if(File.Exists(saveMazePath))
        {
            // AutoSavedMoney = int.Parse(File.ReadAllText(saveMazePath));
        }
    }

    public void NewGamePlus()
    {
    
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
            dataStrings.Insert(dataStrings.Count, trinket.Name.ToString());
            dataStrings.Insert(dataStrings.Count, (trinket.Level + 1).ToString());
        }

        try
        {
            File.WriteAllLines(savePath, dataStrings);
            
            if(fileName == AUTOSAVENAME) { _isSaving = false; return; }
            
            // _savePrompt.SetActive(false);
            // _animator.SetTrigger(SAVED_HASH);
            // string[] savedData = File.ReadAllLines(savePath); // This was just for the buttons I think
            // _saveButton.Setup(savedData[0], savedData[1], savedData[2], savedData[3], savedData[8]);
            // _loadButton.Setup(savedData[0], savedData[1], savedData[2], savedData[3], savedData[8]); // Since Loading is only currently available at Main Title this should be irrelevant
#if UNITY_WEBGL
{
            // _campaign.ReturnToTitle();
}
#endif
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
            // _errorText.text = $"Failed to Save: {ex}";
            // _savePrompt.SetActive(false);
            // _animator.SetTrigger(SAVEFAILED_HASH);
        }
        _isSaving = false;
    }

    public void LoadDataFile(string fileName) // TODO Prompt about loading data
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
            // _animator.SetTrigger(NOFILE_HASH);
            // _loadPrompt.SetActive(false);
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
                if(trinket.Name == dataArray[i])
                {
                    savedTrinkets.Add(trinket);
                    trinketLevels.Add(int.Parse(dataArray[i + 1]));
                    break;
                }
            }
        }

        _inventory.LoadData(savedTrinkets, trinketLevels);

        CloseLoadMenu();
        // TODO Load a new maze
    }
}
