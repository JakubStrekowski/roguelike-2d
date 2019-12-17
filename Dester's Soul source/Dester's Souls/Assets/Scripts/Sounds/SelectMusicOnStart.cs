
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMusicOnStart : MonoBehaviour
{
    public AudioClip[] playlist;
    // Start is called before the first frame update
    void Start()
    {
        int number = GameManager._instance.GetComponent<PlayerDataManager>().previousMusicID;
        int nextSong = Random.Range(0, playlist.Length);
        while (number == nextSong)
        {
            nextSong = Random.Range(0, playlist.Length);
        }
        GetComponent<AudioSource>().clip = playlist[nextSong];
        GetComponent<AudioSource>().Play();
    }

}
