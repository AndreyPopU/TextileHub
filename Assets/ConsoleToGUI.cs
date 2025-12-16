using UnityEngine;

public class ConsoleToGUI : MonoBehaviour
{
    static string myLog = "";
    private string output;

    void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        output = logString;
        myLog += output + " " + myLog;
        if (myLog.Length > 5000)
        {
            myLog = myLog.Substring(0, 4000);
        }
    }

    void OnGUI() => myLog = GUI.TextArea(new Rect(0, Screen.height / 2, Screen.width, Screen.height / 2), myLog);
}