using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public GameObject currentPanel { get { return _currentPanel; } set { _currentPanel = value; } }
    public List<GameObject> menuList;
    public List<GameObject> newGameButtons, loadGameButtons;

    private GameObject _currentPanel;
    private Dictionary<string, GameObject> menuPanels;

    public static MenuManager Instance { get { return _instance; } }
    private static MenuManager _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        menuPanels = new Dictionary<string, GameObject>();

        int length = menuList.Count;
        for (int i = 0; i < length; i++)
        {
            menuList[i].SetActive(false);
            menuPanels.Add(menuList[i].name, menuList[i]);
        }

        currentPanel = menuList[0];
        if (Application.loadedLevel == 0)
        {
            SwitchMenu("Main Panel");
        }
    }

    void Start()
    {
        if (Application.loadedLevel == 0)
        {
            GameManager.Instance.SetNewLoadGameButtons();
        }
    }

    void OnLevelWasLoaded(int level)
    {
        CloseAllMenus();
        if (level == 0) // Main menu
        {
            currentPanel = GetPanel("Main Panel");
            currentPanel.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (MenuManager.Instance.currentPanel.activeSelf)
            {
                MenuManager.Instance.CloseAllMenus();
            }
            else
            {
                GameManager.Instance.Pause();
                MenuManager.Instance.SwitchMenu("Main Panel");
            }
        }
    }

    public GameObject GetPanel(string menu)
    {
        return menuPanels[menu];
    }

    public void SetPanel(string menu)
    {
        currentPanel = GetPanel(menu);
    }

    public void StartNewGame(int gameSave)
    {
        GameManager.Instance.GenerateNewSaveFile(gameSave);

        Application.LoadLevel(1);
    }

    public void LoadGame(int gameSave)
    {
        GameManager.Instance.LoadGame(gameSave);
    }

    public void SwitchMenu(string menu)
    {
        currentPanel.SetActive(false);
        currentPanel = GetPanel(menu);
        currentPanel.SetActive(true);
    }

    public void CloseAllMenus()
    {
        if (GameManager.Instance.isPaused)
        {
            GameManager.Instance.Pause();
        }

        foreach (KeyValuePair<string, GameObject> panel in menuPanels)
        {
            panel.Value.SetActive(false);
        }
    }

    public void CloseOrSwitchToMain()
    {
        if (Application.loadedLevel == 0)
        {
            SwitchMenu("Main Panel");
        }
        else
        {
            CloseAllMenus();
        }
    }

    public void Respawn()
    {
        GameManager.Instance.RespawnPlayer();
    }

    public void QuitToMainMenu()
    {
        GameManager.Instance.ResetGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetNewLoadGameButton(int gameSave)
    {
        newGameButtons[gameSave - 1].GetComponent<Button>().interactable = false;
        loadGameButtons[gameSave - 1].GetComponent<Button>().interactable = true;
    }
}