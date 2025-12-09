using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using TMPro;

// ---------
// Packages
// ---------
#region Packages

[System.Serializable]
public class PlayerMessage
{
    public string type;
    public string playerId;
    public string action;
}

[System.Serializable]
public class PlayerListMessage
{
    public string type;
    public Dictionary<string, string> players;
}

[System.Serializable]
public class AnswerMessage
{
    public string type;
    public string playerId;
    public string answer;
    public bool correct;
}

[System.Serializable]
public class QuestionMessage
{
    public string type;
    public string text;
    public string[] options;
    public string correctAnswer;
}

[System.Serializable]
public class PlayerJoinMessage
{
    public string type;
    public string playerId;
    public string name;
}

[System.Serializable]
public class PlayerData
{
    public string playerId;
    public string name;
    public int score = 0;
}

[System.Serializable]
public class GameMessage
{
    public string type;
    public string text;
}

// Serializable scoreboard container for client
[System.Serializable]
public class ScoreboardMessage
{
    public string type;
    public Dictionary<string, int> scores;
    public Dictionary<string, string> names;
}

#endregion

// -------------------------------------------
// SERVER SIDE
// -------------------------------------------
public class LobbyBehavior : WebSocketBehavior
{
    private static Dictionary<string, string> connectedPlayers = new Dictionary<string, string>();
    private static Dictionary<string, int> playerScores = new Dictionary<string, int>();
    private static HashSet<string> playersAnswered = new HashSet<string>();

    protected override void OnMessage(MessageEventArgs e)
    {
        var baseMsg = JsonUtility.FromJson<GameMessage>(e.Data);

        if (baseMsg.type == "playerJoined")
        {
            var joinMsg = JsonUtility.FromJson<PlayerJoinMessage>(e.Data);

            if (!connectedPlayers.ContainsKey(joinMsg.playerId))
            {
                connectedPlayers.Add(joinMsg.playerId, joinMsg.name);
                playerScores[joinMsg.playerId] = 0;
                Debug.Log($"Player joined: {joinMsg.name}");
            }

            BroadcastPlayerList();
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
                }
            }
        }
        else
        {
            Sessions.Broadcast(e.Data); // rebroadcast other messages
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
        var scoreMsg = new
        {
            type = "scoreboard",
            scores = playerScores,
            names = connectedPlayers
        };

        string json = JsonConvert.SerializeObject(scoreMsg);
        Sessions.Broadcast(json);
        Debug.Log("Scoreboard sent to clients");
    }
}

// -------------------------------------------
// CLIENT SIDE (WebSocketClient)
// -------------------------------------------
public class WebSocketClient : MonoBehaviour
{
    public static WebSocketClient instance;

    WebSocket ws;
    WebSocketServer wss; // server instance

    public Dictionary<string, string> players = new Dictionary<string, string>();
    public Dictionary<string, bool> playerAnswers = new Dictionary<string, bool>(); // Stores received player answers and whether they are correct
    public string localPlayerId;
    public string playerName;

    private Coroutine runningCoroutine;

    // Some messsages don't run on Unity's main thread, so when we detect them they have to go through Update()
    private QuestionMessage pendingQuestion;
    private bool hasPendingQuestion = false;

    private GameMessage pendingMessage;
    private bool hasPendingMessage = false;

    private AnswerMessage pendingAnswer;
    private bool hasPendingAnswer = false;

    private ScoreboardMessage pendingScoreboard;
    private bool hasPendingScoreboard = false;

    private bool hasPendingPlayerList = false;

    private void Awake() => instance = this;

    void Update()
    {
        // turn all of this to one message and move the switch statement here
        if (hasPendingQuestion)
        {
            QuestionManager.instance.DisplayQuestion(pendingQuestion);
            QuestionManager.instance.playersAnswered = 0;
            hasPendingQuestion = false;
        }

        if (hasPendingMessage)
        {
            QuestionManager.instance.timerText.text = pendingMessage.text;
            hasPendingMessage = false;
        }

        if (hasPendingPlayerList)
        {
            GameManager.instance.UpdatePlayerList(players);
            hasPendingPlayerList = false;
        }

        // Handle scoreboard
        if (hasPendingScoreboard)
        {
            DisplayScoreboard(pendingScoreboard);
            hasPendingScoreboard = false;
        }

        if (hasPendingAnswer)
        {
            QuestionManager.instance.playersAnswered++;

            // Store answers with player ID's
            //playerAnswers.Add(pendingAnswer.playerId, pendingAnswer.correct);

            // If all players have answered - Display score
            if (QuestionManager.instance.playersAnswered == players.Count)
            {
                Debug.Log($"All players have answered: {QuestionManager.instance.playersAnswered}/{players.Count}");

                // This function shows the results, score and waits 3 seconds, then asks the next question

                // Send score message to players
            }
            else Debug.Log($"Not all players have answered: {QuestionManager.instance.playersAnswered}/{players.Count}");

            hasPendingAnswer = false;
        }
    }

