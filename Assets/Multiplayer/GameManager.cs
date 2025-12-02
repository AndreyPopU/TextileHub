using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Lobby")]
    public TextMeshProUGUI nameFieldText;
    public TextMeshProUGUI roomCodeText;
    public TextMeshProUGUI codeInputField;
    public GameObject codeWrongText;

    public TextMeshProUGUI[] playerSeatNames;

    [Header("Menus")]
    public GameObject MainMenu;
    public GameObject loadingScreen;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void UpdatePlayerList(Dictionary<string, string> players)
    {
        int index = 0;

        foreach (KeyValuePair<string, string> pair in players)
            playerSeatNames[index++].text = pair.Value;
    }   

    public void CodeInvalid()
    {
        MainMenu.SetActive(true);
        codeWrongText.SetActive(true);
    }

    public void StartGame()
    {
        if (WebSocketClient.instance != null)
        {
            var gameStartMessage = new GameStartMessage
            {
                type = "gameStart",
                sceneIndex = 2,
            };

            string json = JsonUtility.ToJson(gameStartMessage);
            WebSocketClient.instance.SendMessageToServer(json);
        }
    }
}
