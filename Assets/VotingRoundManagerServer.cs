using UnityEngine;
using UnityEngine.UI;

public class VotingRoundManagerServer : MonoBehaviour
{
    public bool started;
    public float timeLeft = 60;
    public Slider timerSlider;
    public bool timerOver;

    void Start()
    {
        timerSlider.maxValue = timeLeft;
    }

    void Update()
    {
        if (!started) return;

        if (timeLeft <= 0)
        {
            timeLeft = 0;

            // Send message once - Cancel voting
            if (!timerOver)
            {
                HostNetwork.instance.BroadcastTimerOver();
                VotingManagerServer.instance.resultsPanel.SetActive(true);
                VotingManagerServer.instance.pitchingPanel.SetActive(false);
                timerOver = true;
            }
            return;
        }

        timeLeft -= Time.deltaTime;
        timerSlider.value = timeLeft;
    }
}
