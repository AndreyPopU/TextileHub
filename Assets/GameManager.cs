using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI playerListText;

    private void Awake()
    {
        instance = this;
    }

    public void UpdatePlayerList(Dictionary<string, string> players)
    {
        string playerList = "";

        foreach (KeyValuePair<string, string> pair in players)
        {
            playerList += pair.Key + "\n"; // Change this to name instead of ID when players can input their name
        }

        playerListText.text = playerList;
    }
}
