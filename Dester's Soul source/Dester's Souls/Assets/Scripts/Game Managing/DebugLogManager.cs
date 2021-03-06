﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class DebugLogManager : MonoBehaviour
{
    string[] currentLog;
    int currentPosition;

    public TextMeshProUGUI debugText;
    public Scrollbar debugScroll;

    private StreamWriter streamWriter;
    private string debugFile;
    // Start is called before the first frame update
    void Awake()
    {
        currentPosition = 0;
        currentLog = new string[30];
    }

    private void Start()
    {
        if (GameManager._instance.GetComponent<GameDataManager>().SaveDebugInFile == 1)
        {
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
            debugFile = ("Logs/GameLog_" + System.DateTime.Now.ToString("MM/dd/yyyy HH_mm") + ".txt");
            streamWriter =  File.AppendText(debugFile);
            if(GameManager._instance.GetComponent<GameDataManager>().UsingBreadthAlgorithm == 1)
            {
                streamWriter.WriteLine("BFS AStar");
            }
            else
            {
                streamWriter.WriteLine("AStar");
            }
        }
    }

  



    public void AddLog(string message, string fileMessage="")
    {
        if (currentPosition == 30)
        {
            debugText.text = "";
            currentPosition = 0;
        }
        string newMessage = (currentPosition + 1).ToString() + ". " + message;
        currentLog[currentPosition] = newMessage;
        string fullMessage = debugText.text + currentLog[currentPosition] + '\n';
        /*
        if(currentPosition<5)
        {
            debugScroll.value = 1;
        }
        else
        {
            debugScroll.value = (float)1 - ((currentPosition) / 29f);
        }*/
        currentPosition++;
        debugText.SetText(fullMessage);
        if(GameManager._instance.GetComponent<GameDataManager>().SaveDebugInFile == 1 && fileMessage != "")
        {
            streamWriter.Write(fileMessage);
        }
    }


    public void RefreshLog()
    {
        if (GameManager._instance.GetComponent<GameDataManager>().DebugConsoleEnabled == 1)
        {
            debugText.text = "";
            foreach (string str in currentLog)
            {
                debugText.SetText(debugText.text + str + System.Environment.NewLine);
            }

        }
    }

    public void SaveLog()
    {
        if (!(streamWriter is null))
            streamWriter.Close();
    }

    private void OnApplicationQuit()
    {
        if(!(streamWriter is null))
        streamWriter.Close();
    }

}
