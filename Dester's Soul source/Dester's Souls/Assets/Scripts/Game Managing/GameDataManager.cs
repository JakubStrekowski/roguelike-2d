using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        debugConsoleEnabled = PlayerPrefs.GetInt("DebugMode", 0);
        saveDebugInFile = PlayerPrefs.GetInt("DebugSave", 0);
        usingBreadthAlgorithm = PlayerPrefs.GetInt("BreadthAlgorithm", 0);
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if(debugConsoleEnabled == 0)
        {
            SaveDebugInFile = 0;
            UsingBreadthAlgorithm = 0;
        }
    }

    private int usingBreadthAlgorithm;
    private int saveDebugInFile;
    private int debugConsoleEnabled;
    private int bestScore;
    private float soundVolume;
    private float musicVolume;


    public int UsingBreadthAlgorithm {
        get => usingBreadthAlgorithm;
        set
        {
            PlayerPrefs.SetInt("BreadthAlgorithm", value);
            usingBreadthAlgorithm = value;
            PlayerPrefs.Save();
        }
    }
    public int SaveDebugInFile {
        get => saveDebugInFile;
        set
        {
            PlayerPrefs.SetInt("DebugSave", value);
            saveDebugInFile = value;
            PlayerPrefs.Save();
        }
    }
    public int DebugConsoleEnabled
    {
        get => debugConsoleEnabled;
        set
        {
            PlayerPrefs.SetInt("DebugMode", value);
            debugConsoleEnabled = value;
            PlayerPrefs.Save();
        }
    }
    public int BestScore
    {
        get => bestScore;
        set
        {
            PlayerPrefs.SetInt("BestScore", value);
            bestScore = value;
            PlayerPrefs.Save();
        }
    }

    public float MusicVolume { get => musicVolume; }
    public float SoundsVolume { get => soundVolume; }
    

    public void SetMusicLevel(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        musicVolume = value;
        PlayerPrefs.Save();
    }

    public void SetSoundLevel(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value);
        soundVolume = value;
        PlayerPrefs.Save();

    }


}
