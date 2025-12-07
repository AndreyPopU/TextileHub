using UnityEngine;
using WebSocketSharp;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;

public class WebSocketClient : MonoBehaviour
{
    public static WebSocketClient instance;

    private WebSocket ws;

    public Dictionary<string, string> players = new Dictionary<string, string>();
    public string localPlayerId;
    public string playerName;

    // Pending data (thread-safe → processed in Update)
    private QuestionMessage pendingQuestion;
    private bool hasPendingQuestion = false;

    private GameMessage pendingMessage;
    private bool hasPendingMessage = false;

    private AnswerMessage pendingAnswer;
    private bool hasPendingAnswer = false;

    private ScoreboardMessage pendingScoreboard;
    private bool hasPendingScoreboard = false;

    private GameStartMessage gameStartMessage;
    private bool hasPendingGameStart = false;

    private bool hasPendingPlayerList = false;

    // For join timeout
    private bool hostFoundFlag = false;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        if (hasPendingQuestion)
        {
            QuestionManager.instance.DisplayQuestion(pendingQuestion);
            QuestionManager.instance.playersAnswered = 0;
            hasPendingQuestion = false;
        }

        if (hasPendingGameStart)
        {
            AsyncLoad.instance.LoadScene(gameStartMessage.sceneIndex);
            hasPendingGameStart = false;
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

        if (hasPendingScoreboard)
        {
            DisplayScoreboard(pendingScoreboard);
            hasPendingScoreboard = false;
        }

        if (hasPendingAnswer)
        {
            QuestionManager.instance.playersAnswered++;

            if (QuestionManager.instance.playersAnswered == players.Count)
            {
                Debug.Log($"All players have answered: {QuestionManager.instance.playersAnswered}/{players.Count}");
                // The server will send scoreboard and countdown
            }
            else
            {
                Debug.Log($"Not all players have answered: {QuestionManager.instance.playersAnswered}/{players.Count}");
            }

            hasPendingAnswer = false;
        }
    }

    // -------------------------
    // JOIN SERVER AS A CLIENT
    // -------------------------
    public void JoinServer(TextMeshProUGUI roomCodeText) => JoinServer(roomCodeText.text);

    public void JoinServer(string roomCode)
    {
        LanDiscoveryClient clientDiscovery = gameObject.AddComponent<LanDiscoveryClient>();
        clientDiscovery.roomCode = roomCode;
        Debug.Log("Connecting with room code " + roomCode);

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

        StartCoroutine(WaitForHostDiscovery(5));
    }

    private IEnumerator WaitForHostDiscovery(float timeout)
    {
        float timer = 0f;

        while (timer < timeout)
        {
            if (hostFoundFlag)
            {
                yield return new WaitForSecondsRealtime(1);
                GameManager.instance.loadingScreen.SetActive(false);
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        Debug.LogWarning("Host not found! Room code might be invalid.");
        GameManager.instance.loadingScreen.SetActive(false);
        GameManager.instance.CodeInvalid();
    }

    // -------------------------
    // HANDLE MESSAGES
    // -------------------------
    public void HandleMessage(MessageEventArgs e, GameMessage baseMsg)
    {
        Debug.Log($"Received message type {baseMsg.type}, {baseMsg.text}");

        switch (baseMsg.type.Trim().ToLower())
        {
            case "playerjoined":
                var joinMsg = JsonUtility.FromJson<PlayerJoinMessage>(e.Data);
                players[joinMsg.playerId] = joinMsg.name;
                break;

            case "playerleft":
                var leftMsg = JsonUtility.FromJson<PlayerJoinMessage>(e.Data);
                players.Remove(leftMsg.playerId);
                break;

            case "question":
                var qMsg = JsonConvert.DeserializeObject<QuestionMessage>(e.Data);
                pendingQuestion = qMsg;
                hasPendingQuestion = true;
                break;

            case "gamestart":
                var gameStartMsg = JsonConvert.DeserializeObject<GameStartMessage>(e.Data);
                gameStartMessage = gameStartMsg;
                hasPendingGameStart = true;
                print("Received game start message");
                break;

            case "voting":
                var votingMsg = JsonConvert.DeserializeObject<VotingMessage>(e.Data);
                Debug.Log($"Received voting message: {string.Join(",", votingMsg.votingResults)}");
                break;

            case "design":
                var designMsg = JsonConvert.DeserializeObject<FinalDesignMessage>(e.Data);
                Debug.Log("Received design message");
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

            case "timeup":
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
                QuestionManager.instance.EnableResultScreen(false);
                break;

            case "scoreboard":
                pendingScoreboard = JsonConvert.DeserializeObject<ScoreboardMessage>(e.Data);
                hasPendingScoreboard = true;
                break;

            case "allanswered":
                // Host uses this; clients can ignore or use for visuals
                break;
        }
    }

    private void DisplayScoreboard(ScoreboardMessage msg)
    {
        string text = "Scores:\n";
        foreach (var kv in msg.scores)
        {
            string name = msg.names.ContainsKey(kv.Key) ? msg.names[kv.Key] : "Unknown";
            int score = kv.Value;
            text += $"{name}: {score}\n";
        }

        QuestionManager.instance.resultText.text = text;
        QuestionManager.instance.EnableResultScreen(true);
    }

    // -------------------------
    // SEND JOIN MESSAGE
    // -------------------------
    public void SendJoinMessage(string playerName)
    {
        localPlayerId = System.Guid.NewGuid().ToString();

        var joinMsg = new PlayerJoinMessage
        {
            type = "playerJoined",
            playerId = localPlayerId,
            name = playerName
        };

        SendMessageToServer(JsonUtility.ToJson(joinMsg));
    }

    // -------------------------
    // SEND ANSWER / CUSTOM MESSAGE
    // -------------------------
    public void SendMessageToServer(string json)
    {
        if (ws != null && ws.ReadyState == WebSocketState.Open)
        {
            ws.Send(json);
            Debug.Log("[Client] Sent: " + json);
        }
        else Debug.LogWarning("[Client] Tried to send but WebSocket not connected!");
    }

    private void OnApplicationQuit()
    {
        ws?.Close();
    }
}
