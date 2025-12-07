using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DesignManagerServer : MonoBehaviour
{
    public static DesignManagerServer instance;

    public int finishedPlayers;
    public int playerCount;
    public Transform gridLayout;
    public GameObject serverShirtPrefab;
    public Dictionary<string, ShirtDesign> playerShirts = new Dictionary<string, ShirtDesign>(); // Key - PlayerID; Value - ShirtDesign
    public GameObject nextLevelButton;

    private void Awake() => instance = this;

    private FinalDesignMessage pendingDesignMsg;
    private bool hasPendingReadyMsg = false;

    void Start()
    {
        playerCount = LobbyBehavior.GetConnectedPlayers().Count;

        List<string> playerIDs = LobbyBehavior.GetAllPlayerIds();


        for (int i = 0; i < playerCount; i++)
        {
            GameObject newShirt = Instantiate(serverShirtPrefab);
            newShirt.transform.SetParent(gridLayout);
            playerShirts[playerIDs[i]] = newShirt.GetComponent<ShirtDesign>();
        }
    }

    private void Update()
    {
        if (hasPendingReadyMsg)
        {
            playerShirts[pendingDesignMsg.playerId].SetResults(pendingDesignMsg.designResults);
            finishedPlayers++;
            Debug.Log($"Color set to {playerShirts[pendingDesignMsg.playerId].GetComponent<Image>().color}");

            if (finishedPlayers == playerCount)
                nextLevelButton.SetActive(true);
        }
    }

    public void SetReady(FinalDesignMessage finalDesignMsg)
    {
        pendingDesignMsg = finalDesignMsg;
        Debug.Log($"Setting player ready {playerShirts.ContainsKey(pendingDesignMsg.playerId)}");
        hasPendingReadyMsg = true;
    }

    public void ServerNextMinigame() => FindFirstObjectByType<AsyncLoad>().LoadScene(3);

    public void NextMinigame() => HostNetwork.instance.NextMinigame(4);
}
