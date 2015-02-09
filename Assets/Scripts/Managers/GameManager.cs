using UnityEngine;
using SimpleJSON;
using System.IO;

public class GameManager : MonoBehaviour
{
    public bool isPausableScene { get { return Application.loadedLevelName != GameManager.Instance.mainMenuSceneName; } }
    public bool isPaused { get { return _isPaused; } }
    public int extraLives { get { return _extraLives; } }
    public string mainMenuSceneName = "main_menu";
    public GameObject player;
    public GameObject camera;

    private JSONClass saveData;
    private string savePath;
    private bool _isPaused = false;
    private int _extraLives = 3;
    private string doorName = "";
    private GameObject door;

    public static GameManager Instance { get { return _instance; } }
    private static GameManager _instance = null;

    void Awake()
    {
        if (Application.loadedLevelName == mainMenuSceneName)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player.SetActive(false);
        }

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

    void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player.SetActive(false);

            camera = Camera.main.gameObject;
            Camera.main.GetComponent<CameraFollow>().enabled = false;
        }
        else
        {
            Camera.main.GetComponent<CameraFollow>().enabled = true;
            if (doorName != "")
            {
                door = GameObject.Find(doorName);
                player.transform.position = door.transform.position;
            }
        }
    }

    public void SetDoor(string doorName)
    {
        GameManager.Instance.doorName = doorName;
    }

    public void Pause()
    {
        _isPaused = !_isPaused;
        if (_isPaused)
        {
            Time.timeScale = 0f;
        }
    }

    public void ResetGame()
    {
        MenuManager.Instance.CloseAllMenus();
        MenuManager.Instance.SetPanel("Main Panel");
        Destroy(player);
        Destroy(camera);
        Application.LoadLevel(mainMenuSceneName);
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
            // Basic player data
            saveData["playerData"]["positionX"].AsFloat = player.transform.position.x;
            saveData["playerData"]["positionY"].AsFloat = player.transform.position.y;
            saveData["playerData"]["maxHealth"].AsInt = player.GetComponent<Player>().maxHealth;
            saveData["playerData"]["health"].AsInt = player.GetComponent<Player>().health;
            saveData["playerData"]["currentLevel"].AsInt = 1;

            // Weapons
            saveData["playerData"]["equippedWeapon"] = "";
            saveData["playerData"]["basicGun"].AsBool = false;
            saveData["playerData"]["sprayGun"].AsBool = false;
            saveData["playerData"]["machineGun"].AsBool = false;

            // Ammo
            saveData["playerData"]["equippedBulletType"] = "";
            saveData["playerData"]["basicShot"].AsBool = false;
            saveData["playerData"]["chargeShot"].AsBool = false;
            saveData["playerData"]["boltShot"].AsBool = false;
            saveData["playerData"]["rockBreaker"].AsBool = false;

            // Items
            saveData["playerData"]["equippedItem"] = "";
            saveData["playerData"]["heatTolerance"].AsBool = false;
            saveData["playerData"]["heatInsulator"].AsBool = false;
            saveData["playerData"]["swimming"].AsBool = false;
            saveData["playerData"]["lightEmitter"].AsBool = false;
            saveData["playerData"]["booster"].AsBool = false;
            saveData["playerData"]["mindUplink"].AsBool = false;

            // Boss states
            saveData["gameEvents"]["firstBoss"].AsBool = false;
            saveData["gameEvents"]["secondBoss"].AsBool = false;
            saveData["gameEvents"]["thirdBoss"].AsBool = false;
            saveData["gameEvents"]["fourthBoss"].AsBool = false;

            // NPC states
            saveData["gameEvents"]["firstNPCScene"].AsBool = false;
            saveData["gameEvents"]["secondNPCScene"].AsBool = false;
            saveData["gameEvents"]["thirdNPCScene"].AsBool = false;
            saveData["gameEvents"]["fourthNPCScene"].AsBool = false;

            // Game stats
            saveData["gameStats"]["currentScore"].AsInt = 0;
            saveData["gameStats"]["enemiesKilled"].AsInt = 0;
            saveData["gameStats"]["distanceTravelled"].AsFloat = 0.0f;

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

        player.SetActive(true);
    }

    public void SaveGame(int gameSave)
    {
        string fileName = "saveData_" + gameSave.ToString() + ".tone";
        saveData = new JSONClass();

        // Basic player data
        saveData["playerData"]["positionX"].AsFloat = player.transform.position.x;
        saveData["playerData"]["positionY"].AsFloat = player.transform.position.y;
        saveData["playerData"]["maxHealth"].AsInt = player.GetComponent<Player>().maxHealth;
        saveData["playerData"]["health"].AsInt = player.GetComponent<Player>().health;
        saveData["playerData"]["currentLevel"].AsInt = Application.loadedLevel;

        // Weapons
        saveData["playerData"]["equippedWeapon"] = "";
        saveData["playerData"]["basicGun"].AsBool = false;
        saveData["playerData"]["sprayGun"].AsBool = false;
        saveData["playerData"]["machineGun"].AsBool = false;

        // Ammo
        saveData["playerData"]["equippedBulletType"] = "";
        saveData["playerData"]["basicShot"].AsBool = false;
        saveData["playerData"]["chargeShot"].AsBool = false;
        saveData["playerData"]["boltShot"].AsBool = false;
        saveData["playerData"]["rockBreaker"].AsBool = false;

        // Items
        saveData["playerData"]["equippedItem"] = "";
        saveData["playerData"]["heatTolerance"].AsBool = false;
        saveData["playerData"]["heatInsulator"].AsBool = false;
        saveData["playerData"]["swimming"].AsBool = false;
        saveData["playerData"]["lightEmitter"].AsBool = false;
        saveData["playerData"]["booster"].AsBool = false;
        saveData["playerData"]["mindUplink"].AsBool = false;

        // Boss states
        saveData["gameEvents"]["firstBoss"].AsBool = false;
        saveData["gameEvents"]["secondBoss"].AsBool = false;
        saveData["gameEvents"]["thirdBoss"].AsBool = false;
        saveData["gameEvents"]["fourthBoss"].AsBool = false;

        // NPC states
        saveData["gameEvents"]["firstNPCScene"].AsBool = false;
        saveData["gameEvents"]["secondNPCScene"].AsBool = false;
        saveData["gameEvents"]["thirdNPCScene"].AsBool = false;
        saveData["gameEvents"]["fourthNPCScene"].AsBool = false;

        // Game stats
        saveData["gameStats"]["currentScore"].AsInt = 0;
        saveData["gameStats"]["enemiesKilled"].AsInt = 0;
        saveData["gameStats"]["distanceTravelled"].AsFloat = 0.0f;

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

    public void LoadGame(int gameSave)
    {
        string fileName = "saveData_" + gameSave.ToString() + ".tone";
        saveData = (JSONClass)JSONNode.Parse(LoadFromFile(fileName));
        
        Application.LoadLevel(saveData["playerData"]["currentLevel"].AsInt);
        player.SetActive(true);

        float posX = saveData["playerData"]["positionX"].AsFloat;
        float posY = saveData["playerData"]["positionY"].AsFloat;
        
        player.transform.position = new Vector3(posX, posY, 0);
        player.GetComponent<Player>().SetMaxHealth(saveData["playerData"]["maxHealth"].AsInt);
        player.GetComponent<Player>().health = saveData["playerData"]["health"].AsInt;
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
}