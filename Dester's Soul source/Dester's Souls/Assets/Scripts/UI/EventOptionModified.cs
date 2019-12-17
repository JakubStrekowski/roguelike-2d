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


}
