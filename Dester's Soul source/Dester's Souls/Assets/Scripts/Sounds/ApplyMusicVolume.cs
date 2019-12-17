using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMusicVolume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().volume = GameManager._instance.GetComponent<GameDataManager>().MusicVolume;
    }

    public void SetMusicVolume(float value)
    {
        GetComponent<AudioSource>().volume = value;
    }
}
