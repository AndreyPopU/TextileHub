using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI playerListText;
    public TextMeshProUGUI nameFieldText;

    private void Awake()
    {
        instance = this;
    }

    public void UpdatePlayerList(Dictionary<string, string> players)
    {
        string playerList = "";

        foreach (KeyValuePair<string, string> pair in players)
        {
            playerList += pair.Value + "\n"; 
        }

        playerListText.text = playerList;
    }
}
