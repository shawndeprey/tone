using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public bool isPausableScene { get { return Application.loadedLevelName != GameManager.Instance.mainMenuSceneName; } }
    public bool isPaused { get { return _isPaused; } }
    public int gameSave { get { return _gameSave; } }
    public string currentSection { get { return _currentSection; } }
    public int equippedWeapon { get { return _equippedWeapon; } set { _equippedWeapon = value; } }
    public int equippedItem { get { return _equippedItem; } set { _equippedItem = value; } }

    public string mainMenuSceneName = "main_menu";
    public GameObject playerPrefab;
    public GameObject cameraPrefab;

    private GameObject player;
    private GameObject mainCamera;
    private JSONClass saveData;
    private string savePath;
    private bool _isPaused = false;
    private int _gameSave;
    private string _currentSection = "";
    private string doorName = "";
    private GameObject door;
    private bool createOnce = true;
    private int _equippedWeapon;
    private List<int> currentAmmo;
    private List<int> maxAmmo;
    private int _equippedItem;
    private List<int> currentCharges;
    private List<int> maxCharges;
    private List<bool> unlockedWeapons;
    private List<bool> unlockedItems;

    public static GameManager Instance { get { return _instance; } }
    private static GameManager _instance = null;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnLevelWasLoaded(int level)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
        RoomData roomData = GameObject.FindGameObjectWithTag("RoomData").GetComponent<RoomData>();

        if (level == 0)
        {
            player.GetComponent<Disabler>().Disable();            
        }
        else
        {
            if (doorName != "")
            {
                door = GameObject.Find(doorName);
                if (door != null)
                {
                    player.transform.position = door.transform.position;
                }
            }
            else
            {
                player.transform.position = new Vector3(0, 0, 0);
            }
        }

        if(roomData.isCameraStationary)
        {
            cameraFollow.enabled = true;
            cameraFollow.SetPosition(roomData.cameraPosition);
            cameraFollow.enabled = false;
        }
        else
        {
            cameraFollow.enabled = true;
            cameraFollow.Initialize();
        }

        if (_currentSection != "" && _currentSection != roomData.levelName)
        {
            _currentSection = roomData.levelName;
            MenuManager.Instance.RoomName(_currentSection);
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
        Pause();
        Time.timeScale = 1f;
        doorName = "";
        Destroy(MenuManager.Instance.gameObject);
        Destroy(ProjectileManager.Instance.gameObject);
        player.GetComponent<Disabler>().Disable();
        Application.LoadLevel(mainMenuSceneName);
    }

    public void NewGame(int gameSaveNum)
    {
        GenerateNewSaveFile(gameSaveNum);
        SetGameData();
    }

    // Saving and Loading stuff
    private void SetGameData()
    {
        _equippedWeapon = saveData["playerData"]["equippedWeapon"].AsInt;

        unlockedWeapons.Add(true);
        unlockedWeapons.Add(saveData["playerData"]["basicGun"].AsBool);
        unlockedWeapons.Add(saveData["playerData"]["chargeGun"].AsBool);

        unlockedItems.Add(true);
        unlockedItems.Add(saveData["playerData"]["heatTolerance"].AsBool);
        unlockedItems.Add(saveData["playerData"]["heatInsulator"].AsBool);
        unlockedItems.Add(saveData["playerData"]["swimming"].AsBool);
        unlockedItems.Add(saveData["playerData"]["lightEmitter"].AsBool);
        unlockedItems.Add(saveData["playerData"]["booster"].AsBool);
        unlockedItems.Add(saveData["playerData"]["mindUplink"].AsBool);

        currentAmmo.Add(0);
        currentAmmo.Add(saveData["playerData"]["basicAmmo"].AsInt);
        currentAmmo.Add(saveData["playerData"]["chargeAmmo"].AsInt);

        maxAmmo.Add(0);
        maxAmmo.Add(saveData["playerData"]["basicMax"].AsInt);
        maxAmmo.Add(saveData["playerData"]["chargeMax"].AsInt);

        currentCharges.Add(0);
        currentCharges.Add(saveData["playerData"]["heatToleranceCharges"].AsInt);
        currentCharges.Add(saveData["playerData"]["heatInsulatorCharges"].AsInt);
        currentCharges.Add(saveData["playerData"]["swimmingCharges"].AsInt);
        currentCharges.Add(saveData["playerData"]["lightEmitterCharges"].AsInt);
        currentCharges.Add(saveData["playerData"]["boosterCharges"].AsInt);
        currentCharges.Add(saveData["playerData"]["mindUplinkCharges"].AsInt);

        maxCharges.Add(0);
        maxCharges.Add(saveData["playerData"]["heatToleranceMax"].AsInt);
        maxCharges.Add(saveData["playerData"]["heatInsulatorMax"].AsInt);
        maxCharges.Add(saveData["playerData"]["swimmingMax"].AsInt);
        maxCharges.Add(saveData["playerData"]["lightEmitterMax"].AsInt);
        maxCharges.Add(saveData["playerData"]["boosterMax"].AsInt);
        maxCharges.Add(saveData["playerData"]["mindUplinkMax"].AsInt);
    }

    private void GenerateNewSaveFile(int gameSaveNum)
    {
        _gameSave = gameSaveNum;

        // Creates the settings file if it doesn't exist yet
        string fileName = "saveData_" + _gameSave.ToString() + ".tone";
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
            saveData["playerData"]["equippedWeapon"].AsInt = 0;
            saveData["playerData"]["basicGun"].AsBool = true;
            saveData["playerData"]["chargeGun"].AsBool = true;

            // Ammo
            saveData["playerData"]["basicAmmo"].AsInt = 0;
            saveData["playerData"]["basicMax"].AsInt = 0;
            saveData["playerData"]["chargeAmmo"].AsInt = 3;
            saveData["playerData"]["chargeMax"].AsInt = 3;

            // Items
            saveData["playerData"]["equippedItem"].AsInt = 0;
            saveData["playerData"]["heatTolerance"].AsBool = false;
            saveData["playerData"]["heatInsulator"].AsBool = false;
            saveData["playerData"]["swimming"].AsBool = false;
            saveData["playerData"]["lightEmitter"].AsBool = true;
            saveData["playerData"]["booster"].AsBool = false;
            saveData["playerData"]["mindUplink"].AsBool = false;

            // Charges
            saveData["playerData"]["heatToleranceCharges"].AsInt = 0;
            saveData["playerData"]["heatToleranceMax"].AsInt = 0;
            saveData["playerData"]["heatInsulatorCharges"].AsInt = 0;
            saveData["playerData"]["heatInsulatorMax"].AsInt = 0;
            saveData["playerData"]["swimmingCharges"].AsInt = 0;
            saveData["playerData"]["swimmingMax"].AsInt = 0;
            saveData["playerData"]["lightEmitterCharges"].AsInt = 0;
            saveData["playerData"]["lightEmitterMax"].AsInt = 100;
            saveData["playerData"]["boosterCharges"].AsInt = 0;
            saveData["playerData"]["boosterMax"].AsInt = 0;
            saveData["playerData"]["mindUplinkCharges"].AsInt = 0;
            saveData["playerData"]["mindUplinkMax"].AsInt = 0;

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
        player.GetComponent<Disabler>().Enable();
    }

    public void DeleteGame(int gameSaveNum)
    {
        string fileName = savePath + "saveData_" + gameSaveNum.ToString() + ".tone";

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    public void SaveGame()
    {
        string fileName = "saveData_" + _gameSave.ToString() + ".tone";
        saveData = new JSONClass();

        // Basic player data
        saveData["playerData"]["positionX"].AsFloat = player.transform.position.x;
        saveData["playerData"]["positionY"].AsFloat = player.transform.position.y;
        saveData["playerData"]["maxHealth"].AsInt = player.GetComponent<Player>().maxHealth;
        saveData["playerData"]["health"].AsInt = player.GetComponent<Player>().health;
        saveData["playerData"]["currentLevel"].AsInt = Application.loadedLevel;

        // Weapons
        saveData["playerData"]["equippedWeapon"].AsInt = _equippedWeapon;
        saveData["playerData"]["basicGun"].AsBool = unlockedWeapons[1];
        saveData["playerData"]["chargeGun"].AsBool = unlockedWeapons[2];

        // Ammo
        saveData["playerData"]["basicAmmo"].AsInt = currentAmmo[1];
        saveData["playerData"]["basicMax"].AsInt = maxAmmo[1];
        saveData["playerData"]["chargeAmmo"].AsInt = currentAmmo[2];
        saveData["playerData"]["chargeMax"].AsInt = maxAmmo[2];

        // Items
        saveData["playerData"]["equippedItem"].AsInt = 0;
        saveData["playerData"]["heatTolerance"].AsBool = unlockedItems[1];
        saveData["playerData"]["heatInsulator"].AsBool = unlockedItems[2];
        saveData["playerData"]["swimming"].AsBool = unlockedItems[3];
        saveData["playerData"]["lightEmitter"].AsBool = unlockedItems[4];
        saveData["playerData"]["booster"].AsBool = unlockedItems[5];
        saveData["playerData"]["mindUplink"].AsBool = unlockedItems[6];

        // Charges
        saveData["playerData"]["heatToleranceCharges"].AsInt = currentCharges[1];
        saveData["playerData"]["heatToleranceMax"].AsInt = maxCharges[1];
        saveData["playerData"]["heatInsulatorCharges"].AsInt = currentCharges[2];
        saveData["playerData"]["heatInsulatorMax"].AsInt = maxCharges[2];
        saveData["playerData"]["swimmingCharges"].AsInt = currentCharges[3];
        saveData["playerData"]["swimmingMax"].AsInt = maxCharges[3];
        saveData["playerData"]["lightEmitterCharges"].AsInt = currentCharges[4];
        saveData["playerData"]["lightEmitterMax"].AsInt = maxCharges[4];
        saveData["playerData"]["boosterCharges"].AsInt = currentCharges[5];
        saveData["playerData"]["boosterMax"].AsInt = maxCharges[5];
        saveData["playerData"]["mindUplinkCharges"].AsInt = currentCharges[6];
        saveData["playerData"]["mindUplinkMax"].AsInt = maxCharges[6];

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

    public void LoadGame(int gameSaveNum)
    {
        _gameSave = gameSaveNum;

        string fileName = "saveData_" + gameSaveNum.ToString() + ".tone";
        saveData = (JSONClass)JSONNode.Parse(LoadFromFile(fileName));

        Application.LoadLevel(saveData["playerData"]["currentLevel"].AsInt);
        player.GetComponent<Disabler>().Enable();

        float posX = saveData["playerData"]["positionX"].AsFloat;
        float posY = saveData["playerData"]["positionY"].AsFloat;

        player.transform.position = new Vector3(posX, posY, 0);
        player.GetComponent<Player>().SetMaxHealth(saveData["playerData"]["maxHealth"].AsInt);
        player.GetComponent<Player>().health = saveData["playerData"]["health"].AsInt;
        player.GetComponent<Disabler>().Enable();

        SetGameData();
    }

    public void SaveSettings(int gameSaveNum)
    {
        string fileName = "saveData_" + gameSaveNum.ToString() + ".tone";
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
        for (int i = 1; i < 4; i++)
        {
            string fileName = "saveData_" + i + ".tone";

            MenuManager.Instance.SetNewLoadGameButton(i, File.Exists(savePath + fileName));
        }
    }

    public int GetCurrentAmmo(int weaponIndex)
    {
        return currentAmmo[weaponIndex];
    }

    public int GetMaxAmmo(int weaponIndex)
    {
        return maxAmmo[weaponIndex];
    }

    public void SetCurrentAmmo(int weaponIndex, int amount)
    {
        currentAmmo[weaponIndex] = amount;
    }

    public void SetMaxAmmo(int weaponIndex, int amount)
    {
        maxAmmo[weaponIndex] = amount;
    }

    public int GetCurrentCharges(int itemIndex)
    {
        return currentCharges[itemIndex];
    }

    public int GetMaxCharges(int itemIndex)
    {
        return maxCharges[itemIndex];
    }

    public void SetCurrentCharges(int itemIndex, int amount)
    {
        currentCharges[itemIndex] = amount;
    }

    public void SetMaxCharges(int itemIndex, int amount)
    {
        maxCharges[itemIndex] = amount;
    }

    private void Initialize()
    {
        currentAmmo = new List<int>();
        maxAmmo = new List<int>();
        currentCharges = new List<int>();
        maxCharges = new List<int>();
        unlockedWeapons = new List<bool>();
        unlockedItems = new List<bool>();

        if (createOnce)
        {
            createOnce = false;
            player = (GameObject)Instantiate(playerPrefab);
            mainCamera = (GameObject)Instantiate(cameraPrefab);
        }

        if (Application.loadedLevelName == mainMenuSceneName)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Disabler>().Disable();
        }

        savePath = Application.persistentDataPath + "/";
    }

    private void LoadSettings(int gameSaveNum)
    {
        string fileName = "saveData_" + gameSaveNum.ToString() + ".tone";
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