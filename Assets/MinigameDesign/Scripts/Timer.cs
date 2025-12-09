using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float time_remaining = 10;
    bool timerI_running = true;
    [SerializeField] TextMeshProUGUI text;

    private void Start()
    {
        // Starts the timer automatically
        timerI_running = true;
    }

    void Update()
    {
        if (timerI_running)
        {
            if (time_remaining > 0)
            {
                time_remaining -= Time.deltaTime;
                DisplayTime(time_remaining);
            }
            else
            {
                time_remaining = 0;
                timerI_running = false;
                text.text = "Time has run out!";
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}