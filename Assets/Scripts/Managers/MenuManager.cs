using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
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
            Initialize();
        }

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (Application.loadedLevelName == GameManager.Instance.mainMenuSceneName)
        {
            SwitchMenu("Main Panel");
            GameManager.Instance.SetNewLoadGameButtons();
        }
    }

    void OnLevelWasLoaded(int level)
    {
        CloseAllMenus();
        if (Application.loadedLevelName == GameManager.Instance.mainMenuSceneName)
        {
            currentPanel = GetPanel("Main Panel");
            currentPanel.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            MenuManager.Instance.Pause();
        }
    }

    public void Pause()
    {
        if (GameManager.Instance.isPausableScene)
        {
            if (MenuManager.Instance.currentPanel.activeSelf)
            {
                StartCoroutine(Unpause());
            }
            else
            {
                GameManager.Instance.Pause();
                MenuManager.Instance.SwitchMenu("Pause Panel");
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

    public void DeleteSaveGame(int gameSave)
    {
        GameManager.Instance.DeleteGame(gameSave);
        GameManager.Instance.SetNewLoadGameButtons();
    }

    public void LoadGame(int gameSave)
    {
        GameManager.Instance.LoadGame(gameSave);
    }

    public void SwitchMenu(string menu)
    {
        if(currentPanel != null)
        {
            currentPanel.SetActive(false);
        }
        currentPanel = GetPanel(menu);
        currentPanel.SetActive(true);

        for (int i = 0; i < currentPanel.GetComponent<UIPanel>().buttons.Count; i++)
        {
            if (currentPanel.GetComponent<UIPanel>().buttons[i].activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(currentPanel.GetComponent<UIPanel>().buttons[i]);
                break;
            }
        }
    }

    public void CloseAllMenus()
    {
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

    public void SetNewLoadGameButton(int gameSave, bool exists)
    {
        newGameButtons[gameSave - 1].SetActive(!exists);
        newGameButtons[gameSave - 1 + 3].SetActive(exists);
        loadGameButtons[gameSave - 1].GetComponent<Button>().interactable = exists;

        for (int i = 0; i < currentPanel.GetComponent<UIPanel>().buttons.Count; i++)
        {
            if (currentPanel.GetComponent<UIPanel>().buttons[i].activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(currentPanel.GetComponent<UIPanel>().buttons[i]);
                break;
            }
        }
    }

    public void SaveIndicator()
    {
        StartCoroutine(DisplaySaveIndicator(3f));
    }

    private void Initialize()
    {
        menuPanels = new Dictionary<string, GameObject>();

        int length = menuList.Count;
        for (int i = 0; i < length; i++)
        {
            menuList[i].SetActive(false);
            menuPanels.Add(menuList[i].name, menuList[i]);
        }

        SwitchMenu("Main Panel");
    }

    private IEnumerator Unpause()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.25f);
        GameManager.Instance.Pause();
        MenuManager.Instance.CloseAllMenus();
    }

    private IEnumerator DisplaySaveIndicator(float seconds)
    {
        GameObject indicator = GetPanel("Save Indicator Panel");
        indicator.SetActive(true);
        
        yield return new WaitForSeconds(seconds);

        indicator.SetActive(false);
    }
}