    // -------------------------
    // HOST A SERVER
    // -------------------------
    public void HostServer()
    {
        // Start web socket server
        wss = new WebSocketServer(3000); // listens on all interfaces
        wss.AddWebSocketService<LobbyBehavior>("/lobby");
        wss.Start();

        // Start LAN discovery host
        LanDiscoveryHost hostDiscovery = gameObject.AddComponent<LanDiscoveryHost>();
        hostDiscovery.roomCode = GenerateRoomCode(); // pass code into discovery script

        Debug.Log("Server started on port 3000, Room code is " + hostDiscovery.roomCode);

        GameManager.instance.roomCodeText.text = hostDiscovery.roomCode;
        JoinServer(hostDiscovery.roomCode);
    }

    public static string GenerateRoomCode(int length = 4)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        System.Random rand = new System.Random();
        char[] code = new char[length];

        for (int i = 0; i < length; i++)
            code[i] = chars[rand.Next(chars.Length)];

        return new string(code);
    }

    // -------------------------
    // JOIN SERVER AS A CLIENT
    // -------------------------
    public void JoinServer(TextMeshProUGUI roomCodeText) => JoinServer(roomCodeText.text);

    private bool hostFoundFlag = false; 

    public void JoinServer(string roomCode)
    {
        LanDiscoveryClient clientDiscovery = gameObject.AddComponent<LanDiscoveryClient>();
        clientDiscovery.roomCode = roomCode;
        print("Connecting with " + roomCode);

        GameManager.instance.loadingScreen.SetActive(true);
        hostFoundFlag = false;

        clientDiscovery.OnHostFound += (ipAddress) =>
        {
            hostFoundFlag = true;
            Debug.Log("Host found at: " + ipAddress);

            ws = new WebSocket($"ws://{ipAddress}:3000/lobby");

            ws.OnOpen += (sender, e) =>
            {
                Debug.Log("Connected to server");
                playerName = GameManager.instance.nameFieldText.text;

                if (playerName.Length <= 1) playerName = "Goldberg";

                SendJoinMessage(playerName);
            };

            ws.OnMessage += (sender, e) =>
            {
                Debug.Log("Message from server: " + e.Data);
                var baseMsg = JsonUtility.FromJson<GameMessage>(e.Data);
                HandleMessage(e, baseMsg);
            };

            ws.ConnectAsync();
        };

        // Start a coroutine to handle timeout
        StartCoroutine(WaitForHostDiscovery(5));
    }

    private IEnumerator WaitForHostDiscovery(float timeout)
    {
        float timer = 0f;

        while (timer < timeout)
        {
            if (hostFoundFlag)
            {
                print("HOST HAS BEEN FOUND IN THE DISCOVERY COROUTINE");
                yield return new WaitForSecondsRealtime(1);
                GameManager.instance.loadingScreen.SetActive(false);
                yield break; // host found, stop waiting
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // Timeout reached and no host found
        Debug.LogWarning("Host not found! Room code might be invalid.");
        GameManager.instance.loadingScreen.SetActive(false);
        GameManager.instance.CodeInvalid();
    }

    public void HandleMessage(MessageEventArgs e, GameMessage baseMsg)
    {
        switch (baseMsg.type.Trim().ToLower())
        {
            case "playerJoined":
                var joinMsg = JsonUtility.FromJson<PlayerJoinMessage>(e.Data);
                players[joinMsg.playerId] = joinMsg.name;
                break;
            case "playerLeft":
                var leftMsg = JsonUtility.FromJson<PlayerJoinMessage>(e.Data);
                players.Remove(leftMsg.playerId);
                break;
            case "question":
                var qMsg = JsonConvert.DeserializeObject<QuestionMessage>(e.Data);
                pendingQuestion = qMsg;
                hasPendingQuestion = true;
                break;
            case "answer":
                var ansMsg = JsonUtility.FromJson<AnswerMessage>(e.Data);
                Debug.Log($"Answer: {ansMsg.playerId} → {ansMsg.answer}. Correct: {ansMsg.correct}");
                pendingAnswer = ansMsg;
                hasPendingAnswer = true;
                break;
            case "timer":
                var timerMsg = JsonUtility.FromJson<GameMessage>(e.Data);
                pendingMessage = timerMsg;
                hasPendingMessage = true;
                break;
            case "timeUp":
                QuestionManager.instance.timerText.text = "Time Up!";
                break;
            case "playerlist":
                var listData = JsonConvert.DeserializeObject<PlayerListMessage>(e.Data);
                players = listData.players;
                Debug.Log($"Player list updated to {listData.players.Count} players");
                hasPendingPlayerList = true;
                break;
            case "countdownstart":
                QuestionManager.instance.DisplayResults();
                break;
            case "countdownend":
                ws.Send(JsonUtility.ToJson(QuestionManager.instance.AskNextQuestion()));
                break;
            case "scoreboard":
                pendingScoreboard = JsonConvert.DeserializeObject<ScoreboardMessage>(e.Data);
                hasPendingScoreboard = true;
                break;
        }
    }

    private void DisplayScoreboard(ScoreboardMessage msg)
    {
        string text = "Scores:\n";
        foreach (var kv in msg.scores)
        {
            string playerName = msg.names[kv.Key];
            int score = kv.Value;
            text += $"{playerName}: {score}\n";
        }

        // Assign to your single text element in UI
        QuestionManager.instance.resultText.text = text;
        QuestionManager.instance.EnableResultScreen(true);

        if (runningCoroutine != null) StopCoroutine(runningCoroutine);
        StartCoroutine(ResultsCountdown(3));
    }

    // -------------------------
    // SEND JOIN MESSAGE
    // -------------------------
    public void SendJoinMessage(string playerName)
    {
        localPlayerId = System.Guid.NewGuid().ToString(); // store it locally

        var joinMsg = new PlayerJoinMessage
        {
            type = "playerJoined",
            playerId = localPlayerId,
            name = playerName
        };

        ws.Send(JsonUtility.ToJson(joinMsg));
    }

    // -------------------------
    // SEND QUESTION
    // -------------------------
    public void SendQuestion()
    {
        if (ws == null || ws.ReadyState != WebSocketState.Open)
        {
            Debug.LogWarning("WebSocket not connected!");
            return;
        }

        QuestionManager.instance.playersAnswered = 0;
        ws.Send(JsonUtility.ToJson(QuestionManager.instance.AskNextQuestion()));
        runningCoroutine = StartCoroutine(QuestionTimer(16f));
    }

    // -------------------------
    // SEND MESSAGE
    // -------------------------
    public void SendMessageToServer(string json)
    {
        if (ws != null && ws.ReadyState == WebSocketState.Open)
        {
            ws.Send(json);
            Debug.Log("Sent: " + json);
        }
        else Debug.LogWarning("Tried to send but WebSocket not connected!");
    }

    IEnumerator ResultsCountdown(float duration)
    {
        // Tell clients to start countdown
        var startMsg = new GameMessage
        {
            type = "countdownStart",
            text = duration.ToString() // send 3 for example
        };

        QuestionManager.instance.DisplayResults();
        wss.WebSocketServices["/lobby"].Sessions.Broadcast(JsonUtility.ToJson(startMsg));

        yield return new WaitForSeconds(duration);

        // After countdown is done, send time up
        var endMsg = new GameMessage
        {
            type = "countdownEnd",
            text = ""
        };

        SendQuestion();
        wss.WebSocketServices["/lobby"].Sessions.Broadcast(JsonUtility.ToJson(endMsg));
    }

    IEnumerator QuestionTimer(float duration)
    {
        float timeLeft = duration;

        while (timeLeft > 0)
        {
            timeLeft -= 1f;

            // Send update to clients once per second
            var timerMsg = new GameMessage
            {
                type = "timer",
                text = timeLeft.ToString()
            };

            wss.WebSocketServices["/lobby"].Sessions.Broadcast(JsonUtility.ToJson(timerMsg));

            yield return new WaitForSeconds(1f);
        }

        // When timer reaches 0, notify clients
        var endMsg = new GameMessage
        {
            type = "timeUp",
            text = ""
        };

        wss.WebSocketServices["/lobby"].Sessions.Broadcast(JsonUtility.ToJson(endMsg));
        runningCoroutine = null;
    }

    void OnApplicationQuit()
    {
        ws?.Close();
        wss?.Stop();
    }
}
