using TMPro;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager instance;

    public GameObject questionDisplay;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI timerText;
    public GameObject[] answers;
    public int playersAnswered;

    public GameObject sendQuestionButton;

    private QuestionMessage currentQuestion;

    private void Awake()
    {
        instance = this;
    }

    public void DisplayQuestion(QuestionMessage msg)
    {
        currentQuestion = msg;
        questionDisplay.SetActive(true);
        questionText.text = msg.text;

        for (int i = 0; i < msg.options.Length; i++)
        {
            answers[i].SetActive(true);
            answers[i].GetComponentInChildren<TextMeshProUGUI>().text = msg.options[i];

            // Detect if option is correct
        }

    }

    /// <summary>
    /// Called when a player selects an answer button
    /// </summary>
    public void SendAnswer(TextMeshProUGUI text)
    {
        if (WebSocketClient.instance != null)
        {
            var answerMsg = new AnswerMessage
            {
                type = "answer",
                playerId = WebSocketClient.instance.localPlayerId, // make sure you store this somewhere when joining
                answer = text.text, // or the actual answer text
                correct = currentQuestion.correctAnswer == text.text ? true : false,
            };

            string json = JsonUtility.ToJson(answerMsg);
            WebSocketClient.instance.SendMessageToServer(json);
        }

        // Hide question
        questionDisplay.SetActive(false);
        sendQuestionButton.SetActive(true);
    }
}
