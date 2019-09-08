using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stairs : Tile, IPlayerStandardInteraction
{
    public void OnPlayerInteract(PlayerCharacter source)
    {
        GameManager._instance.GoDownStairs();
    }
}
