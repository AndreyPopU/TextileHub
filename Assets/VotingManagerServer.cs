using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VotingManagerServer : MonoBehaviour
{
    public static VotingManagerServer instance;

    public int currentIndex;
    public ShirtResults[] shirts;
    public DesignChanger displayShirt;

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

    private VotingMessage pendingVotingMsg;
    private bool hasPendingVotingMessage = false;

    private void Awake() => instance = this;

    void Start()
    {
        // Find all ShirtDesigns from last scene
        results = new int[3];
        shirts = FindObjectsByType<ShirtResults>(FindObjectsSortMode.None);
        timerSlider.maxValue = timeLeft;
        pitchingPanel.SetActive(false);
        presentingPlayerText.text = LobbyBehavior.GetPlayerName(LobbyBehavior.GetAllPlayerIds()[currentIndex]);
    }

    void Update()
    {
        if (hasPendingVotingMessage)
        {
            for (int i = 0; i < pendingVotingMsg.votingResults.Length; i++)
                results[i] += pendingVotingMsg.votingResults[i];

            playersVoted++;

            if (playersVoted < LobbyBehavior.GetConnectedPlayers().Count) // All players have voted
            {
                nextShirtButton.SetActive(true);
                votingRoundManagerServer.started = false;
                timerSlider.transform.parent.gameObject.SetActive(false);
                
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

            return;
        }

        timeLeft -= Time.deltaTime;
        timerSlider.value = timeLeft;
    }

    public void DisplayShirt()
    {
        // If all shirts have been cycled through, move on to the next minigame
        if (currentIndex >= shirts.Length) return;

        displayShirt.SetCollar(shirts[currentIndex].results[0]);
        displayShirt.SetSleeves(shirts[currentIndex].results[1]);
        displayShirt.SetHem(shirts[currentIndex].results[2]);

        displayShirt.GetComponent<DesignFabric>().SetFabric(shirts[currentIndex].results[3]);
        displayShirt.SetMaterial(shirts[currentIndex].results[4]);
        
        displayShirt.ResultSetPrimaryColor(shirts[currentIndex].primaryHex);
        displayShirt.ResultSetSecondaryColor(shirts[currentIndex].secondaryHex);

        currentIndex++;
    }

    public void AddToResults(VotingMessage votingMsg)
    {
        hasPendingVotingMessage = true;
        pendingVotingMsg = votingMsg;
    }

    public void ServerNextMinigame() => FindFirstObjectByType<AsyncLoad>().LoadScene(3);

    public void NextMinigame() => HostNetwork.instance.NextMinigame(4);
}
