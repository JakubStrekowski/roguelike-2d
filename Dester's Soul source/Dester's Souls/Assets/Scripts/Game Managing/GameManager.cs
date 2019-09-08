using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int CurrentLevel{get; private set; }
    public int EnemiesKilled { get; set; }
    public int CollectedGold { get; set; }

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
}
