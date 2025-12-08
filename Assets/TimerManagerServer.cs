using UnityEngine;
using UnityEngine.UI;

public class TimerManagerServer : MonoBehaviour
{
    public float timeLeft = 60;
    public Slider timerSlider;
    public bool timerOver;

    void Start()
    {
        timerSlider.maxValue = timeLeft;
    }

    void Update()
    {
        if (timeLeft <= 0)
        {
            timeLeft = 0;

            // Send message once
            if (!timerOver)
            {
                HostNetwork.instance.BroadcastTimerOver();
                timerOver = true;
            }
            return;
        }

            timeLeft -= Time.deltaTime;
        timerSlider.value = timeLeft;

    }
}
