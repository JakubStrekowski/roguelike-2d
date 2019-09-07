using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, IPositionInitializer
{
    protected int posX;
    protected int posY;
    public bool isPassable;

    public void InitializePosition(int x, int y,Map currentMap)
    {
        posX = x;
        posY = y;
    }
}
