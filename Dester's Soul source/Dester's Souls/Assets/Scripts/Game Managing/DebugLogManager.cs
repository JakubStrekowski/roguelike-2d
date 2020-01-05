using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugLogManager : MonoBehaviour
{
    string[] currentLog;
    int currentPosition;

    public TextMeshProUGUI debugText;
    // Start is called before the first frame update
    void Awake()
    {
        currentPosition = 0;
        currentLog = new string[30];
    }


   
    public void AddLog(string message)
    {
        if (currentPosition == 30)
        {
            debugText.text = "";
            currentPosition = 0;
        }
        string newMessage = (currentPosition + 1).ToString() + ". " + message;
        currentLog[currentPosition] = newMessage;
        string fullMessage = debugText.text + currentLog[currentPosition] + System.Environment.NewLine;
        currentPosition++;
        debugText.SetText(fullMessage);
    }


    public void RefreshLog()
    {
        debugText.text = "";
        foreach(string str in currentLog)
        {
            debugText.SetText(debugText.text + str + System.Environment.NewLine);
        }
    }
}
