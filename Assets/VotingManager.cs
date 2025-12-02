using UnityEngine;

public class VotingManager : MonoBehaviour
{
    public int[] votes;

    private void Start()
    {
        votes = new int[3];
    }

    public void SendVotes()
    {
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
    }

    public void SetVoteTheme(int value) => votes[0] = value;

    public void SetVoteFun(int value) => votes[1] = value;

    public void SetVoteProfit(int value) => votes[2] = value;
}
