using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    protected override void OnItemPickup(PlayerCharacter source)
    {
        GameManager._instance.CollectedGold += Random.Range(12, 26);
        Destroy(gameObject);
    }
}
