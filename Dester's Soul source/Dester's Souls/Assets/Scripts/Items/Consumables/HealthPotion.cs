using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Consumable
{
    public int healAmmount;

    public override void UseItem(Character source)
    {
        source.HealthPoints += healAmmount;
        numberOfUses--;
    }
}
