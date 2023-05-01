using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public UnityEngine.UI.Button startButton;
    public UnityEngine.UI.Button exitButton;
    public UnityEngine.UI.Button backToStartButton;
    public UnityEngine.UI.Button createButton;  

    public GameObject startMenuPanel;
    public GameObject selectPlayerPanel;
    public GameObject createPlayerPanel;

    public GameObject playerPrefab;
    public GameObject createPlayerprefab;

    public TMP_InputField playerName;

    private int playerIndex = 0;
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(Exit);
        backToStartButton.onClick.AddListener(BackToStartMenu);
        createButton.onClick.AddListener(CreatePlayer);
    }

    void Exit()
    {
        Application.Quit();
    }

    void StartGame()
    {
        startMenuPanel.SetActive(false);
        selectPlayerPanel.SetActive(true);
        ShowPlayersAccount();
    }

    void BackToStartMenu()
    {
        startMenuPanel.SetActive(true);
        selectPlayerPanel.SetActive(false);
    }

    void ToCreatePlayerWindow()
    {
        selectPlayerPanel.SetActive(false);
        createPlayerPanel.SetActive(true) ;
    }

    void ShowPlayersAccount()
    {
        playerIndex= 0;
        for (int i = 0; i < 3; i++)
        {
            if (GameData.CheckData("player" + i))
            {
                GameObject player = Instantiate(playerPrefab, selectPlayerPanel.transform);
                player.SetActive(true);
                player.transform.SetParent(selectPlayerPanel.transform, false);
                player.transform.localPosition = new Vector3(-2, 50 - 80 * i);
            }
            else
            {
                GameObject newObject = Instantiate(createPlayerprefab);
                print(newObject.GetType());
                UnityEngine.UI.Button button = newObject.GetComponent<UnityEngine.UI.Button>();
                button.onClick.AddListener(ToCreatePlayerWindow);
                newObject.transform.SetParent(selectPlayerPanel.transform, false);
                newObject.transform.localPosition = new Vector3(-2, 50 - 80 * i);
                playerIndex = i;
                break;
            }
        }
    }

    void CreatePlayer()
    {
        GameData.Save<PlayerInfo>("player" + playerIndex, new PlayerInfo(playerName.text));
        createPlayerPanel.SetActive(false);
        StartGame();
    }
}
