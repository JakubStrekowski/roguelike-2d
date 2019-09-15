using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPositionInitializer
{
    void InitializePosition(int x, int y, Map currentMap, TurnManager tm);
}
