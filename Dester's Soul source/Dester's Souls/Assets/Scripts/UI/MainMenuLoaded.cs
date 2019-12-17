using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLoaded : MonoBehaviour
{
    public GameObject loadingScreen;
    // Start is called before the first frame update
    void Start()
    {
        loadingScreen.SetActive(true);
        loadingScreen.GetComponent<LoadingScreen>().FadeOut();
    }
}
