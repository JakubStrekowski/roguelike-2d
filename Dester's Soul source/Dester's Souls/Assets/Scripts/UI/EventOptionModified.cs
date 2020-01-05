using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class EventOptionModified : MonoBehaviour
{
    
    public ApplyMusicVolume amVolume;
    public ApplySoundVolume soundVolume;
    public Slider musicSlider;
    public Slider soundSlider;
    public Toggle debugModeToggle;

    public GameDataManager gdManager;

    private void Awake()
    {
        if (GameManager._instance != null)
        {
            gdManager = GameManager._instance.GetComponent<GameDataManager>();
        }
    }

    public void Start()
    {
        musicSlider.value = GameManager._instance.GetComponent<GameDataManager>().MusicVolume;
        soundSlider.value = GameManager._instance.GetComponent<GameDataManager>().SoundsVolume;
        debugModeToggle.isOn = GameManager._instance.GetComponent<GameDataManager>().DebugConsoleEnabled > 0 ? true : false;
    }

    public void UpdateMusicLevel(float value)
    {
        gdManager.SetMusicLevel(value);
        amVolume.SetMusicVolume(value);
    }

    public void UpdateSoundLevel(float value)
    {
        gdManager.SetSoundLevel(value);
        soundVolume.SetSoundVolume(value);
    }

    public void UpdateDebugMode(bool value)
    {
        if (value)
        {
            gdManager.SetDebugMode(1);
        }
        else
        {
            gdManager.SetDebugMode(0);
        }
    }


}
