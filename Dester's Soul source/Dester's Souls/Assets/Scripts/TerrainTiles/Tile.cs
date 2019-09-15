using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, IPositionInitializer, IVisibility
{
    protected int posX;
    protected int posY;
    protected bool wasVisited;
    public bool isPassable;
    public int variant;

    private SpriteRenderer sr;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        TurnInvisible();
    }

    public void InitializePosition(int x, int y,Map currentMap, TurnManager tm)
    {
        posX = x;
        posY = y;
    }

    public void TurnVisible()
    {
        wasVisited = true;
        sr.enabled = true;
        sr.color = new Color(1f, 1f, 1f);
    }

    public void TurnInvisible()
    {/*
        if (wasVisited)
        {
            sr.color = new Color(0.5f, 0.5f, 0.5f);
        }
        else
        {
            sr.enabled = false;
        }*/
    }
}
