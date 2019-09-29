using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int CurrentLevel{get; private set; }
    public int EnemiesKilled {
        get
        {
            return gameObject.GetComponent<PlayerDataManager>().enemyKilled;
        }
        set
        {
            gameObject.GetComponent<PlayerDataManager>().enemyKilled = value;
            if (OnKillsChange != null)
                OnKillsChange();
        }
    }
    public delegate void OnKillsChangeDelegate();
    public event OnKillsChangeDelegate OnKillsChange;

    public int CollectedGold
    {
        get
        {
            return gameObject.GetComponent<PlayerDataManager>().gold;
        }
        set
        {
            gameObject.GetComponent<PlayerDataManager>().gold = value;
            if (OnGoldChange != null)
                OnGoldChange();
        }
    }
    public delegate void OnGoldChangeDelegate();
    public event OnGoldChangeDelegate OnGoldChange;



    public static GameManager _instance;
    public TileFactory tileFactory;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        CurrentLevel = 1;
    }

    public void GoDownStairs()
    {
        CurrentLevel++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetGame()
    {
        CurrentLevel=1;
        GetComponent<PlayerDataManager>().ResetValues();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
