using System;
using System.Collections.Generic;

[Serializable]
public class PlayerMessage
{
    public string type;
    public string playerId;
    public string action;
}

[Serializable]
public class PlayerListMessage
{
    public string type;
    public Dictionary<string, string> players;
}

[Serializable]
public class AnswerMessage
{
    public string type;
    public string playerId;
    public string answer;
    public bool correct;
}

[Serializable]
public class QuestionMessage
{
    public string type;
    public string text;
    public string[] options;
    public string correctAnswer;
}

[Serializable]
public class GameStartMessage
{
    public string type;
    public int sceneIndex;
}

[Serializable]
public class VotingMessage
{
    public string type;
    public int[] votingResults;
}

[Serializable]
public class FinalDesignMessage
{
    public string type;
    public string playerId;
    public int[] designResults;
    public string primaryHex;
    public string secondaryHex;
}

[Serializable]
public class PlayerJoinMessage // Server joined player
{
    public string type;
    public string playerId;
    public string name;
}

[Serializable]
public class PlayerJoinedMessage
{
    public string type;
    public string playerId;
    public string name;
}

[Serializable]
public class PlayerData
{
    public string playerId;
    public string name;
    public int score = 0;
}

[Serializable]
public class GameMessage
{
    public string type;
    public string text;
}

// Serializable scoreboard container for client
[Serializable]
public class ScoreboardMessage
{
    public string type;
    public Dictionary<string, int> scores;
    public Dictionary<string, string> names;
}
