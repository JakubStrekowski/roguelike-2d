using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stairs : Tile, IPlayerStandardInteraction
{
    public override void OnPlayerInteract(PlayerCharacter source)
    {
        source.SetDataValues();
        GameManager._instance.GoDownStairs();
    }
}
