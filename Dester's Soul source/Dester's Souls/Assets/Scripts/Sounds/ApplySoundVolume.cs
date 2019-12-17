using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplySoundVolume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().volume = GameManager._instance.GetComponent<GameDataManager>().SoundsVolume;
    }

    public void SetSoundVolume(float value)
    {
        GetComponent<AudioSource>().volume = value;
    }
}
