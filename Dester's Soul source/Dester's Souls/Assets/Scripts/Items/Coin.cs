using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    protected override void OnItemPickup(PlayerCharacter source)
    {
        GameManager._instance.CollectedGold += Random.Range(12, 26);
        currentMap.DeleteItem(posX, posY);
        Destroy(gameObject);
    }

    public override void UseItem(Character source)
    {
        
    }
}
