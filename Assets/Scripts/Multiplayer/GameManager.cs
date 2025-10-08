using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI playerListText;
    public TextMeshProUGUI nameFieldText;
    public TextMeshProUGUI roomCodeText;
    public TextMeshProUGUI codeInputField;
    public GameObject codeWrongText;
    public GameObject MainMenu;

    private void Awake() => instance = this;

    public void UpdatePlayerList(Dictionary<string, string> players)
    {
        string playerList = "";

        foreach (KeyValuePair<string, string> pair in players)
        {
            playerList += pair.Value + "\n"; 
        }

        playerListText.text = playerList;
    }


    public void CodeInvalid()
    {
        MainMenu.SetActive(true);
        codeWrongText.SetActive(true);
    }
}
