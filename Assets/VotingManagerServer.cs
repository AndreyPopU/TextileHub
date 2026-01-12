using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class VotingManagerServer : MonoBehaviour
{
    public static VotingManagerServer instance;

    public int currentIndex;
    public ShirtResults[] shirts;
    public DesignChanger displayShirt;
    public DesignChanger winningShirt;

    public GameObject nextShirtButton;
    public TextMeshProUGUI presentingPlayerText;
    public int playersVoted;
    public int[] results;

    [Header("Menus")]
    public GameObject getReadyPanel;
    public GameObject pitchingPanel;
    public GameObject resultsPanel;

    [Header("Timer")]
    public VotingRoundManagerServer votingRoundManagerServer;
    public bool started = true;
    public float timeLeft = 10;
    public Slider timerSlider;
    public Slider pitchingTimerSlider;

    private VotingMessage pendingVotingMsg;
    private bool hasPendingVotingMessage = false;

    private void Awake() => instance = this;

    void Start()
    {
        // Find all ShirtDesigns from last scene then set the order to match the order of the players
        ShirtResults[] unfilteredShirts = FindObjectsByType<ShirtResults>(FindObjectsSortMode.None);
        List<string> playerIds = LobbyBehavior.GetAllPlayerIds();
        shirts = new ShirtResults[unfilteredShirts.Length]; // Initialize the array

        for (int i = 0; i < unfilteredShirts.Length; i++)
        {
            // For each unfiltered shirt, find the owning player and assign him
            for (int j = 0; j < playerIds.Count; j++)
            {
                // If unfiltered shirt matches the player assign it to shirts
                if (unfilteredShirts[i].playerId == playerIds[j])
                {
                    shirts[j] = unfilteredShirts[i];
                    break;
                }
            }
        }

        results = new int[shirts.Length];
        print("Length of results is " + results.Length);
        timerSlider.maxValue = timeLeft;
        pitchingPanel.SetActive(false);
        presentingPlayerText.text = LobbyBehavior.GetPlayerName(LobbyBehavior.GetAllPlayerIds()[currentIndex]);
    }

    void Update()
    {
        if (hasPendingVotingMessage)
        {
            // Add voting results to the overall results
            for (int i = 0; i < pendingVotingMsg.votingResults.Length; i++)
                results[currentIndex] += pendingVotingMsg.votingResults[i];

            playersVoted++;

            if (playersVoted >= LobbyBehavior.GetConnectedPlayers().Count - 1) // All players (but the one pitching) have voted
            {
                nextShirtButton.SetActive(true);
                votingRoundManagerServer.started = false;
                timerSlider.transform.parent.gameObject.SetActive(false);
                pitchingTimerSlider.transform.parent.gameObject.SetActive(false);
                print("All players have voted");
            }    
            print("Added results");

            hasPendingVotingMessage = false;
        }

        if (!started) return;

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            started = false;
            getReadyPanel.SetActive(false);
            pitchingPanel.SetActive(true);
            votingRoundManagerServer.started = true;
            DisplayShirt();

            // Allow players to vote
            HostNetwork.instance.AllowVoting(LobbyBehavior.GetAllPlayerIds()[currentIndex]);
            return;
        }

        timeLeft -= Time.deltaTime;
        timerSlider.value = timeLeft;
    }

    public void PitchNextPlayer()
    {
        currentIndex++;

        // If all shirts have been cycled through, move on to the next minigame
        if (currentIndex >= shirts.Length)
        {
            pitchingPanel.SetActive(false);
            resultsPanel.SetActive(true);

            // Calculate results
            int winningIndex = 0;
            int highestResult = 0;

            for (int i = 0; i < results.Length; i++)
            {
                if (highestResult < results[i])
                {
                    highestResult = results[i];
                    winningIndex = i;
                }
            }

            // Enable the winning shirt
            winningShirt.SetCollar(shirts[winningIndex].results[0]);
            winningShirt.SetSleeves(shirts[winningIndex].results[1]);
            winningShirt.SetHem(shirts[winningIndex].results[2]);

            winningShirt.GetComponent<DesignFabric>().SetFabric(shirts[winningIndex].results[3]);
            winningShirt.GetComponent<DesignPattern>().SetPattern(shirts[winningIndex].results[4]); // The underlying code has been changed by Jeremy so this line no longer works

            winningShirt.ResultSetPrimaryColor(shirts[winningIndex].primaryHex);
            winningShirt.ResultSetSecondaryColor(shirts[winningIndex].secondaryHex);

            return;
        }

        nextShirtButton.SetActive(false);
        pitchingPanel.SetActive(false);
        getReadyPanel.SetActive(true);
        timeLeft = 10;
        timerSlider.value = timeLeft;
        timerSlider.transform.parent.gameObject.SetActive(true);
        presentingPlayerText.text = LobbyBehavior.GetPlayerName(LobbyBehavior.GetAllPlayerIds()[currentIndex]);
        started = true;
    }

    public void DisplayShirt()
    {
        displayShirt.SetCollar(shirts[currentIndex].results[0]);
        displayShirt.SetSleeves(shirts[currentIndex].results[1]);
        displayShirt.SetHem(shirts[currentIndex].results[2]);

        displayShirt.GetComponent<DesignFabric>().SetFabric(shirts[currentIndex].results[3]); 
        displayShirt.GetComponent<DesignPattern>().SetPattern(shirts[currentIndex].results[4]); // The underlying code has been changed by Jeremy so this line no longer works
        
        displayShirt.ResultSetPrimaryColor(shirts[currentIndex].primaryHex);
        displayShirt.ResultSetSecondaryColor(shirts[currentIndex].secondaryHex);
    }

    public void AddToResults(VotingMessage votingMsg)
    {
        hasPendingVotingMessage = true;
        pendingVotingMsg = votingMsg;
    }

    public void ServerNextMinigame() => FindFirstObjectByType<AsyncLoad>().LoadScene(9);

    public void NextMinigame() => HostNetwork.instance.AssignRoles(); // 5 6 7 8 are the scenes the clients should load
}
