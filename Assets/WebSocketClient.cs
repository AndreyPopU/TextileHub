using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;   // needed for server
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;

//
// Packages
//
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
    public string type;     // e.g., "playerJoined"
    public string playerId; // assigned by the server
    public string name;     // chosen by the player
}

[System.Serializable]
public class GameMessage
{
    public string type;
    public string text;
}

//
// WebSocket host behaviour
//
public class LobbyBehavior : WebSocketBehavior
{
    // Static so all instances share the same list
    private static Dictionary<string, string> connectedPlayers = new Dictionary<string, string>();

    protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
    {
        var msg = JsonUtility.FromJson<PlayerJoinMessage>(e.Data);

        if (msg.type == "playerJoined")
        {
            if (!connectedPlayers.ContainsKey(msg.playerId))
            {
                connectedPlayers.Add(msg.playerId, msg.name);
                Debug.Log($"Player joined: {msg.name} ({msg.playerId})");
            }

            var listMsg = new PlayerListMessage
            {
                type = "playerlist",
                players = connectedPlayers
            };

            // Use Newtonsoft for serialization (JsonUtility can’t do dictionaries)
            string json = JsonConvert.SerializeObject(listMsg);
            Sessions.Broadcast(json);
        }
        else
        {
            // rebroadcast other messages as normal
            Sessions.Broadcast(e.Data);
        }
    }

    protected override void OnClose(CloseEventArgs e)
    {
        // Find and remove disconnected player
        string keyToRemove = null;
        foreach (var kv in connectedPlayers)
        {
            if (kv.Key == ID) // or map SessionID ↔ playerId if you prefer
            {
                keyToRemove = kv.Key;
                break;
            }
        }

        if (keyToRemove != null)
        {
            Debug.Log($"Player left: {connectedPlayers[keyToRemove]}");
            connectedPlayers.Remove(keyToRemove);

            var listMsg = new PlayerListMessage
            {
                type = "playerlist",
                players = connectedPlayers
            };

            string json = JsonConvert.SerializeObject(listMsg);
            Sessions.Broadcast(json);
        }

        base.OnClose(e);
    }
}


public class WebSocketClient : MonoBehaviour
{
    public static WebSocketClient instance;

    WebSocket ws;
    WebSocketServer wss; // server instance

    public Dictionary<string, string> players = new Dictionary<string, string>();
    public string localPlayerId;
    public string playerName;

    // Some messsages don't run on Unity's main thread, so when we detect them they have to go through Update()
    private QuestionMessage pendingQuestion;
    private bool hasPendingQuestion = false;

    private GameMessage pendingMessage;
    private bool hasPendingMessage = false;

    private AnswerMessage pendingAnswer;
    private bool hasPendingAnswer = false;

    private bool hasPendingPlayerList = false;

    private void Awake() => instance = this;    

    void Update()
    {
        if (hasPendingQuestion)
        {
            QuestionManager.instance.DisplayQuestion(pendingQuestion);
            hasPendingQuestion = false;
        }

        if (hasPendingMessage)
        {
            QuestionManager.instance.timerText.text = pendingMessage.text;
            hasPendingMessage = false;
        }

        if (hasPendingPlayerList)
        {
            print("here");
            GameManager.instance.UpdatePlayerList(players);
            hasPendingPlayerList = false;
        }

        if (hasPendingAnswer)
        {
            // If all players have answered - Display score

            // Else display waiting screen

            hasPendingAnswer = false;
        }
    }

    // -------------------------
    // HOST A SERVER
    // -------------------------
    public void HostServer()
    {
        wss = new WebSocketServer(3000); // listens on all interfaces
        wss.AddWebSocketService<LobbyBehavior>("/lobby");
        wss.Start();
        Debug.Log("Server started on port 3000");

        // -------------------------
        // CONNECT AS A CLIENT
        // -------------------------
        Connect();
    }

    // -------------------------
    // CONNECT AS A CLIENT
    // -------------------------
    public void Connect()
    {
        // IMPORTANT: if hosting locally, use your LAN IP here, e.g., ws://192.168.2.3:3000/lobby
        ws = new WebSocket("ws://192.168.2.3:3000/lobby");

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Connected to server");

            // Send join message right after connecting
            playerName = GameManager.instance.nameFieldText.text;
            SendJoinMessage(playerName);
        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message from server: " + e.Data);

            var baseMsg = JsonUtility.FromJson<GameMessage>(e.Data);

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

                    Debug.Log($"Buffered question: {pendingQuestion}");
                    break;
                case "answer":
                    var ansMsg = JsonUtility.FromJson<AnswerMessage>(e.Data);
                    Debug.Log($"Answer: {ansMsg.playerId} → {ansMsg.answer}. Correct: {ansMsg.correct}");

                    pendingAnswer = ansMsg;
                    hasPendingAnswer = true;
                    StopCoroutine("QuestionTimer");
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
                    print("Received player list message");
                    var listData = JsonConvert.DeserializeObject<PlayerListMessage>(e.Data);
                    players = listData.players; // overwrite local dictionary
                    Debug.Log($"Updated player list, total: {players.Count}");

                    hasPendingPlayerList = true;
                break;
            }
        };

        ws.Connect();
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

        QuestionMessage question = new QuestionMessage
        {
            type = "question",
            text = "What is 2 + 2?",
            options = new string[] { "2", "3", "4", "5" },
            correctAnswer = "4",
        };

        ws.Send(JsonUtility.ToJson(question));
        Debug.Log("Question sent to phones.");
        StartCoroutine(QuestionTimer(16f));
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
    }

    void OnApplicationQuit()
    {
        ws?.Close();
        wss?.Stop();
    }
}
