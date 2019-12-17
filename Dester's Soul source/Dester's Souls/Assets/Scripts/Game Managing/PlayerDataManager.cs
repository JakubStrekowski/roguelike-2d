using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        ResetValues();
    }

    public int playerHealth=4;
    public int gold=0;
    public int enemyKilled=0;
    public int attackValue=1;
    public int vision = 5;

    public int previousMusicID;

    public Item[] equipment;

    public void ResetValues()
    {
        previousMusicID = 0;

        playerHealth = 4;
        gold = 0;
        enemyKilled = 0;
        attackValue=1;
        vision = 5;

        for(int i = 0; i < equipment.Length; i++)
        {
            if (equipment[i] != null)
            {
                GameObject.Destroy(equipment[i].gameObject);
                equipment[i] = null;
            }
        }

    equipment = new Item[6];
    }
}
