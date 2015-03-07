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
            gameObject.GetComponent<HealthDisplay>().healthPanel.SetActive(false);

            ItemDisplay[] itemDisplays = gameObject.GetComponents<ItemDisplay>();
            for (int i = 0; i < itemDisplays.Length; i++)
            {
                itemDisplays[i].displayPanel.SetActive(false);
            }

            currentPanel = GetPanel("Main Panel");
            currentPanel.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<HealthDisplay>().healthPanel.SetActive(true);

            ItemDisplay[] itemDisplays = gameObject.GetComponents<ItemDisplay>();
            for (int i = 0; i < itemDisplays.Length; i++)
            {
                itemDisplays[i].displayPanel.SetActive(true);
            }
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

    public void ReloadGame()
    {
        GameManager.Instance.LoadGame(GameManager.Instance.gameSave);
    }

    public void SwitchMenu(string menu)
    {
        if (currentPanel != null)
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

        GameObject.Find("menuOverlay").GetComponent<Renderer>().enabled = true;
    }

    public void CloseAllMenus()
    {
        foreach (KeyValuePair<string, GameObject> panel in menuPanels)
        {
            panel.Value.SetActive(false);
        }
        GameObject.Find("menuOverlay").GetComponent<Renderer>().enabled = false;
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
        StartCoroutine(DisplaySaveIndicator(1.75f));
    }

    public void RoomName(string roomName)
    {
        StartCoroutine(FadeRoomName());
    }

    public ItemDisplay GetWeaponDisplay()
    {
        ItemDisplay[] itemDisplays = gameObject.GetComponents<ItemDisplay>();
        return itemDisplays[0];
    }

    public ItemDisplay GetItemDisplay()
    {
        ItemDisplay[] itemDisplays = gameObject.GetComponents<ItemDisplay>();
        return itemDisplays[1];
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

        if (Application.loadedLevelName == GameManager.Instance.mainMenuSceneName)
        {
            gameObject.GetComponent<HealthDisplay>().healthPanel.SetActive(false);

            ItemDisplay[] itemDisplays = gameObject.GetComponents<ItemDisplay>();
            for (int i = 0; i < itemDisplays.Length; i++)
            {
                itemDisplays[i].displayPanel.SetActive(false);
            }
        }
        else
        {
            gameObject.GetComponent<HealthDisplay>().healthPanel.SetActive(true);

            ItemDisplay[] itemDisplays = gameObject.GetComponents<ItemDisplay>();
            for (int i = 0; i < itemDisplays.Length; i++)
            {
                itemDisplays[i].displayPanel.SetActive(true);
            }
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

    private IEnumerator FadeRoomName()
    {
        GameObject room = GetPanel("Room Name Panel");
        GameObject text = room.transform.FindChild("Text").gameObject;

        yield return new WaitForSeconds(0.25f);

        Text roomText = text.GetComponent<Text>();
        roomText.text = GameManager.Instance.currentSection;

        Image panel = room.GetComponent<Image>();
        float panelAlpha = panel.color.a;

        roomText.color = new Color(roomText.color.r, roomText.color.g, roomText.color.b, 0f);
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0f);

        room.SetActive(true);

        //yield return new WaitForSeconds(1.75f);

        // Fade in
        for (float i = 1f; i <= 20f; i++)
        {
            yield return new WaitForEndOfFrame();
            float alpha = Mathf.Lerp(roomText.color.a, 1f, Time.deltaTime * 10);
            roomText.color = new Color(roomText.color.r, roomText.color.g, roomText.color.b, alpha);
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panelAlpha * alpha);
        }

        yield return new WaitForSeconds(1.25f);

        // Fade out
        for (float i = 1f; i <= 20f; i++ )
        {
            yield return new WaitForEndOfFrame();
            float alpha = Mathf.Lerp(roomText.color.a, 0f, Time.deltaTime * 10);
            roomText.color = new Color(roomText.color.r, roomText.color.g, roomText.color.b, alpha);
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panelAlpha * alpha);
        }
        room.SetActive(false);
        roomText.color = new Color(roomText.color.r, roomText.color.g, roomText.color.b, 1f);
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panelAlpha);
    }
}