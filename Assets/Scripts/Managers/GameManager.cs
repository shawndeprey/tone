using UnityEngine;
using SimpleJSON;
using System.IO;

public class GameManager : MonoBehaviour
{
    public bool isPaused { get { return _isPaused; } }
    public int extraLives { get { return _extraLives; } }

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
        GenerateSettingsFile();
    }

    void Start()
    {
        LoadSettings();
        SaveSettings();
    }

    void OnLevelWasLoaded(int level) {
        if(moveToDoorName != null){
            GameObject door = GameObject.Find(moveToDoorName);
            if(door != null){
                GameObject player = GameObject.Find("Player");
                player.transform.position = new Vector2(door.transform.position.x, door.transform.position.y);
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

    // Saving
    public void SaveGame(int gameSave)
    {
        string fileName = "saveData.tone";
        saveData = new JSONClass();
        saveData["save_" + gameSave.ToString()]["testSetting"].AsFloat = 0.5f;

        SaveToFile(fileName);
        Debug.Log("Game Saved");
    }

    public void LoadGame(int gameSave)
    {
        string fileName = "saveData.tone";
        saveData = (JSONClass)JSONNode.Parse(LoadFromFile(fileName));
        Debug.Log(saveData["save_" + gameSave.ToString()]["testSetting"].AsFloat);
    }

    public void SaveSettings()
    {
        string fileName = "settings.tone";
        saveData = new JSONClass();
        saveData["settings"]["testSetting"].AsFloat = 0.5f;

        SaveToFile(fileName);
    }

    private void LoadSettings()
    {
        string fileName = "settings.tone";
        saveData = (JSONClass)JSONNode.Parse(LoadFromFile(fileName));
        Debug.Log(saveData["settings"]["testSetting"].AsFloat);
    }

    private void GenerateSettingsFile()
    {
        // Creates the settings file if it doesn't exist yet
        string fileName = "settings.tone";
        if (!File.Exists(savePath + fileName))
        {
            saveData = new JSONClass();
            saveData["audioSettings"]["masterVolume"].AsFloat = 1f;
            saveData["audioSettings"]["sfxVolume"].AsFloat = 1f;
            saveData["audioSettings"]["bgmVolume"].AsFloat = 1f;

            SaveToFile(fileName);
        }
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