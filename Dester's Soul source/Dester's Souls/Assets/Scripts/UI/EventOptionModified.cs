using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class EventOptionModified : MonoBehaviour
{
    
    public ApplyMusicVolume amVolume;
    public ApplySoundVolume soundVolume;
    public Slider musicSlider;
    public Slider soundSlider;
    public Toggle debugModeToggle;
    public Toggle saveFileToggle;
    public Toggle bfsToggle;
    public TextMeshProUGUI saveFileTitle;
    public TextMeshProUGUI bfsTitle;
    public GameObject optionsPanel;

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
        saveFileToggle.isOn = GameManager._instance.GetComponent<GameDataManager>().SaveDebugInFile > 0 ? true : false;
        bfsToggle.isOn = GameManager._instance.GetComponent<GameDataManager>().UsingBreadthAlgorithm > 0 ? true : false;
        if(GameManager._instance.GetComponent<GameDataManager>().DebugConsoleEnabled == 0)
        {
            saveFileToggle.gameObject.SetActive(false);
            bfsToggle.gameObject.SetActive(false);
            saveFileTitle.enabled = false;
            bfsTitle.enabled = false;
        }
        optionsPanel.SetActive(false);


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
        gdManager.DebugConsoleEnabled = value ? 1 : 0;
        if(value is false)
        {
            UpdateSaveInFile(false);
            UpdateBreadthAlgorithm(false);
            saveFileToggle.gameObject.SetActive(false);
            bfsToggle.gameObject.SetActive(false);
            saveFileTitle.enabled = false;
            bfsTitle.enabled = false;
        }
        else
        {
            saveFileToggle.gameObject.SetActive(true);
            bfsToggle.gameObject.SetActive(true);
            saveFileTitle.enabled = true;
            bfsTitle.enabled = true;
        }
    }

    public void UpdateSaveInFile(bool value)
    {
        gdManager.SaveDebugInFile = value ? 1 : 0;
    }

    public void UpdateBreadthAlgorithm(bool value)
    {
        gdManager.UsingBreadthAlgorithm = value ? 1 : 0;
    }
}
