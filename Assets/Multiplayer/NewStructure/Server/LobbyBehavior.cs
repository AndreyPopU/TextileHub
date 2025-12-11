using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;
using UnityEngine;
using Newtonsoft.Json;

public class LobbyBehavior : WebSocketBehavior
{
    private static Dictionary<string, string> connectedPlayers = new Dictionary<string, string>(); // Key - PlayerId; Value - name;
    private static Dictionary<string, int> playerScores = new Dictionary<string, int>();
    private static HashSet<string> playersAnswered = new HashSet<string>();

    protected override void OnMessage(MessageEventArgs e)
    {
        var baseMsg = JsonUtility.FromJson<GameMessage>(e.Data);
        Debug.Log($"Receiving message {baseMsg.type}");

        if (baseMsg.type == "playerjoined")
        {
            // Receive new player and broadcast it
            var joinMsg = JsonUtility.FromJson<PlayerJoinMessage>(e.Data);

            if (!connectedPlayers.ContainsKey(joinMsg.playerId))
            {
                connectedPlayers.Add(joinMsg.playerId, joinMsg.name);
                playerScores[joinMsg.playerId] = 0;
                if (GameManager.instance != null)
                {
                    GameManager.instance.connectedPlayers = connectedPlayers;
                    GameManager.instance.hasPendingPlayerList = true;
                }
                Debug.Log($"[Server] Player joined: {joinMsg.name}");
            }

            BroadcastPlayerList();

            // Send only to new player
            var joinedMessage = new GameMessage
            {
                type = "welcome",
                text = $"Welcome, {joinMsg.name}! Your id is {joinMsg.playerId}"
            };

            Send(JsonUtility.ToJson(joinedMessage));
        }
        else if (baseMsg.type == "answer")
        {
            var ansMsg = JsonUtility.FromJson<AnswerMessage>(e.Data);

            if (!playersAnswered.Contains(ansMsg.playerId))
            {
                playersAnswered.Add(ansMsg.playerId);

                if (ansMsg.correct)
                    playerScores[ansMsg.playerId] += 10; // award points

                // All players answered
                if (playersAnswered.Count == connectedPlayers.Count)
                {
                    BroadcastScoreboard();
                    playersAnswered.Clear();

                    // Tell host to start results countdown (optional)
                    var allAnsweredMsg = new GameMessage { type = "allAnswered", text = "" };
                    Sessions.Broadcast(JsonUtility.ToJson(allAnsweredMsg));
                }
            }
        }
        else if (baseMsg.type == "finaldesign")
        {
            var designMsg = JsonUtility.FromJson<FinalDesignMessage>(e.Data);

            for (int i = 0; i < designMsg.designResults.Length; i++)
                Debug.Log($"[Server] Recieved design results: {designMsg.designResults[i]}");

            Debug.Log($"[Server] Received design results: {designMsg.primaryHex}");
            Debug.Log($"[Server] Received design results: {designMsg.secondaryHex}");

            if (DesignManagerServer.instance != null)
            {
                Debug.Log($"DesignManagerServer is not null, setting {designMsg.playerId} to ready");
                DesignManagerServer.instance.SetReady(designMsg);
            }
        }
        else if (baseMsg.type == "voting")
        {
            var votingMsg = JsonUtility.FromJson<VotingMessage>(e.Data);

            for (int i = 0; i < votingMsg.votingResults.Length; i++)
                Debug.Log($"[Server] Recieved voting results: {votingMsg.votingResults[i]}");

            if (VotingManagerServer.instance != null)
            {
                Debug.Log($"VotingManagerServer is not null, setting results of it to the results received");
                VotingManagerServer.instance.AddToResults(votingMsg);
            }
        }
        else
        {
            // rebroadcast other messages as normal
            Sessions.Broadcast(e.Data);
        }
    }

    private void BroadcastPlayerList()
    {
        var listMsg = new PlayerListMessage
        {
            type = "playerlist",
            players = connectedPlayers
        };

        string json = JsonConvert.SerializeObject(listMsg);
        Sessions.Broadcast(json);
    }

    private void BroadcastScoreboard()
    {
        var scoreMsg = new ScoreboardMessage
        {
            type = "scoreboard",
            scores = playerScores,
            names = connectedPlayers
        };

        string json = JsonConvert.SerializeObject(scoreMsg);
        Sessions.Broadcast(json);
        Debug.Log("[Server] Scoreboard sent to clients");
    }

    public static Dictionary<string, string> GetConnectedPlayers()
    {
        return connectedPlayers;
    }

    public static List<string> GetAllPlayerIds()
    {
        return new List<string>(connectedPlayers.Keys);
    }

    public static string GetPlayerName(string playerId)
    {
        return connectedPlayers[playerId];
    }
}
