using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagementUI : MonoBehaviour
{
    public void ResetGame()
    {
        GameManager._instance.ResetGame();
    }

    public void QuitGame()
    {
        GameManager._instance.ExitGame();
    }

    public void SelectScene(int sceneID)
    {
        GameManager._instance.LoadScene(sceneID);
    }
}
