using UnityEngine;
using WebSocketSharp.Server;
using System.Collections;

public class HostNetwork : MonoBehaviour
{
    public static HostNetwork instance;

    public WebSocketServer wss;
    private Coroutine runningCoroutine;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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

    public void AllowVoting(string presentingPlayerId)
    {
        var allowVotingMsg = new GameMessage
        {
            type = "allowvoting",
            text = presentingPlayerId,
        };

        wss.WebSocketServices["/lobby"].Sessions.Broadcast(JsonUtility.ToJson(allowVotingMsg));
    }

    public void StartGame()
    {
        var gameStartMessage = new GameStartMessage
        {
            type = "gameStart",
            sceneIndex = 2,
        };

        wss.WebSocketServices["/lobby"].Sessions.Broadcast(JsonUtility.ToJson(gameStartMessage));
    }

    public void NextMinigame(int sceneIndex)
    {
        var gameStartMessage = new GameStartMessage
        {
            type = "gameStart",
            sceneIndex = sceneIndex,
        };

        wss.WebSocketServices["/lobby"].Sessions.Broadcast(JsonUtility.ToJson(gameStartMessage));
    }

    public void AssignRoles()
    {
        // Pick a scene index to send to each player - possibly the most scuffed for loop ever
        for (int i = 5; i < 9; i++)
        {
            if (i - 5 >= LobbyBehavior.GetAllPlayerIds().Count) return; // If less than 4 players have connected return;

            var gameStartMessage = new GameStartMessage
            {
                type = "gameStart",
                sceneIndex = i,
            };

            //wss.WebSocketServices["/lobby"].Sessions.SendTo(JsonUtility.ToJson(gameStartMessage), LobbyBehavior.GetAllPlayerIds()[i - 5]);
            LobbyBehavior.SendToPlayer(LobbyBehavior.GetAllPlayerIds()[i - 5], gameStartMessage);
        }
    }

    // -------------------------
    // SEND QUESTION TO ALL CLIENTS
    // -------------------------
    public void SendQuestion()
    {
        if (wss == null)
        {
            Debug.LogWarning("[Host] WebSocketServer is null, cannot send question.");
            return;
        }

        QuestionManager.instance.playersAnswered = 0;

        // AskNextQuestion should return a QuestionMessage
        QuestionMessage question = QuestionManager.instance.AskNextQuestion();

        string json = JsonUtility.ToJson(question);
        wss.WebSocketServices["/lobby"].Sessions.Broadcast(json);
        Debug.Log("[Host] Question sent to phones.");

        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);

        runningCoroutine = StartCoroutine(QuestionTimer(16f));
    }

    // -------------------------
    // TIMER / COUNTDOWN
    // -------------------------
    IEnumerator QuestionTimer(float duration)
    {
        float timeLeft = duration;

        while (timeLeft > 0)
        {
            timeLeft -= 1f;

            var timerMsg = new GameMessage
            {
                type = "timer",
                text = timeLeft.ToString()
            };

            wss.WebSocketServices["/lobby"].Sessions.Broadcast(JsonUtility.ToJson(timerMsg));
            yield return new WaitForSeconds(1f);
        }

        var endMsg = new GameMessage
        {
            type = "timeUp",
            text = ""
        };

        wss.WebSocketServices["/lobby"].Sessions.Broadcast(JsonUtility.ToJson(endMsg));
        runningCoroutine = null;
    }

    public void StartResultsCountdown(float duration)
    {
        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);

        runningCoroutine = StartCoroutine(ResultsCountdown(duration));
    }

    IEnumerator ResultsCountdown(float duration)
    {
        // Tell host UI to show results
        QuestionManager.instance.DisplayResults();

        var startMsg = new GameMessage
        {
            type = "countdownStart",
            text = duration.ToString()
        };
        wss.WebSocketServices["/lobby"].Sessions.Broadcast(JsonUtility.ToJson(startMsg));

        yield return new WaitForSeconds(duration);

        var endMsg = new GameMessage
        {
            type = "countdownEnd",
            text = ""
        };
        wss.WebSocketServices["/lobby"].Sessions.Broadcast(JsonUtility.ToJson(endMsg));

        QuestionManager.instance.EnableResultScreen(false);

        // Send next question
        SendQuestion();

        runningCoroutine = null;
    }

    private void OnApplicationQuit() => StopServer();

    public void StopServer()
    {
        if (wss == null) return;

        var shutdownMsg = new GameMessage
        {
            type = "servershutdown",
            text = "Server closed by host"
        };

        wss.WebSocketServices["/lobby"].Sessions.Broadcast(JsonUtility.ToJson(shutdownMsg));

        // 2) Actually stop the server (this closes all client sockets)
        wss.Stop();
        wss = null;

        Debug.Log("[Host] Server stopped, clients should disconnect.");
    }

    public void BroadcastTimerOver()
    {
        if (wss == null) return;

        var timerOverMsg = new GameMessage
        {
            type = "timerover",
        };

        wss.WebSocketServices["/lobby"].Sessions.Broadcast(JsonUtility.ToJson(timerOverMsg));
        Debug.Log("[Server] Timer Over sent to clients");
    }
}
