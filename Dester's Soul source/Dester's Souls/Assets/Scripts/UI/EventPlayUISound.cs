using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPlayUISound : MonoBehaviour
{
    /*
     * Audio UI Effects:
     * 0-option select
     * 1-audio test
     * 2-back?
     * */
    public AudioClip[] audioEffects;

    public void PlaySound(int soundID)
    {
        GetComponent<AudioSource>().PlayOneShot(audioEffects[soundID]);
    }
}
