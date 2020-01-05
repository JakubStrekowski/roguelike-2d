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
    }


    private int debugConsoleEnabled;
    private float soundVolume;
    private float musicVolume;

    public int DebugConsoleEnabled { get => debugConsoleEnabled; set => debugConsoleEnabled = value; }
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

    public void SetDebugMode(int value)
    {
        PlayerPrefs.SetInt("DebugMode", value);
        debugConsoleEnabled = value;
        PlayerPrefs.Save();
    }
}
