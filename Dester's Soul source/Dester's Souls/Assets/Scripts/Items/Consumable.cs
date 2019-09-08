using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : Item
{
    public int numberOfUses;

    public abstract void UseItem(Character source);
}
