using UnityEngine;
using SimpleJSON;
using System.IO;

public class GameManager : MonoBehaviour
{
    public bool isPausableScene { get { return Application.loadedLevelName != GameManager.Instance.mainMenuSceneName; } }
    public bool isPaused { get { return _isPaused; } }
    public int extraLives { get { return _extraLives; } }
    public string mainMenuSceneName = "main_menu";

    private JSONClass saveData;
    private string savePath;
    private bool _isPaused = false;
    private int _extraLives = 3;
    private string moveToDoorName = null;

    public static GameManager Instance { get { return _instance; } }
    private static GameManager _instance = null;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        savePath = Application.persistentDataPath + "/";
    }

    void OnLevelWasLoaded(int level) {
        if(moveToDoorName != null){
            GameObject door = GameObject.Find(moveToDoorName);
            if(door != null){
                GameObject player = GameObject.Find("Player");
                GameObject camera = GameObject.Find("Main Camera");
                player.transform.position = new Vector2(door.transform.position.x, door.transform.position.y);
                camera.transform.position = new Vector2(door.transform.position.x, door.transform.position.y);
                moveToDoorName = null;
            }
        }
    }

    public void Pause()
    {
        _isPaused = !_isPaused;
    }

    public void ResetGame()
    {
        _isPaused = false;
        MenuManager.Instance.CloseAllMenus();
        Pause();
        MenuManager.Instance.SetPanel("Main Panel");

        Application.LoadLevel(1);
    }

    public void RespawnPlayer()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    // Saving and Loading stuff
    public void GenerateNewSaveFile(int gameSave)
    {
        // Creates the settings file if it doesn't exist yet
        string fileName = "saveData_" + gameSave.ToString() + ".tone";
        if (!File.Exists(savePath + fileName))
        {
            saveData = new JSONClass();
            // Player Data
            saveData["playerData"]["currentLevel"].AsInt = gameSave;

            // Graphics
            saveData["settings"]["graphics"]["fullscreen"].AsBool = true;

            // Audio
            saveData["settings"]["audio"]["masterVolume"].AsFloat = 1f;
            saveData["settings"]["audio"]["sfxVolume"].AsFloat = 1f;
            saveData["settings"]["audio"]["bgmVolume"].AsFloat = 1f;

            // Game Settings
            saveData["settings"]["gameSettings"]["test"].AsInt = 1;

            SaveToFile(fileName);
        }
        else
        {
            Debug.LogError("Save file: " + fileName + " already exists!");
        }
    }

    public void SaveGame(int gameSave)
    {
        string fileName = "saveData_" + gameSave.ToString() + ".tone";
        saveData = new JSONClass();
        saveData["playerData"]["currentLevel"].AsInt = gameSave;

        SaveToFile(fileName);
    }

    public void LoadGame(int gameSave)
    {
        string fileName = "saveData_" + gameSave.ToString() + ".tone";
        saveData = (JSONClass)JSONNode.Parse(LoadFromFile(fileName));
        
        Application.LoadLevel(saveData["playerData"]["currentLevel"].AsInt);
    }

    public void SaveSettings(int gameSave)
    {
        string fileName = "saveData_" + gameSave.ToString() + ".tone";
        saveData = new JSONClass();
        // Graphics
        saveData["settings"]["graphics"]["fullscreen"].AsBool = true;

        // Audio
        saveData["settings"]["audio"]["masterVolume"].AsFloat = 1f;
        saveData["settings"]["audio"]["sfxVolume"].AsFloat = 1f;
        saveData["settings"]["audio"]["bgmVolume"].AsFloat = 1f;

        // Game Settings
        saveData["settings"]["gameSettings"]["test"].AsInt = 1;

        SaveToFile(fileName);
    }

    public void SetNewLoadGameButtons()
    {
        for(int i = 1; i < 4; i++)
        {
            string fileName = "saveData_" + i + ".tone";
            if (File.Exists(savePath + fileName))
            {
                MenuManager.Instance.SetNewLoadGameButton(i);
            }
        }
    }

    private void LoadSettings(int gameSave)
    {
        string fileName = "saveData_" + gameSave.ToString() + ".tone";
        saveData = (JSONClass)JSONNode.Parse(LoadFromFile(fileName));
        // TODO: Actually put the loaded data somewhere
        Debug.Log(saveData["settings"]["audio"]["masterVolume"].AsFloat);
    }

    private void SaveToFile(string fileName)
    {
        // Writes the data stored in fileName to file
        using (FileStream fs = new FileStream(savePath + fileName, FileMode.Create))
        {
            BinaryWriter fileWriter = new BinaryWriter(fs);
            fileWriter.Write(saveData.ToString(""));
            fs.Close();
        }
    }

    private string LoadFromFile(string fileName)
    {
        // Reads the data stored in fileName
        string data = "";
        using (FileStream fs = new FileStream(savePath + fileName, FileMode.Open))
        {
            BinaryReader fileReader = new BinaryReader(fs);
            data = fileReader.ReadString();
            fs.Close();
        }

        return data;
    }

    public void SetMovingToDoor(string doorName)
    {
        moveToDoorName = doorName;
    }
}