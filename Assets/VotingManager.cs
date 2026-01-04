using TMPro;
using UnityEngine;

public class VotingManager : MonoBehaviour
{
    public static VotingManager instance;

    public GameObject votingPanel, votingButton, gameStateText;
    public bool finishedVoting;
    public int[] votes;

    private void Awake() => instance = this;

    private void Start()
    {
        votes = new int[3];
    }

    public void SendVotes()
    {
        if (finishedVoting) return;

        if (WebSocketClient.instance != null)
        {
            var voteMessage = new VotingMessage
            {
                type = "voting",
                votingResults = votes,
            };

            string json = JsonUtility.ToJson(voteMessage);
            WebSocketClient.instance.SendMessageToServer(json);
        }

        AllowVotes(false);
        gameStateText.GetComponent<TextMeshProUGUI>().text = "Waiting for others to vote.";
        finishedVoting = true;
    }

    public void AllowVotes(bool allowed)
    {
        votingPanel.SetActive(allowed);
        votingButton.SetActive(allowed);
        gameStateText.SetActive(!allowed);
    }

    public void SetVoteTheme(int value) => votes[0] = value;

    public void SetVoteFun(int value) => votes[1] = value;

    public void SetVoteProfit(int value) => votes[2] = value;
}
